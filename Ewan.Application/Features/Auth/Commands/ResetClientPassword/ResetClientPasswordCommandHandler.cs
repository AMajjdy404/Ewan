using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Services;
using Ewan.Core.Specifications;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using CoreClient = global::Ewan.Core.Models.Client;

namespace Ewan.Application.Features.Auth.Commands.ResetClientPassword
{
    public class ResetClientPasswordCommandHandler : IRequestHandler<ResetClientPasswordCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<CoreClient> _passwordHasher;
        private readonly ITokenService _tokenService;

        public ResetClientPasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher<CoreClient> passwordHasher,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(ResetClientPasswordCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var clientRepo = _unitOfWork.Repository<global::Ewan.Core.Models.Client>();
            var resetTokenRepo = _unitOfWork.Repository<ClientPasswordResetToken>();

            var client = await clientRepo.GetEntityWithSpec(
                new ClientByEmailSpecification(request.Email.Trim()));

            if (client == null)
                throw new UnauthorizedAccessException("Invalid reset request");

            var tokenHash = ComputeSha256Hash(request.Code.Trim());

            var resetToken = await resetTokenRepo.GetEntityWithSpec(
                new ClientPasswordResetTokenByHashSpecification(client.Id, tokenHash));

            if (resetToken == null)
                throw new SecurityTokenException("Invalid or expired reset code");

            client.PasswordHash = _passwordHasher.HashPassword(client, request.NewPassword);
            clientRepo.Update(client);

            resetToken.IsUsed = true;
            resetToken.UsedAt = DateTime.UtcNow;
            resetTokenRepo.Update(resetToken);

            await _unitOfWork.SaveChangesAsync();

            await _tokenService.RevokeAllUserRefreshTokensAsync(
                ownerId: client.Id.ToString(),
                userType: "Client",
                ipAddress: command.IpAddress,
                reason: "Password reset");

            return Unit.Value;
        }

        private static string ComputeSha256Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input.Trim());
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}