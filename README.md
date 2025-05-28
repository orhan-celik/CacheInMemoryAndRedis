# Cache Projeleri Seti (.NET 8)

Bu proje seti, farklı cache kullanım senaryolarını içermektedir ve .NET 8 ile geliştirilmiştir. Projeler, hem In-Memory cache hem de Redis tabanlı cache çözümlerini kapsamaktadır. Ayrıca Docker desteği ve Redis için görsel yönetim aracı olarak RedisInsight kullanımı mevcuttur.

---

## İçerik

### 1. CacheInMemoryAndRedis.IDistributedCacheRedisApp
- `IDistributedCache` arayüzü kullanılarak Redis tabanlı cache uygulaması.
- Redis cache operasyonlarının temel kullanımını gösterir.
- Dağıtık cache senaryolarına uygun yapı.

### 2. CacheInMemoryAndRedis.InMemoryApp
- Uygulama içi (In-Memory) cache kullanımı.
- Basit ve hızlı cache ihtiyacı için tasarlanmıştır.
- Redis gereksinimi olmadan çalışır.

### 3. RedisExchangeApi.Web
- Redis Exchange API üzerinden cache yönetimi.
- Daha gelişmiş Redis özelliklerini ve komutlarını kullanmak için örnek.
- Redis ile direkt etkileşim içeren API yapısı.

---

## Teknolojiler ve Araçlar
- .NET 8
- Redis
- Docker (Redis ve RedisInsight için)
- RedisInsight (Redis keylerini görsel olarak yönetmek için)

---

## Docker Desteği

Projede Redis ve RedisInsight konteyner olarak ayağa kaldırılmıştır.  
Aşağıdaki örnek `docker-compose.yml` dosyası ile Redis ve RedisInsight hızlıca çalıştırılabilir:

```yaml
version: "3.9"
services:
  redis:
    image: redis:latest
    ports:
      - "6379:6379"
    restart: always

  redisinsight:
    image: redislabs/redisinsight:latest
    ports:
      - "8001:8001"
    restart: always
