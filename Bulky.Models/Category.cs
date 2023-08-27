using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int CategoryId{ get; set; }

        [DisplayName("Category Name")]
        [MaxLength(14)]
        public string? Name { get; set; }
        [Range(1, 5, ErrorMessage = "Error")]
        public int DisplayOrder { get; set; }

        //public ICollection<Product> Product { get; set; }

    }
}
