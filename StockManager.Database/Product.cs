using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace StockManager.Database
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
