using elraee.IRepository;
using elraee.Models;
using Microsoft.EntityFrameworkCore;

namespace elraee.Repositories
{
    public class categoryRepo : ICategoryRepo
    {
        context context;
        public categoryRepo(context _context)
        {
            this.context = _context;
        }

        public void Add(category _category)
        {
            context.Add(_category);
        }

        public void Update(category _category)
        {
            context.Update(_category);
        }

        public void Delete(int id)
        {
            category cat = GetById(id);
            context.Remove(cat);
        }

     
        public List<category> GetAll()
        {
            return context.categories.OrderBy(c=>c.name).ToList();
        }

        public category GetById(int id)
        {
            return context.categories.FirstOrDefault(c => c.id == id);
        }

        public int GetCount()
        {
            return context.categories.Count();
        }


        public category GetCatWithProdsById(int id)
        {
            return context.categories.Include(c=>c.Products).ThenInclude(p=>p.images).FirstOrDefault(c => c.id == id);
        }
        public List<category> getAllWithSearch(string name)
        {
            return context.categories.Include(c=>c.Products).Where(c => c.name.Contains(name)).OrderBy(c => c.name)
                .ToList();
        }


        public List<category> getCategoryByName(string name)
        {
            return context.categories.Include(c=>c.Products).Where(p => p.name.Contains(name)).ToList();
        }
        public void Save()
        {
            context.SaveChanges();
        }

       
    }
}
