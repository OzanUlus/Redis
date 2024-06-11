using IDistributeCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributeCacheRedisApp.Web.Controllers
{

    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();

            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name", "Ozan",cacheOptions);
            //await _distributedCache.SetStringAsync("surname" , "Ulus"); 
            Product product = new Product() { Id = 1, Name = "kalem", Price = 100 };
            string jsonproduct = JsonConvert.SerializeObject(product);

            //Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);

            //_distributedCache.Set("product:1", byteproduct);


            await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheOptions);

            return View();
        }
        public async Task<IActionResult> Show()
        {
            //string name = _distributedCache.GetString("name");
            //string surname = await _distributedCache.GetStringAsync("surname");

            //   ViewBag.Name = surname;

            string jsonproduct = await _distributedCache.GetStringAsync("product:1");

            //Byte[] byteProduct = _distributedCache.Get("product:1");

            //string jsonproduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);

            ViewBag.product = p;

            return View();
        }
        public IActionResult Delete()
        {
            //_distributedCache.Remove("name");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim" , imageByte);
            return View();
        }
        public IActionResult ImageShow() 
        {
            byte[] resimbyte = _distributedCache.Get("resim");
            return File(resimbyte, "image/jpg");
        }
    }
}
