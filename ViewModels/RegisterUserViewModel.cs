using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class RegisterUserViewModel
    {

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }
    }
}
