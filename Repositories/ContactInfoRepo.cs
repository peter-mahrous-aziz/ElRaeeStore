using elraee.Models;
using elraee.IRepository;
using Microsoft.EntityFrameworkCore;

namespace elraee.Repositories
{
    public class ContactInfoRepo : IContactInfoRepo
    {
        context context;

        public ContactInfoRepo (context context)
        {
            this.context = context;
        }
        public void Add(ContactInfo contact)
        {
            context.Add(contact);
        }
        public void Update(ContactInfo contact)
        {
            context.Update(contact);
        }
        public void Delete(int id)
        {
            ContactInfo temp = context.contactInfo.FirstOrDefault(c => c.id == id);
            context.Remove(temp);
        }

        public List<ContactInfo> GetAll()
        {
            return context.contactInfo.ToList();
        }
        public ContactInfo GetContact()
        {
            return context.contactInfo.Include(c=>c.phoneNums).FirstOrDefault();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
