using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManager.Database;

namespace StockManager.API.Controllers
{
    [Route("api/row")]
    [ApiController]
    public class RowController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StockManagerDbContext _dbContext;

        public RowController(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetService<StockManagerDbContext>() as StockManagerDbContext;
        }

        [HttpGet, Route("getall")]
        public IActionResult GetAll()
        {
            if(_dbContext.Rows.Any())
            {
                return Ok(_dbContext.Rows);
            }
            return BadRequest();
        }

        [HttpPost, Route("add")]
        public IActionResult Add([FromBody] RowAddDTO rowAddDTO)
        {
            if(_dbContext.RowTypesDict.Where(x => x.Name == rowAddDTO.Type).Count() < 1)
                return BadRequest();
            
            var typeId = _dbContext.RowTypesDict.Where(x => x.Name == rowAddDTO.Type).First().Id;
            var rowByType = _dbContext.Rows.Where(x => x.RowTypesDictFK == typeId);
            int newNumber = rowByType.Any() ? rowByType.Max(x => x.Number) + 1 : 1;
            
            Row row = new Row { Height = rowAddDTO.Height, Length = rowAddDTO.Length, Width = rowAddDTO.Width, Description = rowAddDTO.Description, Number = newNumber , RowTypesDictFK = typeId, Status = 0};
            _dbContext.Rows.Add(row);
            int count = _dbContext.SaveChanges();
            if (count > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPost, Route("close/{id}")]
        public IActionResult Close(int id)
        {
            if(_dbContext.Rows.Any())
            {
                var row = _dbContext.Rows.Where(x => x.Id == id).First();
                row.Status = row.Status == 0 ? 1 : 0;
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        // ustawia priorytet dla rzędu i produktu
        [HttpPost, Route("priority/set/{id}/{idproduct}")]
        public IActionResult SetPriority(int id, int idproduct)
        {
            var row = _dbContext.Rows.Where(x => x.Id == id);
            if(row.Any())
            {
                var attachedRows = _dbContext.RowProducts.Where(x => x.RowID == id && x.ProductID == idproduct);
                if(attachedRows.Any())
                {
                    var attachedsRowsProduct = _dbContext.RowProducts.Where(x => x.ProductID == idproduct);
                    if( attachedsRowsProduct.Any())
                    {
                        foreach(var x in attachedsRowsProduct)
                        {
                            x.Priority = 0;
                        }
                        attachedRows.First().Priority = 1;
                        _dbContext.SaveChanges();
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }
        // ściąga priorytet z podanego rzędu i produktu 
        [HttpPost, Route("priority/unset/{id}/{idproduct}")]
        public IActionResult UnSetPriority(int id, int idproduct)
        {
            var attachedRows = _dbContext.RowProducts.Where(x => x.RowID == id && x.ProductID == idproduct);
            if (attachedRows.Any())
            {
                attachedRows.First().Priority = 0;
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        // dodaje produkt do wybranego rzędu
        [HttpPost, Route("attach/product/{id}/{idproduct}")]
        public IActionResult AttachProduct(int id, int idproduct)
        {
            if (_dbContext.Rows.Any())
            {
                var row = _dbContext.Rows.Where(x => x.Id == id);
                var product = _dbContext.Products.Where(x => x.Id == idproduct);
                var isAttached = _dbContext.RowProducts.Where(x => x.RowID == id && x.ProductID == idproduct).Count();
                if(row.Count() > 0 && product.Count() > 0 && isAttached < 1)
                {
                    _dbContext.RowProducts.Add( new RowProduct { RowID = id, ProductID = idproduct, Priority = 0} );
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }
        // dodaje kontrahenta dla wybranego rzędu
        [HttpPost, Route("attach/customer/{id}/{idcustomer}")]
        public IActionResult AttachCustomer(int id, int idcustomer)
        {
            if(_dbContext.Rows.Any())
            {
                var row = _dbContext.Rows.Where(x => x.Id == id);
                var customer = _dbContext.Customers.Where(x => x.Id == idcustomer);
                var isAttached = _dbContext.RowCustomers.Where (x => x.Id == idcustomer && x.CustomerID == idcustomer).Count();
                if(isAttached < 1 && customer.Count() > 0 && row.Count() > 0)
                {
                    _dbContext.RowCustomers.Add(new RowCustomer { RowID = id, CustomerID = idcustomer});
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }
        // odłącza wybrany towar dla wybranego rzędu
        [HttpPost, Route("/deattach/product/{id}/{idproduct}")]
        public IActionResult DeAttachProduct(int id, int idproduct)
        {
            if (_dbContext.Rows.Any())
            {
                var attachedProduct = _dbContext.RowProducts.Where(x => x.RowID == id && x.ProductID == idproduct);
                if (attachedProduct.Count() > 0)
                {
                    _dbContext.RowProducts.Remove(attachedProduct.First());
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }
        // odłącza wybranego kontrahenta dla wybranego rzędu
        [HttpPost, Route("/deattach/customer/{id}/{idcustomer}")]
        public IActionResult DeAttachCustomer(int id, int idcustomer)
        {
            if (_dbContext.Rows.Any())
            {
                var attachedCustomer = _dbContext.RowCustomers.Where(x => x.Id == idcustomer && x.CustomerID == idcustomer);
                if(attachedCustomer.Count() > 0)
                {
                    _dbContext.RowCustomers.Remove(attachedCustomer.First());
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }

        // edytuje wybrany rząd
        [HttpPost, Route("edit/{id}")]
        public IActionResult EditRow(int id, [FromBody] RowEditDTO rowEditDTO)
        {
            var row = _dbContext.Rows.Where(x => x.Id == id);
            if(row.Any())
            {
                var rowToEdit = row.First();
                rowToEdit.Description = rowEditDTO.Description ?? rowToEdit.Description;
                rowToEdit.Length = rowEditDTO.Length ?? rowToEdit.Length;
                rowToEdit.Width = rowEditDTO.Width ?? rowToEdit.Width;
                rowToEdit.Height = rowEditDTO.Height ?? rowToEdit.Height;
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
