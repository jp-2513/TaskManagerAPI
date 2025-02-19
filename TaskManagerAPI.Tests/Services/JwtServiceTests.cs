using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public JwtServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("SuperSecretTestKeySuperSecretTestKey");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");
            _mockConfiguration.Setup(config => config["Jwt:ExpireHours"]).Returns("1");

            _jwtService = new JwtService(_mockConfiguration.Object);
        }

        [Fact]
        public void GenerateJwtToken_Should_Return_Valid_Token()
        {
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com"
            };

            var token = _jwtService.GenerateJwtToken(user);

            token.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.Should().NotBeNull();
            jwtToken.Issuer.Should().Be("TestIssuer");
            jwtToken.Audiences.Should().Contain("TestAudience");

            var claims = jwtToken.Claims.ToList();
            claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "1");
            claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "test@example.com");
            claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "1");
            claims.Should().Contain(c => c.Type == "name" && c.Value == "Test User");
        }

        [Fact]
        public void GenerateJwtToken_Should_Have_Valid_Signature()
        {

            var user = new User
            {
                Id = 2,
                Name = "Another User",
                Email = "another@example.com"
            };

            var token = _jwtService.GenerateJwtToken(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_mockConfiguration.Object["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _mockConfiguration.Object["Jwt:Issuer"],
                ValidAudience = _mockConfiguration.Object["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };


            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            validatedToken.Should().NotBeNull();
            principal.Should().NotBeNull();
            principal.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "2");
        }
    }
}
