using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
using recipe.Models;

namespace RecipeApp.Models
{
    public class RecipeService:IRecipeService
    {
        private readonly applicationDataContext _context;

        public RecipeService(applicationDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            return await _context.recipes.Include(r=>r.recipeIngredients).ThenInclude(x=>x.ingredient).ToListAsync();
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            return await _context.recipes.Include(r=>r.recipeIngredients).ThenInclude(x=>x.ingredient).FirstOrDefaultAsync(x=>x.recipeId==id);
        }

        public async Task<Recipe> CreateRecipe(RecipeRequest request)
        {
            var recipe = new Recipe
            {
                recipeName = request.recipeName,
                steps = request.steps,
                recipeIngredients = new List<RecipeIngredient>()
            };

            foreach (var ingredientRequest in request.ingredientRequests)
            {
               
                var existingIngredient = await _context.ingredients
                    .FirstOrDefaultAsync(i => i.IngredientId == ingredientRequest.IngredientId);

                if (existingIngredient == null)
                {
                   
                    existingIngredient = new Ingredient
                    {
                        IngredientId = ingredientRequest.IngredientId,
                        IngredientName = ingredientRequest.IngredientName,
                        quantity = ingredientRequest.quantity
                    };
                    _context.ingredients.Add(existingIngredient);
                }

                var recipeIngredient = new RecipeIngredient
                {
                    IngredientId = existingIngredient.IngredientId,
                    quantity = ingredientRequest.quantity,
                    ingredient = existingIngredient
                };

                recipe.recipeIngredients.Add(recipeIngredient);
            }

            _context.recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }



        public async Task<bool> UpdateRecipe(int id, RecipeRequest request)
        {
            if (id != request.recipeId)
            {
                return false;
            }
            try
            {
               
                var existingRecipe = await _context.recipes
                    .Include(r => r.recipeIngredients)
                    .FirstOrDefaultAsync(r => r.recipeId == id);

                if (existingRecipe == null)
                {
                    return false; 
                }

                
                existingRecipe.recipeName = request.recipeName;
                existingRecipe.steps = request.steps;

                
                foreach (var ingredientRequest in request.ingredientRequests)
                {
                  
                    var existingIngredient = existingRecipe.recipeIngredients
                        .FirstOrDefault(ri => ri.IngredientId == ingredientRequest.IngredientId);

                    if (existingIngredient != null)
                    {
                        existingIngredient.quantity = ingredientRequest.quantity;
                    }
                    else
                    {
                        
                        existingRecipe.recipeIngredients.Add(new RecipeIngredient
                        {
                            IngredientId = ingredientRequest.IngredientId,
                            quantity = ingredientRequest.quantity
                        });
                    }
                }

               
                foreach (var ingredient in existingRecipe.recipeIngredients)
                {
                    var ingredientRequest = request.ingredientRequests
                        .FirstOrDefault(ir => ir.IngredientId == ingredient.IngredientId);

                    if (ingredientRequest != null)
                    {
                        ingredient.ingredient.IngredientName = ingredientRequest.IngredientName;
                    }
                }

                
                _context.Entry(existingRecipe).State = EntityState.Modified;

                
                await _context.SaveChangesAsync();

                return true; 
            }
            catch (DbUpdateConcurrencyException ex)
            {
                
                if (!_context.recipes.Any(r => r.recipeId == id))
                {
                    return false; 
                }
                else
                {
                    throw; 
                }
            }
        }


        public async Task<bool> DeleteRecipe(int id)
        {
            var recipe = await _context.recipes.FindAsync(id);
            if (recipe == null)
            {
                return false;
            }
            _context.recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
