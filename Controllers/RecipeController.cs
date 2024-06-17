using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Models;

namespace RecipeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel>> GetRecipes()
        {
            try
            {
                var recipes = await _recipeService.GetRecipes();
                if (recipes == null || !recipes.Any())
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Recipes not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Recipes retrieved successfully",
                    data = recipes,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.Message,
                    isSuccess = false
                });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel>> GetRecipe(int id)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeById(id);
                if (recipe == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Recipe not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Recipe retrieved successfully",
                    data = recipe,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.Message,
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResponseModel>> CreateRecipe(RecipeRequest recipe)
        {
            try
            {
                var newRecipe = await _recipeService.CreateRecipe(recipe);
                return CreatedAtAction(nameof(GetRecipe), new { id = newRecipe.recipeId }, new ResponseModel
                {
                    statusCode = 201,
                    message = "Recipe created successfully",
                    data = newRecipe,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.Message,
                    isSuccess = false
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResponseModel>> UpdateRecipe(int id, RecipeRequest recipe)
        {
            try
            {
                var updatedRecipe = await _recipeService.UpdateRecipe(id, recipe);
                if (updatedRecipe == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Recipe not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Recipe updated successfully",
                    data = updatedRecipe,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.Message,
                    isSuccess = false
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResponseModel>> DeleteRecipe(int id)
        {
            try
            {
                var result = await _recipeService.DeleteRecipe(id);
                if (!result)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Recipe not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.Message,
                    isSuccess = false
                });
            }
        }
    }
}
