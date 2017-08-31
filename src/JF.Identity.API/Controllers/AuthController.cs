using JF.Identity.API.Utils;
using JF.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace JF.Identity.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IClusterClient _client;
        public AuthController(
            IClusterClient client
            )
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpCommand cmd)
        {
            var hlr = _client.GetGrain<ISignUpCommandHandler>(Guid.NewGuid());
            var ret = await hlr.SignUpAsync(cmd);
            return ret.Succeed ? Accepted() : this.Error(ret.ErrorCode);
        }
    }
}
