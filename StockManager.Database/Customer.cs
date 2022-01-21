using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Database
{
    [Index(nameof(Name), IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
    }
}
