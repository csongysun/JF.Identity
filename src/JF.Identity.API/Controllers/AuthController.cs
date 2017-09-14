using System;
using System.Threading.Tasks;
using JF.Identity.API.Utils;
using JF.Identity.Grain;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace JF.Identity.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IGrainFactory _factory;
        public AuthController( 
            IGrainFactory factory
            )
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpCommand cmd)
        {
            var grain = _factory.GetGrain<IAuthWorker>(0);
            var ret = await grain.SignUpAsync(cmd);
            return ret.Succeed ? Accepted() : this.Error(ret.ErrorCode);
        }
    }
}
