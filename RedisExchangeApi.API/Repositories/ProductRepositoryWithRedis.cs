using RedisExchangeApi.API.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExchangeApi.API.Repositories
{
    public class ProductRepositoryWithRedis : IProductRepository
    {

        private readonly IProductRepository _productRepository;
        private readonly IDatabase _redis;
        private const string redisKey = "products";

        /// <summary>
        /// Decorator Design Pattern ile sonradan uygulamaya özellik kazandırdık.
        /// Böylelikle SOLID prensiplerini çiğnemedik.
        /// ÖNEMLİ : IProductRepository bu sınıf (ProductRepositoryWithRedis) dışında implement eden sınıflar için örneklendiğinde ProductRepositoryWithRedis, 
        /// ProductRepositoryWithRedis içerisinde örneklendiğinde ise ProductRepository sınıfı verilecek. Bu düzenlemeyi Program.cs içerisinde yaptık.
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="productRepository"></param>
        public ProductRepositoryWithRedis(IDatabase redis, IProductRepository productRepository)
        {
            _redis = redis;
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            Product newProduct = await _productRepository.CreateAsync(product);

            if (await _redis.HashExistsAsync(redisKey, newProduct.Id))
                await _redis.HashSetAsync(redisKey, newProduct.Id, JsonSerializer.Serialize<Product>(newProduct));

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {

            if (!await _redis.KeyExistsAsync(redisKey))
                return await LoadToCacheFromDbAsync();

            List<Product> products = new List<Product>();

            var productsCacheData = await _redis.HashGetAllAsync(redisKey);

            foreach (var item in productsCacheData.ToList())
            {
                Product product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {

            if (await _redis.HashExistsAsync(redisKey, id))
            {
                var redisData = await _redis.HashGetAsync(redisKey, id);
                return redisData.HasValue ? JsonSerializer.Deserialize<Product>(redisData) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            List<Product> products = await _productRepository.GetAsync();

            products.ForEach(x =>
            {
                _redis.HashSetAsync(redisKey, x.Id, JsonSerializer.Serialize(x));
            });

            return products;
        }
    }
}
