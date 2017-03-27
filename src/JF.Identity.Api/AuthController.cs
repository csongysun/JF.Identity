using AutoMapper;
using JF.Identity.Manager;
using JF.Identity.Proto;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            if (model == null || !ModelState.IsValid)
                return BadRequest(ApiErrorDescriber.ModelNotValid);
            var result = await _signin.PasswordSignInAsync(model.Email, model.Password);
            if (result.result.Succeeded)
            {
                _logger.LogInformation($"User ({model.Email}) log in");
                return Ok(Mapper.Map<UserRes>(result.user));
            }
            return BadRequest(result.result.Error);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpReq model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ApiErrorDescriber.ModelNotValid);

            var user = Mapper.Map<User>(model);
            var result = await _user.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshAsync([FromQuery]string token)
        {
            if (token == null) return BadRequest(ApiErrorDescriber.ModelNotValid);
            var result = await _signin.RefreshLoginAsync(token);
            if (!result.result.Succeeded) return BadRequest(result.result.Error);
            return Ok(result.user);
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
