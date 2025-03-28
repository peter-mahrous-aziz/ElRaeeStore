using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class updateCategoryViewModel
    {
        public string name { get; set; }

        public int id { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]
        public IFormFile? img { get; set; }

        public string oldImg { get; set; }

    }
}
