using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VisitorMGMT.API.BusinessLogic.Interfaces;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Interfaces;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.BusinessLogic.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly IVisitorRepository _visitorRepository;
        private readonly IConfiguration _configuration;
        private readonly IValidator<Profile> _profileValidator;

        public VisitorService(IVisitorRepository visitorRepository, IConfiguration configuration,IValidator<Profile> profileValidator)
        {
            _visitorRepository = visitorRepository;
            _configuration = configuration;
            _profileValidator = profileValidator;
        }

        public async Task<string> AuthenticateAsync(LoginDTO visitorLogin)
        {
            string authenticatedVisitor = await AuthenticateVisitorAsync(visitorLogin);
            return authenticatedVisitor;
        }

        public async Task<Visitor> SignUpNewAsync(VisitorDTO visitorDTO)
        {
            bool userExists = await _visitorRepository.CheckIfVisitorExistsAsync(visitorDTO.Username);
            if(userExists == true)
            {
                throw new Exception($"User with the username {visitorDTO.Username} already exists. Try another one.");
            }

            Visitor visitor = await CreateNewAccount(visitorDTO.Username, visitorDTO.Password, visitorDTO.SignUpProfile);

            if(visitor.Profile != null)
            {
                await RunFluentValidation(visitor.Profile);
            }

            await _visitorRepository.AddAsync(visitor);
            
            return visitor;
        }

        private async Task<Visitor> CreateNewAccount(string username, string password, ProfileDTO signUpProfile)
        {
            if(username == null || password == null)
            {
                throw new ArgumentNullException("Please enter username and password.");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            Visitor visitor = new Visitor()
            {
                UserName = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Profile = new Profile()
                {
                    FirstName = signUpProfile.FirstName,
                    LastName = signUpProfile.LastName,
                    IdentityNumber = signUpProfile.IdentityNumber,
                    PhoneNumber = signUpProfile.PhoneNumber,
                    Email = signUpProfile.Email
                }
            };

            return visitor;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }

        private async Task<string> GenerateTokenAsync(Visitor visitor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, visitor.UserName),
                new Claim(ClaimTypes.Role, visitor.Role)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims : claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> AuthenticateVisitorAsync(LoginDTO visitorLogin)
        {
            var currentVisitor = await _visitorRepository.GetVisitorByUserNameAsync(visitorLogin.Username);           

            if(!VerifyPasswordHash(visitorLogin.Password, currentVisitor.PasswordHash, currentVisitor.PasswordSalt))
            {
                throw new ArgumentException("Username or password is incorrect");
            }

            return await GenerateTokenAsync(currentVisitor);
        }

        public async Task DeleteAsync(Guid visitorId)
        {
            await _visitorRepository.DeleteAsync(visitorId);
        }          
        
        public async Task<Visitor> GetVisitorByIdAsync(Guid id)
        {
            return await _visitorRepository.GetVisitorByIdAsync(id);
        }

        public async Task<Visitor> GetVisitorByUserNameAsync(string username)
        {
            return await _visitorRepository.GetVisitorByUserNameAsync(username);
        }
        private async Task RunFluentValidation(Profile profile)
        {
            var result = await _profileValidator.ValidateAsync(profile);
            if (!result.IsValid)
            {
                FluentValidation.Results.ValidationResult validationResult = result;
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }
        }
    }
}
