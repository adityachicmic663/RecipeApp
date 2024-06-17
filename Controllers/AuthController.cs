using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Models;

namespace RecipeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterModel registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel
                {
                    statusCode = 400,
                    message = "Invalid data",
                    data = "No data",
                    isSuccess = false
                });
            }

            try
            {
                var token = _authService.Register(registerRequest);
                if (token == null)
                {
                    return Conflict(new ResponseModel
                    {
                        statusCode = 409,
                        message = "Email already exists",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 201,
                    message = "Registered successfully",
                    data = token,
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

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginModel loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel
                {
                    statusCode = 400,
                    message = "Invalid data",
                    data = "No data",
                    isSuccess = false
                });
            }

            try
            {
                var token = _authService.Login(loginRequest);
                if (token == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "User not found or invalid credentials",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Login successful",
                    data = token,
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

        [HttpPost("userpasswordreset")]
        public IActionResult PasswordReset(PasswordModel passwordRequest)
        {
            try
            {
                var result = _authService.PasswordReset(passwordRequest);
                if (result == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "User not found or invalid credentials",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Password updated successfully",
                    data = "No data",
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

        [Authorize(Roles = "admin")]
        [HttpPost("adminPasswordReset")]
        public IActionResult AdminPasswordReset(AdminPasswordModel passwordRequest)
        {
            try
            {
                var result = _authService.AdminPasswordReset(passwordRequest);
                if (result == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        statusCode = 400,
                        message = "Invalid credentials",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Password updated successfully",
                    data = "No data",
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

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(forgetPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel
                {
                    statusCode = 400,
                    message = "Invalid data",
                    data = "No data",
                    isSuccess = false
                });
            }

            try
            {
                var result = _authService.Forgotpassword(forgotPasswordModel.email);
                if (!result)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "User not found or invalid email",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Password reset email sent successfully",
                    data = "No data",
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

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public IActionResult ResetPasswordWithOtp(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel
                {
                    statusCode = 400,
                    message = "Invalid data",
                    data = "No data",
                    isSuccess = false
                });
            }

            try
            {
                var result = _authService.resetPasswordWithOtp(resetPasswordModel);
                if (!result)
                {
                    return BadRequest(new ResponseModel
                    {
                        statusCode = 400,
                        message = "OTP do not match or unauthorized",
                        data = "No data",
                        isSuccess = false
                    });
                }

                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "Password has been reset",
                    data = "No data",
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
    }
}
