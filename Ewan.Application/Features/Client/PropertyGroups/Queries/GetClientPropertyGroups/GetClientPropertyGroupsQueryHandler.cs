using Ewan.Core.Models.Enums;
using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Client.PropertyGroups.Queries.GetClientPropertyGroups
{
    public class GetClientPropertyGroupsQueryHandler : IRequestHandler<GetClientPropertyGroupsQuery, IReadOnlyList<PropertyGroupDto>>
    {
        public GetClientPropertyGroupsQueryHandler()
        {
        }

        public async Task<IReadOnlyList<PropertyGroupDto>> Handle(GetClientPropertyGroupsQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return Enum.GetValues<PropertyType>()
                .Select(x => new PropertyGroupDto
                {
                    Id = (int)x,
                    Name = x switch
                    {
                        PropertyType.Chalet => "ÔÇáíÉ",
                        PropertyType.Hotel => "ÝäÏÞ",
                        PropertyType.Apartment => "ÔÞÉ",
                        PropertyType.Hall => "ÞÇÚÉ",
                        _ => x.ToString()
                    }
                })
                .OrderBy(x => x.Id)
                .ToList();
        }
    }
}
