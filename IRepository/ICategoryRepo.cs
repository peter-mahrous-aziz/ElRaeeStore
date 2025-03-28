using elraee.Models;

namespace elraee.IRepository
{
    public interface ICategoryRepo
    {
        public void Add(category _category);
        public void Update(category _category);
        public void Delete(int id);
        public List<category> GetAll();
        public int GetCount();
        public List<category> getAllWithSearch(string name);

        public List<category> getCategoryByName(string name);
        public category GetById(int id);
        public category GetCatWithProdsById(int id);

        public void Save();
    }
}
