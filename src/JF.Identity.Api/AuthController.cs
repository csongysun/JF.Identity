using AutoMapper;
using JF.Identity.Api.Model.Req;
using JF.Identity.Manager;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JF.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ApiErrorDescriber.ModelNotValid);
            var result = await _signin.PasswordSignInAsync(model.Email, model.Password);
            if (result.result.Succeeded)
            {
                _logger.LogInformation($"User ({model.Email}) log in");
                return Ok(result.user);
            }
            return BadRequest(result.result.Error);
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpModel model)
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
        [AllowAnonymous]
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
