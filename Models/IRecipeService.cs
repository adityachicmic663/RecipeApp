namespace RecipeApp.Models
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetRecipes();

        Task<Recipe> GetRecipeById(int id);

        Task<Recipe> CreateRecipe(RecipeRequest request);

        Task<bool> UpdateRecipe(int id, RecipeRequest request);

        Task<bool> DeleteRecipe(int id);
    }
}
