﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController :Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "sortedsetNames";

        public SortedSetTypeController(RedisService redisService) 
        {
            _redisService = redisService;
            db = _redisService.GetDatabase(3);
        }

        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });

                db.SortedSetRangeByRank(listKey, 0, 5, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.SortedSetAdd(listKey, name, score);
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
