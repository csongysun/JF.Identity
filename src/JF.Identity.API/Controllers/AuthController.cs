using System;
using System.Threading.Tasks;
using JF.Domain.Command;
using JF.Identity.API.Utils;
using JF.Identity.Core.Application.Command;
using Microsoft.AspNetCore.Mvc;

namespace JF.Identity.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        public AuthController(
            ICommandBus client
            )
        {
            _commandBus = client ?? throw new ArgumentNullException(nameof(client));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpCommand cmd)
        {
            var ret = await _commandBus.SendAsync(cmd, HttpContext.RequestAborted);
            return ret.Succeed ? Accepted() : this.Error(ret.ErrorCode);
        }
    }
}
