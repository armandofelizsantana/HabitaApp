using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Habita.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Habita.Api.Tests.Integration;

public sealed class JwtValidationTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public JwtValidationTests(ApiFactory factory) => _factory = factory;

    [Theory]
    [InlineData("expired")]
    [InlineData("issuer")]
    [InlineData("audience")]
    [InlineData("signature")]
    [InlineData("unsigned")]
    [InlineData("malformed")]
    public async Task InvalidToken_Returns401(string scenario)
    {
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(scenario));
        var response = await client.GetAsync("/api/v1/agents");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ValidToken_WithClientRole_Returns403()
    {
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken("role"));
        var response = await client.GetAsync("/api/v1/agents");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private string CreateToken(string scenario)
    {
        if (scenario == "malformed")
            return "invalid.jwt.token";

        var settings = _factory.Services.GetRequiredService<IOptions<JwtSettings>>().Value;
        var key = scenario == "signature" ? new string('x', 64) : settings.SecretKey;
        var credentials = scenario == "unsigned" ? null : new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: scenario == "issuer" ? "untrusted-issuer" : settings.Issuer,
            audience: scenario == "audience" ? "another-audience" : settings.Audience,
            claims: new[] { new Claim(ClaimTypes.Role, scenario == "role" ? "Client" : "Admin") },
            expires: DateTime.UtcNow.AddMinutes(scenario == "expired" ? -10 : 10),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
