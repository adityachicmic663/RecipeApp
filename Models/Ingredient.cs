using System.ComponentModel.DataAnnotations;

namespace recipe.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        [Required]
        public string IngredientName { get; set; }

        [Required]
        public int quantity { get; set; }

        public List<RecipeIngredient> recipeIngredients { get; set; }
    }
}