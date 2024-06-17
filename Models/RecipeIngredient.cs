using Org.BouncyCastle.Crypto.Paddings;
using RecipeApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace recipe.Models
{
    public class RecipeIngredient
    {
        
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public int quantity { get; set; }
        public Ingredient ingredient { get; set; }
        
        public Recipe recipe { get; set; }
    }
}