using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace RGR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        static public Int64 V { get; private set; } = 0;
        static public Int64 N { get; private set; } = 0;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
            _logger.LogInformation("InitAuth");
        }

        [HttpPost]
        public Int64 Post(User user)
        {
            var users = Startup.uc.Users.ToList().Where(x => x.Login == user.Login);
            if(users.Count() > 0)
            {
                User u = Startup.uc.Users.ToList().Where(x => x.Login == user.Login).First();
                V = u.ID;
                N = u.N;
                _logger.LogInformation("N: " + N.ToString());
                _logger.LogInformation("V: " + V.ToString());
                return u.N;
            }
            return -1;

        }
    }
}
