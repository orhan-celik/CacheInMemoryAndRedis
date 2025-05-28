using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {

        private readonly IRedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "cars";

        public SetTypeController(IRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb();
        }

        public IActionResult Index()
        {

            HashSet<string> carList = new HashSet<string>();
            if (db.KeyExists(listKey))
                db.SetMembers(listKey).ToList().ForEach(x => carList.Add(x.ToString()));

            ViewBag.CarCount = db.SetLength(listKey);

            return View(carList);
        }

        [HttpPost]
        public IActionResult Add(string brand)
        {
            db.SetAdd(listKey, brand);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(string brand)
        {
            db.SetRemove(listKey, brand);
            return RedirectToAction("Index");
        }
    }
}
