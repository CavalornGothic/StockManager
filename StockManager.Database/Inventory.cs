using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Database
{
    public class Inventory
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int RowID { get; set; }
        [ForeignKey(nameof(RowID))]
        public virtual Row Row { get; set; }
    }
}
