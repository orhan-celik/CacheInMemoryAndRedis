using CacheInMemoryAndRedis.InMemoryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CacheInMemoryAndRedis.InMemoryApp.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {

            // Cache 1 : Basit olarak cacheleme
            // _memoryCache.Set<string>("zaman", DateTime.Now.ToString());

            // Cache 2 : Options geçerek özellik belirleyerek cacheleme

            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {

                MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions();
                opt.SlidingExpiration = TimeSpan.FromSeconds(10); // 10 sn. de bir cache süresi uzatılsın.
                opt.AbsoluteExpiration = DateTime.Now.AddSeconds(30); // Ne olursa olsun 30 sn. sonra daha uzatılma olmasın silinsin.
                opt.Priority = CacheItemPriority.High; // Ram dolarsa en son silinecek seviyede olsun.

                // Silinme sebebini yakalayıp cacheleyebiliriz.
                opt.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value} => Sebep : {reason}");
                });

                // options geçerek cache oluşturuyoruz.
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), opt);
            }

            // Cache 3 : Complex Type Sınıf cacheleme
            Product product = new Product { Id = 1, Name = "Kalem", Price = 10 };
            _memoryCache.Set<Product>("product:1", product);


            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string zamancache);
            ViewBag.zaman = zamancache;

            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;

            _memoryCache.TryGetValue("product:1", out Product p);
            ViewBag.product = p;

            return View();
        }
    }
}
