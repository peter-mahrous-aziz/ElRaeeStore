using elraee.Models;

namespace elraee.IRepository
{
    public interface IContactInfoRepo
    {
        public void Add(ContactInfo contact);
        public void Update(ContactInfo contact);
        public void Delete(int id);
      
        public List<ContactInfo> GetAll();
        public ContactInfo GetContact();
        public void Save();
    }
}
