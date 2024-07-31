using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1_asp.net_MVC__.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
