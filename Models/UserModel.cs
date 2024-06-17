using System.IO.Pipes;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class UserModel
    {
        [Required]
        [Key]
        public int userId { get; set; }

        public string userName { get; set; }

        public string Email { get; set;}

        public string Password { get; set; }

        public string role { get; set; }

        public int phoneNumber { get; set; }

        public bool emailConfirmed { get; set; } 

        public int age { get; set; } = 18;

        public string gender { get; set; } = "Male";

        public string otpToken {  get; set; }

        public DateTime? OtpTokenExpiry { get; set; }

    }
}
