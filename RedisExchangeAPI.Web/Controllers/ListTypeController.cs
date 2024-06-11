using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDatabase(1);
        }

        
        public IActionResult Index()
        {
            List<string> list = new List<string>();
            if (db.KeyExists(listKey)) 
            {
               db.ListRange(listKey).ToList().ForEach(n => {
                   list.Add(n.ToString());
               });
                
            }


            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey,name); // sona ekler
            //db.ListLeftPush(listKey,name); // başa ekler

            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name) 
        {
            db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");

            //db.ListLeftPop(listKey); // baştan siler
            //db.ListRightPop(listKey); // sondan siler
        
        }
    }
}
