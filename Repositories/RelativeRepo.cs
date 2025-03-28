using elraee.IRepository;

using elraee.Models;

namespace elraee.Repositories
{
    public class RelativeRepo : IRelativeRepo
    {
        context context;

        public RelativeRepo(context context)
        {
            this.context = context;
        }


        public void Add(relativeProducts _relative)
        {
            context.Add(_relative);
        }
        public void BulkAdd(List<relativeProducts> relativeList)
        {
            foreach (relativeProducts rel in relativeList)
            {
                context.Add(rel);
            }

        }
        public void Update(relativeProducts _relative)
        {
            context.Update(_relative);
        }
        public void Delete(int id)
        {
            relativeProducts temp = context.relativeProducts.FirstOrDefault(r => r.id == id);
            context.Remove(temp);
        }

        public void BulkDelete(List<relativeProducts> relativeList)
        {
            foreach (relativeProducts rel in relativeList)
            {
                context.Remove(rel);
            }

        }
        public List<relativeProducts> GetAll()
        {
            return context.relativeProducts.ToList();
        }
        public relativeProducts GetById(int id)
        {
            return context.relativeProducts.FirstOrDefault(r => r.id == id);
        }
        public List<relativeProducts> GetRelativesByParentId(int parentId)
        {
            return context.relativeProducts.Where(r => r.parentProductId == parentId).ToList();
        }

        public void Save() 
        {
            context.SaveChanges();
        }
    }
}
