using Ewan.Core.Models.Dtos.ContactUs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.ContactUs.Queries.GetContactUs
{
    public record GetContactUsQuery : IRequest<ContactUsDto>;
}
