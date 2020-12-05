using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace RGR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FiatShamirController : Controller
    {
        private readonly ILogger<FiatShamirController> _logger;

        static private List<Int64> x = new List<Int64>();
        static private List<int> e = new List<int>();
        static private Random rnd = new Random();

        public FiatShamirController(ILogger<FiatShamirController> logger)
        {
            _logger = logger;
            _logger.LogInformation("InitFS");
        }

        [HttpPost]
        public IEnumerable<int> Post([FromBody] IEnumerable<AuthData> data)
        {
            //_logger.LogInformation("Stage: " + data.stage.ToString());
            //_logger.LogInformation("N: " + data.n.ToString());
            switch (data.Last().stage)
            {
                case 0:
                    break;
                case 1:
                    _logger.LogInformation("Stage 1");
                    if (data.Count() < 30)
                    {
                        x = new List<Int64>();
                        e = new List<int>();
                        int[] temp = { -1 };
                        _logger.LogInformation("Unsucces \n");
                        return new List<int>(temp);
                    }
                    foreach (var i in data)
                    {
                        x.Add(i.n);
                        e.Add(rnd.Next(0, 2));
                    }
                    //_logger.LogInformation(e.Last().ToString());
                    _logger.LogInformation("E: " + e.ToString());
                    return e;
                case 2:
                    _logger.LogInformation("Stage 2");
                    var y = data.ToList();
                    if(data.Count() < 30)
                    {
                        x = new List<Int64>();
                        e = new List<int>();
                        int[] temp = { -1 };
                        _logger.LogInformation("Unsucces \n");
                        return new List<int>(temp);
                    }
                    for (int i = 0; i < x.Count(); i++)
                    {
                        //if (y[i].n * y[i].n % AuthController.N == S x[i] * Math.Pow(AuthController.V, e[i]) % AuthController.N)
                        if(Startup.pi.mult_m(y[i].n, y[i].n, AuthController.N) == 
                            Startup.pi.mult_m(x[i], (Int64)Math.Pow(AuthController.V, e[i]),  AuthController.N))
                        {
                            _logger.LogInformation("Round: " + i.ToString() + "\n Status: succes");
                            continue;
                        }
                        else
                        {
                            x = new List<Int64>();
                            e = new List<int>();
                            int[] temp = { -1 };
                            _logger.LogInformation("Round: " + i.ToString() + "\nUnsucces \n");
                            return new List<int>(temp);
                        }
                    }
                    x = new List<Int64>();
                    e = new List<int>();
                    int[] t = { 1 };
                    _logger.LogInformation("Succes");
                    return new List<int>(t);
            }
            return null;
        }
    }

    public class AuthData
    {
        public Int64 n { get; set; }

        public int stage { get; set; }
    }
}
