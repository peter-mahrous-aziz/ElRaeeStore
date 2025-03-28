using Application.Helpers;
using elraee.IRepository;
using elraee.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace elraee.Repositories
{
    public class productRepo : IProductRepo
    {
        context context;
        IImageAsStringRepo imgRepo;
        IRelativeRepo relativeRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public productRepo(context _context, IImageAsStringRepo imgRepo, IWebHostEnvironment webHostEnvironment , IRelativeRepo relativeRepo)
        {
            this.context = _context;
            this.imgRepo = imgRepo;
            this.relativeRepo = relativeRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public void Add(product _product)
        {
            context.Add(_product);
        }


        public void Update(product _product)
        {
            context.Update(_product);
        }

        public void Delete(int id)
        {
            product prod = GetById(id);
            ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{prod.homeImage}");
            if (prod.images.Count > 0)
            {
                imgRepo.DeleteAllImages(prod.images);
            }
            List<relativeProducts> relativeList = relativeRepo.GetAll().Where(r => r.relativeProductId == id).ToList();
            if(relativeList!=null)
            {
                relativeRepo.BulkDelete(relativeList);
                relativeRepo.Save();
            }

            context.Remove(prod);
        }

        public List<product> GetAll()
        {
            return context.products.Include(p => p.images).Include(p=>p.relativeProducts).ToList();
        }

        public product GetById(int id)
        {
            return context.products.Include(p => p.images).Include(p => p.relativeProducts).FirstOrDefault(p => p.Id == id);             ;
        }

        public int GetCount()
        {
            return context.products.Count();
        }

        public List<product> getAllWithSearch(string name)
        {
            return context.products.Where(p => p.Name.Contains(name))
                .ToList();
        }


        public List<product> GetAllProductsByCategoryId(int id)
        {
            return context.products.Where(p=>p.CategoryId== id).ToList();
        }


        public void Save()
        {
            context.SaveChanges();
        }


        public List<product> getProductsByName(string name)
        {
            return context.products.Where(p => p.Name.Contains(name)).ToList();
        }

        public void DeleteAllProducts(List<product> prods) //used when admin wanna delete a category with its prods
        {
           
            for (int i = prods.Count-1; i >= 0; i--)
            {
                Delete(prods[i].Id);
            }
        }

    }


    }

