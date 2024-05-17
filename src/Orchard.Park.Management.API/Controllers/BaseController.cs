using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orchard.Park.Model.Configuration;

namespace Orchard.Park.Management.API.Controllers;

[Authorize(Roles = JwtTokenRoles.OrchardParkManagementAPI)]
[ApiController]
[Route("api/orchard_park/[controller]/[action]")]
public class BaseController : ControllerBase
{
}