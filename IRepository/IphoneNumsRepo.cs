using elraee.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace elraee.IRepository
{
    public interface IphoneNumsRepo
    {
        public void Add(phoneNums number);
        public void Update(phoneNums number);
        public void Delete(int id);

        public void removeOldNumbers(int contactId);
        public List<phoneNums> GetAll();
        public void Save();
    }
}
