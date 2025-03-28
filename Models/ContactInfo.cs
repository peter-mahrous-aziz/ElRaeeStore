namespace elraee.Models
{
    public class ContactInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string facebookLink { get; set; }
        public string gpsLocation { get; set; }

        public string gpsEmbedLocation { get; set; }

        public string whatsAppNum { get; set; }

        public string? EventCover { get; set; }
        public List<phoneNums> phoneNums { get; set; }






    }
}
