using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class forgetPasswordModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="invalid email addresss")]
        public string email { get; set; }
    }
}
