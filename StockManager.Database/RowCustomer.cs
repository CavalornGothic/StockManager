using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Database
{
    // wiele do wielu, Row - Customer
    public class RowCustomer
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey(nameof(CustomerID))]
        public virtual Customer Customer { get; set; }
        public int RowID { get; set; }
        [ForeignKey(nameof(RowID))]
        public virtual Row Row { get; set; }
    }
}
