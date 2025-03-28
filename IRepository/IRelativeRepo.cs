using elraee.Models;

namespace elraee.IRepository
{
    public interface IRelativeRepo
    {
        public void Add(relativeProducts _relative);
        public void BulkAdd(List<relativeProducts> relativeList);
        public void Update(relativeProducts _relative);
        public void Delete(int id);
        public List<relativeProducts> GetAll();
        public relativeProducts GetById(int id);
        public List<relativeProducts> GetRelativesByParentId(int id);
        public void BulkDelete(List<relativeProducts> relativeList);
        public void Save();
    }
}
