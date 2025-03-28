using System.ComponentModel.DataAnnotations.Schema;


namespace elraee.Models
{
    public class imageAsString
    {

        public int Id { get; set; }
        public string Image { get; set; }

        [ForeignKey("product")]
        public int productId { get; set; }
        public product? product { get; set; }
    }
}
