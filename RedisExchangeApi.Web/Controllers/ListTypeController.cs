using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {

        private readonly IRedisService _redisService;
        private readonly IDatabase _db;
        private readonly string _listKey;

        public ListTypeController(IRedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb();
            _listKey = "colors";
        }

        public IActionResult Index()
        {

            List<string> colorList = new List<string>();
            if (_db.KeyExists(_listKey))
            {
                _db.ListRange(_listKey).ToList().ForEach(x =>
                {
                    colorList.Add(x);
                });
            }

            ViewBag.Colors = colorList;
            ViewBag.ColorsCount = _db.ListLength(_listKey);

            return View();
        }

        [HttpPost]
        public IActionResult Add(string color)
        {
            _db.ListRightPush(_listKey, color);
            return RedirectToAction("Index");
        }

        [HttpGet("ListType/Remove/{color}")]
        public IActionResult Remove(string color)
        {
            _db.ListRemove(_listKey, color);
            return RedirectToAction("Index");
        }
    }
}
