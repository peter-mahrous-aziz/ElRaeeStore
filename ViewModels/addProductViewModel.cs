using elraee.Models;
using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class addProductViewModel
    {
        public string name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string brand { get; set; } 
        public string madeIn { get; set; } 

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }


        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double? InsteadOfPrice { get; set; }


        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]
        public IFormFile homeImage { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "يجب أن يكون الملف صورة (jpg, jpeg, png, gif)")]
        public List<IFormFile>? images { get; set; }

       public List<int>? relativeProductIDs { get; set; }
    }
}
