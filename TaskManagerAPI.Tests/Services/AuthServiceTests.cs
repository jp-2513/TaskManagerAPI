using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<JwtService> _mockJwtService;
        private readonly Mock<IValidator<RegisterUserDTO>> _mockValidator;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtService = new Mock<JwtService>(Mock.Of<IConfiguration>());
            _mockValidator = new Mock<IValidator<RegisterUserDTO>>();

            _authService = new AuthService(
                _mockUserRepository.Object,
                _mockJwtService.Object,
                _mockValidator.Object
            );
        }


        [Fact]
        public async Task RegisterAsync_Should_Throw_Exception_On_Invalid_Validation()
        {
            var registerDto = new RegisterUserDTO
            {
                Name = "",
                Email = "invalid-email",
                Password = "123"
            };

            var validationFailures = new ValidationResult(new[]
            {
                new ValidationFailure("Email", "Email inválido"),
                new ValidationFailure("Password", "Senha muito curta")
            });

            _mockValidator
                .Setup(v => v.ValidateAsync(registerDto, default))
                .ReturnsAsync(validationFailures);

            Func<Task> act = async () => await _authService.RegisterAsync(registerDto);

            await act.Should().ThrowAsync<ValidationException>();

            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }



    }
}
