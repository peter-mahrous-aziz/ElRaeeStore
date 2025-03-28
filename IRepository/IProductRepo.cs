using elraee.Models;

namespace elraee.IRepository
{
    public interface IProductRepo
    {
        public void Add(product _product);
        public void Update(product _product);
        public void Delete(int id);
        public List<product> GetAll();
        public product GetById(int id);
        public int GetCount();
        public List<product> getAllWithSearch(string name);
        public void DeleteAllProducts(List<product> prods);

        public List<product> GetAllProductsByCategoryId(int id);

        public List<product> getProductsByName(string name);
        public void Save();
    }
}
