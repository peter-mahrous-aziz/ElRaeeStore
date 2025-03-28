using elraee.Models;

namespace elraee.Repositories
{
    public interface IApplicationUserRepo
    {

        public void Add(applicationUser user);
        public void Update(applicationUser user);
        public void Delete(int id);
        public List<applicationUser> GetAll();
        public applicationUser GetById(int id);

 

       
        public void Save();
    }
}
