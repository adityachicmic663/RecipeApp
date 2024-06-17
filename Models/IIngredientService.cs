using recipe.Models;

namespace RecipeApp.Models
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> getIngredients();
        Task<Ingredient> GetIngredient(int id);

        Task<Ingredient> CreateIngredient(IngredientRequest request);

        Task<bool> UpdateIngredient(int id, IngredientRequest request);

        Task<bool> DeleteIngredient(int id);

    }
}
