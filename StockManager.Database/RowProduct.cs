using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Database
{
    public class RowProduct
    {
        public int Id { get; set; }
        public int RowID { get; set; }
        [ForeignKey(nameof(RowID))]
        public virtual Row Row { get; set; }
        public int ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public virtual Product Product { get; set; }
        public int Priority { get; set; } = 0;
    }
}
