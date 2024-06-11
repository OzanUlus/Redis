using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
   
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDatabase(0);
        }

        public IActionResult Index()
        {
           
            db.StringSet("name", "Ozan Ulus");
            db.StringSet("ziyaretci", 100);
            return View();
        }

        public IActionResult Show() 
        {
            var datas = db.StringGet("name");
            if (datas.HasValue)
            {
                ViewBag.data = datas.ToString();
            }
          return View();
        }
    }
}
