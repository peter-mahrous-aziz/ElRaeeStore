using System.ComponentModel.DataAnnotations;

namespace elraee.ViewModels
{
    public class roleVM
    {
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
