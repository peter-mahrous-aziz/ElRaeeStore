using System.ComponentModel.DataAnnotations.Schema;

namespace elraee.Models
{
    public class relativeProducts
    {
        public int id { get; set; }
        public int relativeProductId { get; set; }

        [ForeignKey("product")]
        public int parentProductId { get; set; }
        public product product { get; set; }





    }
}
