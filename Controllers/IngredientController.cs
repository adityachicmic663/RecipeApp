using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Models;

namespace RecipeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel>> getIngredients()
        {
            try
            {
                var ingredients = await _ingredientService.getIngredients();
                if (ingredients == null || !ingredients.Any())
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Ingredients not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Ingredients retrieved successfully",
                    data = ingredients,
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
        public async Task<ActionResult<ResponseModel>> GetIngredient(int id)
        {
            try
            {
                var ingredient = await _ingredientService.GetIngredient(id);
                if (ingredient == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Ingredient not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Ingredient retrieved successfully",
                    data = ingredient,
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
        public async Task<ActionResult<ResponseModel>> CreateIngredient(IngredientRequest ingredient)
        {
            try
            {
                var newIngredient = await _ingredientService.CreateIngredient(ingredient);
                return CreatedAtAction(nameof(GetIngredient), new { id = newIngredient.IngredientId }, new ResponseModel
                {
                    statusCode = 201,
                    message = "Ingredient created successfully",
                    data = newIngredient,
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
        public async Task<ActionResult<ResponseModel>> UpdateIngredient(int id, IngredientRequest ingredient)
        {
            try
            {
                var updatedIngredient = await _ingredientService.UpdateIngredient(id, ingredient);
                if (updatedIngredient == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Ingredient not found",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Ingredient updated successfully",
                    data = updatedIngredient,
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
        public async Task<ActionResult<ResponseModel>> DeleteIngredient(int id)
        {
            try
            {
                var result = await _ingredientService.DeleteIngredient(id);
                if (!result)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "Ingredient not found",
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
