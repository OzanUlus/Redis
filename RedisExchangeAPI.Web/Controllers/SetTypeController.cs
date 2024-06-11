using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "hashName";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDatabase(2);
        }

       
        public IActionResult Index()
        {
            HashSet<string> names = new HashSet<string>();
            if(db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(n => { 
                   names.Add(n.ToString());
                });
            }
            return View(names);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            //if (!db.KeyExists(listKey)) her seferinde süre sıfırlanmasını istemezsek kullanbiliriz.
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            db.SetAdd(listKey,name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string name) 
        {
           await db.SetRemoveAsync(listKey,name);

            return RedirectToAction("Index");
        }
    }
}
