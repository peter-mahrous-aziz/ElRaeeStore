using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class updateProductViewModel
    {
        [Display(Name = "اسم المنتج")]
        public string name { get; set; }
        
        [Display(Name = "كود المنتج")]
        public int id { get; set; }
        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Display(Name = "الصوره الرئيسيه")]
        public string oldHomeImage {get; set;}

        [Display(Name = "السعر")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }

        [Display(Name = "العلامة التجاريه")]
        public string brand { get; set; }

        [Display(Name = "بلد المنشأ")]
        public string madeIn { get; set; }

        [Display(Name = "القسم")]
        public int categoryId { get; set; }
        public List<string> ? oldImageList { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]
        [Display(Name = "اذا كنت ترغب في تغيير الصوره الرئيسيه")]
        public IFormFile? newHomeImage { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]

        [Display(Name = "اذا كنت ترغب في تغيير باقي صور المنتج")]
        public List<IFormFile>? newImageList { get; set; }

        public List<int>? relativeProductIDs { get; set; }



    }
}
