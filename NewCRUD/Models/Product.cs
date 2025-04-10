using Microsoft.EntityFrameworkCore;

namespace NewCRUD.Models
{
    public class Product
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageFileName { get; set; }
        //public DateTime CreatedAt { get; set; }
    }
}
