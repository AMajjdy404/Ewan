using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ewan.Application.Features.Auth.Commands.LoginPropertyOwner
{
    public class LoginPropertyOwnerCommandHandler : IRequestHandler<LoginPropertyOwnerCommand, AuthResponseDto>
    {
        private const string PropertyOwnerUserType = "PropertyOwner";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginPropertyOwnerCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Handle(LoginPropertyOwnerCommand command, CancellationToken cancellationToken)
        {
            var phone = command.Request.PhoneNumber.Trim();

            var property = await _unitOfWork.Repository<Property>()
                .FirstOrDefaultAsync(x => x.OwnerPhoneNumber == phone);

            if (property == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var isValidPassword = BCrypt.Net.BCrypt.Verify(command.Request.Password, property.OwnerPasswordHash);
            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid credentials");

            var expiresAtUtc = GetAccessTokenExpirationUtc();
            var accessToken = GenerateJwtToken(property, expiresAtUtc);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = string.Empty,
                UserType = PropertyOwnerUserType,
                UserId = property.Id.ToString(),
                UserName = property.Name,
                ExpiresAtUtc = expiresAtUtc
            };
        }

        private DateTime GetAccessTokenExpirationUtc()
        {
            if (!double.TryParse(_configuration["JWT:AccessTokenDurationInMinutes"], out var minutes))
                throw new InvalidOperationException("Invalid JWT duration.");

            return DateTime.UtcNow.AddMinutes(minutes);
        }

        private string GenerateJwtToken(Property property, DateTime expiresAtUtc)
        {
            var key = _configuration["JWT:Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("JWT Key missing");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, property.Id.ToString()),
                new Claim(ClaimTypes.Name, property.Name),
                new Claim(ClaimTypes.MobilePhone, property.OwnerPhoneNumber),
                new Claim("UserType", PropertyOwnerUserType),
                new Claim("PropertyId", property.Id.ToString())
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expiresAtUtc,
                claims: claims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
