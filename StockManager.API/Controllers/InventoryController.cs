using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManager.Database;

namespace StockManager.API.Controllers
{
    [Route("api/inv")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly StockManagerDbContext _dbContext;

        public InventoryController(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
        }

        [HttpGet, Route("getall/{idrow}")]
        public IActionResult GetAll(int idrow)
        {
            var invs = _dbContext.Inventories.Where(x => x.RowID == idrow);
            if (_dbContext.Rows.Where(x => x.Id == idrow).Any() && invs.Any())
            {
                return Ok(invs);
            }
            return BadRequest();
        }
        [HttpPost, Route("add/{idrow}")]
        public IActionResult AddInv(int idrow, [FromBody] InvAddDTO invAddDTO)
        {
            if (_dbContext.Rows.Where(x => x.Id == idrow).Any())
            {
                _dbContext.Inventories.Add(new Inventory { CreateDateTime = DateTime.Parse(invAddDTO.CreateDateTime), Quantity = invAddDTO.Quantity, RowID = idrow});
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost, Route("delete/{idinv}")]
        public IActionResult DeleteInv(int idinv)
        {
            var inv = _dbContext.Inventories.Where(x => x.Id == idinv);
            if(inv.Any())
            {
                _dbContext.Inventories.Remove(inv.First());
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost, Route("edit/{idinv}")]
        public IActionResult EditInv(int idinv, [FromBody] double Quantity)
        {
            var inv = _dbContext.Inventories.Where(x => x.Id == idinv);
            if (inv.Any())
            {
                inv.First().Quantity = Quantity;
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
