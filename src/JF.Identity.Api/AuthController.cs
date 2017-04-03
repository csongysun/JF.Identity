using AutoMapper;
using CSYS.Proto.Common;
using JF.Identity.Manager;
using JF.Identity.Proto;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace JF.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager _user;
        private readonly SignInManager _signin;
        private ILogger _logger;

        public AuthController(
            UserManager user,
            SignInManager signin,
            ILoggerFactory loggerFactory)
        {
            _user = user;
            _signin = signin;
            _logger = loggerFactory.CreateLogger<AuthController>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginReq model)
        {
            var (err, user) = await _signin.PasswordSignInAsync(model.Email, model.Password);
            if (err == null)
            {
                _logger.LogInformation($"User ({model.Email}) log in");
                return Ok(Mapper.Map<UserRes>(user));
            }
            return BadRequest(Mapper.Map<Error>(err));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpReq model)
        {
            var user = Mapper.Map<User>(model);
            var err = await _user.CreateAsync(user, model.Password);
            if (err == null)
            {
                return NoContent();
            }
            return BadRequest(Mapper.Map<Error>(err));
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshAsync([FromQuery]string token)
        {
            var (err, user) = await _signin.RefreshLoginAsync(token);
            if (err == null) return BadRequest(Mapper.Map<Error>(err));
            return Ok(Mapper.Map<UserRes>(user));
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            //Todo: signout
            await _user.SignOutAsync();
            return NoContent();
        }
    }
}
