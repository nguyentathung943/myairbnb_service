using MyAirbnb.API.Extensions;
using MyAirbnb.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Application Logging
builder.Services.ConfigureApplicationLogging(builder.Configuration);

// Configure Application Authentication
builder.Services.ConfigureApplicationAuthentication(builder.Configuration);

// Register Services
builder.Services.RegisterApplicationServices(builder.Configuration);

// Register MediatR
builder.Services.RegisterMediatR();

// CORS
builder.Services.AddCors();

// Configure Swagger
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(builder =>
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
