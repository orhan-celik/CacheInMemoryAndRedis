using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {

        private readonly IRedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "sortedSetNames";

        public SortedSetTypeController(IRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb();
        }

        public IActionResult Index()
        {
            HashSet<(string, int)> list = new HashSet<(string, int)>();

            if (db.KeyExists(listKey))
            {
                db.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add((x.Element.ToString(), Convert.ToInt32(x.Score)));
                });
            }

            ViewBag.Total = db.SortedSetLength(listKey);
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(10));
            db.SortedSetAdd(listKey, name, score);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(string name)
        {
            db.SortedSetRemove(listKey, name);
            return RedirectToAction("Index");
        }


    }
}
