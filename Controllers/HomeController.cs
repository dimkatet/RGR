using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace RGR.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogInformation("InitHome");
        }

        [HttpGet]
        public Int64 Get()
        {
            _logger.LogInformation("N: " + Startup.pi.N);
            return Startup.pi.N;
        }

        [HttpPost]
        public int Post(User user)
        {
            if (Startup.uc.Users.ToList().Where(x => x.Login == user.Login).Count() != 0)
                return -1;
            if (user.Login.Length < 4 || user.Login.Length > 16)
                return -2;

            Startup.uc.Users.Add(user);
            Startup.uc.SaveChanges();
            _logger.LogInformation("User:" + user.ID.ToString());
            return 0;
        }
    }
}
