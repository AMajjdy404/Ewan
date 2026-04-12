using Ewan.Core.Models.Dtos.ContactUs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.ContactUs.Commands.UpsertContactUs
{
    public record UpsertContactUsCommand(UpdateContactUsRequestDto Request) : IRequest;
}
