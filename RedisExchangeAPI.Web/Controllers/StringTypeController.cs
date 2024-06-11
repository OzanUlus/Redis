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

            var datas = db.StringGetRange("name", 0, 6);

            var datas = db.StringLength("name");





            
            
                ViewBag.data = datas.ToString();

            var count = db.StringIncrement("ziyaretci", 1); //++

            ViewBag.Count = count;

            var decount = db.StringDecrementAsync("ziyaretci", 1).Result; //--
            ViewBag.decount = decount;
            return View();
        }
    }
}
