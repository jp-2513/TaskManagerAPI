using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Serilog;

namespace TaskManagerAPI.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IValidator<RegisterUserDTO> _validator;

        public AuthService(IUserRepository userRepository, JwtService jwtService, IValidator<RegisterUserDTO> validator)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _validator = validator;
        }

        public async Task<string> RegisterAsync(RegisterUserDTO dto)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Erro de validação no registro: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                Log.Warning("Tentativa de registro com e-mail já existente: {Email}", dto.Email);
                throw new ArgumentException("E-mail já registrado.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(newUser);
            Log.Information("Usuário registrado: {Email}", dto.Email);

            return _jwtService.GenerateJwtToken(newUser);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                Log.Warning("Tentativa de login falhou: {Email}", email);
                throw new ArgumentException("Credenciais inválidas.");
            }
            Log.Information("Usuário autenticado: {Email}", email);
            return _jwtService.GenerateJwtToken(user);
        }
    }
}
