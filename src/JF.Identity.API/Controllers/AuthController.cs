using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JF.Identity.API.Utils;
using JF.Identity.Grain;
using JF.Identity.Grain.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;

namespace JF.Identity.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IGrainFactory _factory;
        private readonly ILogger _logger;
        public AuthController( 
            IGrainFactory factory,
            ILogger<AuthController> logger
            )
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpCommand cmd)
        {
            var sw = new Stopwatch();
            sw.Start();
            var grain = _factory.GetGrain<ISignUpWorker>(0);
            var ret = await grain.HandleAsync(cmd);
            sw.Stop();
            _logger.LogWarning(sw.ElapsedMilliseconds.ToString());
            return ret.Succeed ? Accepted() : this.Error(ret.ErrorCode);
        }
    }
}
