namespace RecipeApp.Models
{
    public class RecipeRequest
    {
        
        public int recipeId { get; set; }
        public string recipeName { get; set; }

        public List<IngredientRequest> ingredientRequests { get; set; }

        public List<string> steps { get; set; }
    }
}
