using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class HashTypeController : Controller
    {

        private readonly IRedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "hashTypeListNames";

        public HashTypeController(IRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb();
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            if (db.KeyExists(listKey))
            {
                db.HashGetAll(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name!, x.Value!);
                });
            }

            ViewBag.Total = db.HashLength(listKey);
            return View(list);
        }

        public IActionResult Add(string key, string val)
        {
            db.HashSet(listKey, key, val);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name)
        {
            db.HashDelete(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
