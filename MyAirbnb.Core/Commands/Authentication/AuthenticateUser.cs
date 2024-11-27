using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAirbnb.DataAccess;
using MyAirbnb.Models.DTOs.Authentication;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using static MyAirbnb.Models.ObjectValidationHelper;

namespace MyAirbnb.Core.Commands.Authentication;

public class AuthenticateUser
{
    public record Command(AuthenticationRequestDTO Credentials) : IRequest<ErrorOr<AuthenticationResponseDTO>> {}

    public sealed class Handler: IRequestHandler<Command, ErrorOr<AuthenticationResponseDTO>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public Handler(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ErrorOr<AuthenticationResponseDTO>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var errorMessage = ValidateAndGetFirstErrorMessage(request.Credentials);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Errors.InvalidDataModel(errorMessage);
                }

                var userByCredentials = await _context.Users
                    .Where(x => string.Equals(x.UserName, request.Credentials.UserName)
                    && string.Equals(x.Password, request.Credentials.Password)).AsNoTracking().FirstOrDefaultAsync();

                if (userByCredentials is null)
                {
                    return Errors.InvalidCredentials;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userByCredentials.UserName),
                    new Claim(ClaimTypes.NameIdentifier, userByCredentials.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"]!,
                    audience: _configuration["JWT:Audience"]!,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenHandler = new JwtSecurityTokenHandler();
                var accessToken = tokenHandler.WriteToken(token);

                return new AuthenticationResponseDTO()
                {
                    AccessToken = accessToken
                };
            }
            catch (Exception ex)
            {
                return Error.Failure(description: ex.Message);
            }
        }
    }

    public class Errors
    {
        public static Error InvalidDataModel(string errorMessage) => Error.Conflict(ErrorCodes.InvalidDataModel, errorMessage);
        public static readonly Error InvalidCredentials = Error.NotFound(ErrorCodes.InvalidCredentials, "Invalid Credentials!");
    }

    public class ErrorCodes
    {
        public const string InvalidDataModel = "[[INVALID_DATA_MODEL]]";
        public const string InvalidCredentials = "[[INVALID_CREDENTIALS]]";
    }
}
