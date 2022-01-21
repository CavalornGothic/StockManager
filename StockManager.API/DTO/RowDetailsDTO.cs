using StockManager.Database;

namespace StockManager.API.DTO
{
    public class RowDetailsDTO
    {
        public int ID { get; set; }
        public int Number { private get; set; }
        public string Type { private get; set; }
        public string FullType { get { return $"{Type}-{Number}"; } }

        public int Priority { get; set; }
        public int Status { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string? Description { get; set; }
        public double TotalVolume { get { return Length * Width * Height; } }
        public double? UsageVolume { get; set; }
        public IList<Customer>? Customers { get; set; }
        public IList<Product>? Products { get; set; }
    }
}
