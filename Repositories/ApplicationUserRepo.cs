using elraee.Models;
using Microsoft.EntityFrameworkCore;

namespace elraee.Repositories
{
    public class ApplicationUserRepo : IApplicationUserRepo
    {

        context context;
        public ApplicationUserRepo(context _context)
        {
            this.context = _context;
        }
        public void Add(applicationUser user)
        {
            context.Add(user);
        }
        public void Update(applicationUser user)
        {
            context.applicationUsers.Update(user);
        }
        public void Delete(int id)
        {
            applicationUser user = GetById(id);
            context.Remove(user);
        }
        public List<applicationUser> GetAll()
        {
            return context.applicationUsers.ToList();
        }
        public applicationUser GetById(int id)
        {
            return context.applicationUsers.FirstOrDefault(a => a.Id == id);
        }


       
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
