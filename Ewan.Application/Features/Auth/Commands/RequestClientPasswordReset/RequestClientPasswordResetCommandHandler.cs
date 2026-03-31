using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Mail;
using Ewan.Core.Services;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Ewan.Application.Features.Auth.Commands.RequestClientPasswordReset
{
    public class RequestClientPasswordResetCommandHandler : IRequestHandler<RequestClientPasswordResetCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public RequestClientPasswordResetCommandHandler(IUnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task<Unit> Handle(RequestClientPasswordResetCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Request.Email))
                return Unit.Value;

            var clientRepo = _unitOfWork.Repository<Client>();
            var resetTokenRepo = _unitOfWork.Repository<ClientPasswordResetToken>();

            var client = await clientRepo.GetEntityWithSpec(
                new ClientByEmailSpecification(command.Request.Email.Trim()));

            if (client == null)
                return Unit.Value;

            var activeTokens = await resetTokenRepo.ListAsync(
                new ActiveClientPasswordResetTokensSpecification(client.Id));

            if (activeTokens.Count > 0)
            {
                resetTokenRepo.RemoveRange(activeTokens);
                await _unitOfWork.SaveChangesAsync();
            }

            var plainToken = GenerateSecureToken();
            var tokenHash = ComputeSha256Hash(plainToken);

            var resetToken = new ClientPasswordResetToken
            {
                ClientId = client.Id,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
                CreatedByIpAddress = command.IpAddress
            };

            await resetTokenRepo.AddAsync(resetToken);
            await _unitOfWork.SaveChangesAsync();

            var email = new Email
            {
                To = client.Email,
                Subject = "Reset your password",
                IsHtml = true,
                Body = $"""
                        <p>Hello {client.FullName},</p>
                        <p>We received a request to reset your password.</p>
                        <p>Your reset code is:</p>
                        <h2>{plainToken}</h2>
                        <p>This code expires in 30 minutes.</p>
                        <p>If you did not request this, you can ignore this email.</p>
                        """
            };

            await _mailService.SendEmailAsync(email, cancellationToken);

            return Unit.Value;
        }

        private static string GenerateSecureToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        private static string ComputeSha256Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input.Trim());
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}