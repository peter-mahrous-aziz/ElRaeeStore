using elraee.IRepository;

using elraee.Models;

namespace elraee.Repositories
{
    public class phoneNumsRepo : IphoneNumsRepo
    {
        context context;
        public phoneNumsRepo(context context)
        {
            this.context = context;
        }

        public void Add(phoneNums number)
        {
            context.Add(number);
        }
        public void Update(phoneNums number)
        {
            context.Update(number);
        }
        public void Delete(int id)
        {
            phoneNums temp = context.phoneNums.FirstOrDefault(p => p.id == id);
            context.Remove(temp);
        }

        public void removeOldNumbers(int contactId)
        {
            List<phoneNums> nums = context.phoneNums.Where(p => p.contactId == contactId).ToList();

            if (nums != null)
            {
                foreach (phoneNums num in nums)
                {
                    Delete(num.id);
                }
            }
        }
        public List<phoneNums> GetAll()
        {
            return context.phoneNums.ToList();
        }
        public void Save()
        {
            context.SaveChanges();
        }

    }
}
