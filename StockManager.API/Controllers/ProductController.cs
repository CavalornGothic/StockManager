using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManager.Database;

namespace StockManager.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ProductController(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        [HttpGet, Route("getall")]
        public IActionResult GetProducts()
        {
            var database = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
            var products = database.Products;
            if(products.Any())
            {
                IList<ProductDTO> productsDTO = new List<ProductDTO>();
                foreach (var product in products)
                {
                    productsDTO.Add(new ProductDTO { Id = product.Id, Name = product.Name});
                }
                return Ok(productsDTO);
            }
            return BadRequest();
        }

        [HttpGet, Route("get/{id}")]
        public IActionResult Get(int id)
        {
            var database = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
            var product = database.Products.Where(x => x.Id == id);
            if(product.Any())
            {
                ProductDTO productDTO = new ProductDTO() { Id = product.First().Id, Name = product.First().Name};
                return Ok(productDTO);
            }
            return BadRequest();
        }

        [HttpPost, Route("add")]
        public IActionResult Add([FromBody] ProductDTO product)
        {
            var productToSave = new Product { Name = product.Name};
            var database = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
            database.Products.Add(productToSave);
            int count = database.SaveChanges();
            if(count > 0)
                return Ok();
            return BadRequest();
        }

        [HttpDelete, Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var database = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
            var product = database.Products.Where(x => x.Id == id);
            if(product.Count() > 0)
            {
                database.Products.Remove(product.First());
                var count = database.SaveChanges();
                if (count > 0)
                    return Ok();
            }
            
            return BadRequest();
        }

        [HttpPost, Route("edit")]
        public IActionResult Edit([FromBody] ProductDTO productDTO)
        {
            if(productDTO != null)
            {
                var database = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
                var product = database.Products.Where(x => x.Id == productDTO.Id);
                if(product.Count() > 0)
                {
                    product.First().Name = productDTO.Name;
                    int count = database.SaveChanges();
                    if (count > 0)
                        return Ok();
                }
            }
            return BadRequest();
        }
    }
}
