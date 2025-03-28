using elraee.Models;

namespace elraee.IRepository
{
    public interface IImageAsStringRepo
    {
        public void Add(imageAsString img);

        public void AddListOfImages(List<imageAsString> imgList);
        public void Update(imageAsString img);
        public void Delete(int id);
        public void DeleteWithProductId(int Pid);
        public void DeleteWithProductId(int Pid, string imgName);
        public List<imageAsString> GetAll();

        public imageAsString GetById(int id);
        public void DeleteAllImages(List<imageAsString> images);

        public void Save();
    }
}
