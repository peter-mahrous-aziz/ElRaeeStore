using System.ComponentModel.DataAnnotations.Schema;

namespace elraee.Models
{
    public class phoneNums
    {
        public int id { get; set; }
        public string number { get; set; }

        [ForeignKey("ContactInfo")]
        public int contactId { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }
}
