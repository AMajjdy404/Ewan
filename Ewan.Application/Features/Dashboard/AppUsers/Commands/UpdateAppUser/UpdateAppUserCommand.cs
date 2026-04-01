using Ewan.Core.Models.Dtos.AppUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUser
{
    public record UpdateAppUserCommand(UpdateAppUserRequestDto Request) : IRequest<bool>;

}
