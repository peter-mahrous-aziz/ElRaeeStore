using System.ComponentModel.DataAnnotations.Schema;

namespace elraee.Models
{
    public class category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string img { get; set; }

        //relations
        public List<product>? Products { get; set; }

    }
}
