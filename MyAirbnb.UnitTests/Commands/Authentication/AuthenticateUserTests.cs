using Microsoft.Extensions.Configuration;
using Moq;
using MyAirbnb.Core.Commands.Authentication;
using MyAirbnb.DataAccess;
using MyAirbnb.Models.DTOs.Authentication;
using Xunit;

namespace MyAirbnb.UnitTests.Commands.Authentication;

public class AuthenticateUserTests : IDisposable
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthenticateUser.Handler _handler;
    private readonly ApplicationDbContext _context;

    public AuthenticateUserTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _context = ApplicationDbContextHelper.Create();
        _handler = new AuthenticateUser.Handler(_context, _mockConfiguration.Object);
    }

    /// <summary>
    /// Unit Test for Valid Authentication Response
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthenticationResponse()
    {
        // Arrange
        ApplicationDbContextHelper.SeedUser(_context, 1);
        var credentials = new AuthenticationRequestDTO { UserName = "testuser", Password = "password" };

        _mockConfiguration.Setup(c => c["JWT:SecretKey"]).Returns("mYsUPERsECretKeYYYmYsUPERsECretKeYYY");
        _mockConfiguration.Setup(c => c["JWT:Issuer"]).Returns("MyAirBnb_Issuer");
        _mockConfiguration.Setup(c => c["JWT:Audience"]).Returns("MyAirBnb_Audience");

        var command = new AuthenticateUser.Command(credentials);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.IsType<AuthenticationResponseDTO>(result.Value);
        Assert.NotEmpty(result.Value.AccessToken);
    }

    /// <summary>
    /// Unit Test for Invalid Request DTO
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_InvalidRequestDTO_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var credentials = new AuthenticationRequestDTO { UserName = null, Password = "password" };

        _mockConfiguration.Setup(c => c["JWT:SecretKey"]).Returns("mYsUPERsECretKeYYYmYsUPERsECretKeYYY");
        _mockConfiguration.Setup(c => c["JWT:Issuer"]).Returns("MyAirBnb_Issuer");
        _mockConfiguration.Setup(c => c["JWT:Audience"]).Returns("MyAirBnb_Audience");

        var command = new AuthenticateUser.Command(credentials);

        // Act
        var result = await _handler.Handle(command, default);


        // Assert
        Assert.True(result.IsError);
        Assert.Equal(AuthenticateUser.ErrorCodes.InvalidDataModel, result.FirstError.Code);
    }

    /// <summary>
    /// Unit Test for Invalid Credential Response
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var credentials = new AuthenticationRequestDTO { UserName = "testuser", Password = "wrongpassword" };

        _mockConfiguration.Setup(c => c["JWT:SecretKey"]).Returns("mYsUPERsECretKeYYYmYsUPERsECretKeYYY");
        _mockConfiguration.Setup(c => c["JWT:Issuer"]).Returns("MyAirBnb_Issuer");
        _mockConfiguration.Setup(c => c["JWT:Audience"]).Returns("MyAirBnb_Audience");

        var command = new AuthenticateUser.Command(credentials);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(AuthenticateUser.Errors.InvalidCredentials, result.FirstError);
    }

    public void Dispose()
    {
        ApplicationDbContextHelper.Destroy(_context);
    }
}
