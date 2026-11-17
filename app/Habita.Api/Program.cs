using System.Text;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Habita.Api.Extensions;
using Habita.Api.Middlewares;
using Habita.Api.Policies;
using Habita.Application;
using Habita.Domain.Common;
using Habita.Domain.Settings;
using Habita.Infrastructure.Identity;
using Habita.Infrastructure.Persistence;
using Habita.Infrastructure.Shared;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers(options =>
    {
        options.ConfigureModelBindingMessages();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.ConfigureInvalidModelStateResponse();
    });

builder.Services.AddAuthorization(options =>
{
    options.AddApiAuthorizationPolicies();
});

builder
    .Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version")
        );
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";                      
        options.SubstituteApiVersionInUrl = true;                                                                          
    });

builder.Services.AddOpenApi();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddIdentityForWebApi(builder.Configuration);

var allowedOrigins =
    builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:3000", "http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DefaultPolicy",
        policy =>
        {
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }
    );
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.Services.RunIdentitySeedAsync();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;
    var (code, message) = response.StatusCode switch
    {
        StatusCodes.Status401Unauthorized => ("UNAUTHORIZED", "Se requiere autenticación para acceder a este recurso."),
        StatusCodes.Status403Forbidden => ("FORBIDDEN", "No tienes permiso para realizar esta acción."),
        StatusCodes.Status404NotFound => ("NOT_FOUND", "El recurso solicitado no fue encontrado."),
        _ => ("HTTP_ERROR", "No se pudo completar la solicitud."),
    };

    await response.WriteAsJsonAsync(ApiResponse.Failure(code, message));
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
}

app.UseCors("DefaultPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.MapControllers();

await app.RunAsync();

public partial class Program { }
