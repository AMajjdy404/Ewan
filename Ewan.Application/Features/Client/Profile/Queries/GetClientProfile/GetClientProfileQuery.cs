using Ewan.Core.Models.Dtos.Client;
using MediatR;

namespace Ewan.Application.Features.Client.Profile.Queries.GetClientProfile
{
    public record GetClientProfileQuery(int ClientId) : IRequest<ClientProfileDto>;
}
