namespace RecipeApp.Models
{
    public interface IAuthService
    {
        string Login(LoginModel loginRequest);

        string Register(RegisterModel registerRequest);

        string PasswordReset(PasswordModel passwordRequest);

        string AdminPasswordReset(AdminPasswordModel passwordRequest);

        bool Forgotpassword(string email);

        bool resetPasswordWithOtp(ResetPasswordModel resetPasswordRequest);
    }
}
