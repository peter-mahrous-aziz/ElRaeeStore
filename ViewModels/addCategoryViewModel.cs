
using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class addCategoryViewModel
    {
        public string name { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]
        public IFormFile img { get; set; }

    }
}
