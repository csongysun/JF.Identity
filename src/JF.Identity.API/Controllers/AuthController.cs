using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JF.Common;
using JF.Domain.Command;
using JF.Identity.Core.Application.Command;
using Microsoft.AspNetCore.Mvc;
using JF.Identity.API.Utils;

namespace JF.Identity.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        public AuthController(
            ICommandBus commandBus
            )
        {
            _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpCommand cmd)
        {
            var ret = await _commandBus.SendAsync<SignUpCommand, XError>(cmd, HttpContext.RequestAborted);
            return ret.IsOk ? Accepted() : this.Error(ret.ErrorCode);
        }
    }
}
