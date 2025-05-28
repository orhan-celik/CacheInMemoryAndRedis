using CacheInMemoryAndRedis.IDistributedCacheRedisApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CacheInMemoryAndRedis.IDistributedCacheRedisApp.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions opt = new DistributedCacheEntryOptions();
            opt.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
            _distributedCache.SetString("company_name", "Memorial", opt);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 10 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            _distributedCache.SetString("product:1", jsonProduct, opt);

            Product product2 = new Product { Id = 1, Name = "Defter", Price = 20 };
            string jsonProduct2 = JsonConvert.SerializeObject(product2);
            _distributedCache.SetString("product:2", jsonProduct2, opt);

            return View();
        }

        public IActionResult Show()
        {
            ViewBag.CompanyName = _distributedCache.GetString("company_name");
            ViewBag.Product1 = JsonConvert.DeserializeObject<Product>(_distributedCache.GetString("product:1"));
            ViewBag.Product2 = JsonConvert.DeserializeObject<Product>(_distributedCache.GetString("product:2"));
            return View();
        }

        public IActionResult Delete()
        {
            _distributedCache.Remove("company_name");
            return RedirectToAction("Show");
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/memorial.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("resim", imageByte);
            return View();
        }

        public IActionResult ImageUrl()
        {

            byte[] imageByte = _distributedCache.Get("resim");
            return File(imageByte, "image/png");
        }
    }
}
