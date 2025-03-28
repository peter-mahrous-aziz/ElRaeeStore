using elraee.Models;

namespace elraee.ViewModels
{
    public class EditContactViewModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string facebookLink { get; set; }
        public string gpsLocation { get; set; }
        public string gpsEmbedLocation { get; set; }
        public string whatsAppNum { get; set; }
        public List<String> phoneNums { get; set; }
    }
}
