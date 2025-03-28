using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace elraee.Models
{
    public class product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string homeImage { get; set; } //اللى تظهر من برا 

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double? InsteadOfPrice { get; set; }
        public string brand { get; set; } 

        public string madeIn { get; set; } 
        

        // Relationships

        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public category? category { get; set; }

        public List<imageAsString>? images { get; set; }

        public List<relativeProducts>? relativeProducts {get; set;}
    }
}
