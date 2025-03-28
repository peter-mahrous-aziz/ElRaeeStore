using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class EventCoverViewModel
    {
        [Display(Name ="صورة المناسبة")]

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]
        public IFormFile? image { get; set; }
    }
}
