using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {

        private readonly IRedisService _redisService;
        private readonly IDatabase _db;

        public StringTypeController(IRedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb();
        }

        public IActionResult Index()
        {
            _db.StringSet("fullname", "Orhan ÇELİK");
            _db.StringSet("ziyaretci_sayisi", 100);
            _db.StringSet("skill", "web");
            return View();
        }

        public IActionResult Increment()
        {
            _db.StringIncrement("ziyaretci_sayisi");
            return View();
        }

        public IActionResult Decrement()
        {
            _db.StringDecrement("ziyaretci_sayisi");
            return View();
        }

        public IActionResult Show()
        {
            ViewBag.ziyaretci_sayisi = _db.StringGet("ziyaretci_sayisi");
            return View();
        }
    }
}
