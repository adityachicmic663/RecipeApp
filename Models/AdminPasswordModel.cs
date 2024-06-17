﻿using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class AdminPasswordModel
    {
        [EmailAddress(ErrorMessage = "invalid email")]
        public string Email { get; set; }

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain each of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string NewPassword { get; set; }
    }
}