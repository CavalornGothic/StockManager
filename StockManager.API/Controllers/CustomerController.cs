using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManager.Database;

namespace StockManager.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StockManagerDbContext _dbContext;

        public CustomerController(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetCustmers()
        {
            var customers = _dbContext.Customers;
            if(customers.Any())
            {
                IList<CustomerDTO> customerDTOs = new List<CustomerDTO>();
                foreach (var customer in customers)
                {
                    customerDTOs.Add(new CustomerDTO { Id = customer.Id, Name = customer.Name, City = customer.City, Street = customer.Street, ZipCode = customer.ZipCode });
                }
                return Ok(customerDTOs);
            }
            return BadRequest();
        }

        [HttpGet, Route("get/{id}")]
        public IActionResult Get(int id)
        {
            var customer = _dbContext.Customers.Where(x => x.Id == id);
            if(customer.Any())
            {
                var oneCustomer = customer.First();
                CustomerDTO customerDTO = new CustomerDTO { Id = oneCustomer.Id, City = oneCustomer.City, Name = oneCustomer.Name, Street = oneCustomer.Street, ZipCode = oneCustomer.ZipCode };
                return Ok(customerDTO);
            }
            return BadRequest();
        }
        [HttpDelete, Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var customer = _dbContext.Customers.Where(_x => _x.Id == id);
            if(customer.Any())
            {
                _dbContext.Customers.Remove(customer.First());
                int count = _dbContext.SaveChanges();
                if (count > 0)
                    return Ok();
            }
            return BadRequest();
        }
        [HttpPost, Route("edit")]
        public IActionResult Edit([FromBody] CustomerDTO customerDTO)
        {
            var customer = _dbContext.Customers.Where(x => x.Id == customerDTO.Id);
            if(customer.Any())
            {
                customer.First().Name = customerDTO.Name;
                customer.First().Street = customerDTO.Street;
                customer.First().ZipCode = customerDTO.ZipCode;
                customer.First().City = customerDTO.City;
                int count = _dbContext.SaveChanges();
                if (count > 0)
                    return Ok();
            }
            return BadRequest();
        }
        [HttpPut, Route("add")]
        public IActionResult Add([FromBody] CustomerDTO customerDTO)
        {
            Customer customer = new Customer { Name = customerDTO.Name, City = customerDTO.City, Street = customerDTO.Street, ZipCode = customerDTO.ZipCode };
            _dbContext.Customers.Add(customer);
            int count = _dbContext.SaveChanges();
            if (count > 0)
                return Ok();
            return BadRequest();
        }
    }
}
