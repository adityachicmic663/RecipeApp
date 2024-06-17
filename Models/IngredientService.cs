using Microsoft.EntityFrameworkCore;
using recipe.Models;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace RecipeApp.Models
{
    public class IngredientService:IIngredientService
    {
        private readonly applicationDataContext _context;

        public IngredientService(applicationDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ingredient>> getIngredients()
        {
            return await _context.ingredients.ToListAsync();
        }
        public async Task<Ingredient> GetIngredient(int id)
        {
            var ingredient = await _context.ingredients.FindAsync(id);

            return ingredient;
        }

        public async Task<Ingredient> CreateIngredient(IngredientRequest request)
        {
            var ingredient = new Ingredient
            {
                IngredientId = request.IngredientId,
                IngredientName = request.IngredientName,
                quantity = request.quantity
            };
            _context.ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

         public async  Task<bool> UpdateIngredient(int id, IngredientRequest request)
        {
            if (id != request.IngredientId)
            {
                return false;
            }
            try
            {
                var ingredient =await  _context.ingredients.FindAsync(id);
                if (ingredient == null)
                {
                    return false;
                }
                ingredient.quantity = request.quantity;
                ingredient.IngredientName = request.IngredientName;
                _context.Entry(ingredient).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            catch(DbUpdateConcurrencyException) {
                if (!_context.ingredients.Any(e=>e.IngredientId==id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
                }
            return true;
        }
           
           
        public async Task<bool> DeleteIngredient(int id)
        {
            var ingredient = await _context.ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return false;
            }
            _context.ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
