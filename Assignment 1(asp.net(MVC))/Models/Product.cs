using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_1_asp.net_MVC__.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; } // Changed to ProductId

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; } 
        public Category Category { get; set; } 
    }
}
