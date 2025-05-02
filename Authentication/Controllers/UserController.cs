using Amazon.Lambda.APIGatewayEvents;
using Authentication.Core.Interfaces;
using Authentication.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserService _IUserService;
        private readonly IAuthService _IAuthService;

        public UserController(ILogger<WeatherForecastController> logger, IUserService iuserService, IAuthService iAuthService)
        {
            _logger = logger;
            _IUserService = iuserService;
            _IAuthService = iAuthService;
            _logger.LogInformation("User Controller constructor hit!");
        }

        //[Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string name, string password)
        {
            var user = await _IUserService.GetByNameAndPasswordAsync(name, password);
            var token = new Token();
            if (user != null)
            {
                token = _IAuthService.Authenticate(user);
                return Ok(new
                {
                    token
                });
            }
            else
            {
                return Unauthorized(new
                {
                    error = "Unauthorized",
                    message = "Authentication failed. Invalid credentials."
                });
            }

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
