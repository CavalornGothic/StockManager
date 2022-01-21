using System.ComponentModel.DataAnnotations.Schema;

namespace StockManager.Database
{
    public class Row
    {
        public int Id { get; set; }
        public int RowTypesDictFK { get; set; }
        [ForeignKey("RowTypesDictFK")]
        public virtual RowTypesDict Type{ get; set; }
        public int Number { get; set; }
        public int Status { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string? Description { get; set; }

    }
}
