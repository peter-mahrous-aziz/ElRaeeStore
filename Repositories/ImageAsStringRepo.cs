using Application.Helpers;
using elraee;
using elraee.IRepository;
using elraee.Models;
using Microsoft.AspNetCore.Hosting;

namespace elraee.Repositories
{
    public class ImageAsStringRepo : IImageAsStringRepo
    {
        context context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageAsStringRepo (context context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Add(imageAsString img)
        {
            context.Add(img);
        }

        public void AddListOfImages(List<imageAsString> imgList)
        {
            foreach(imageAsString img in imgList)
            {
                context.Add(img);
            }
        }

        public void Update(imageAsString img)
        {
            context.Update(img);
        }

        public void Delete(int id)
        {
            imageAsString img = context.images.FirstOrDefault(i => i.Id == id);
            context.Remove(img);
            ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{img.Image}");
        }

        public void DeleteWithProductId(int Pid)
        {
            imageAsString img = context.images.FirstOrDefault(i => i.productId == Pid);
            context.Remove(img);
        }

        public void DeleteWithProductId(int Pid ,string imgName)
        {
            imageAsString img = context.images.FirstOrDefault(i => i.productId == Pid && i.Image==imgName);
            context.Remove(img);
        }

        public void DeleteAllImages(List<imageAsString> images)
        {
            foreach(imageAsString img in images)
            {
                Delete(img.Id);  
            }
            context.SaveChanges();
        }
        public List<imageAsString> GetAll()
        {
            return context.images.ToList();
        }

        public imageAsString GetById(int id)
        {
            return context.images.FirstOrDefault(i => i.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

     
    }
}
