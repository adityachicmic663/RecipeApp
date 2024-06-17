using recipe.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class Recipe
    {
        [Key]
        public int recipeId { get; set; }
        [Required]
        public string recipeName { get; set; }

        public List<RecipeIngredient> recipeIngredients { get; set; } 

        public List<string> steps { get; set; } 

        
    }
}
