using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeApp.Models
{
    public class AuthService:IAuthService
    {
        private readonly applicationDataContext _context;
        private readonly string _secretKey;
        private readonly IEmailSender _emailSender;

        public AuthService(applicationDataContext context,IOptions<JwtSettings> jwtSettings,IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _context = context;
            _secretKey = jwtSettings.Value.SecretKey;
        }
        public string Register(RegisterModel registerRequest)
        {
            if (_context.users.Any(x => x.Email == registerRequest.email))
            {
                return null;
            }

            var newUser = new UserModel
            {
                userName = registerRequest.userName,
                Email = registerRequest.email,
                Password = registerRequest.password,
                role = "user"
            };

            _context.users.Add(newUser);
            _context.SaveChanges();

            return Login(new LoginModel { email = registerRequest.email, password = registerRequest.password });
        }

        public string Login(LoginModel loginRequest)
        {
            var user = _context.users.SingleOrDefault(x => x.Email == loginRequest.email &&
            x.Password == loginRequest.password);

            if (user == null)
            {
                return null;
            }
            if (user.role == "admin")
            {
                Console.WriteLine(GenerateJwtToken(user));
            }
            return GenerateJwtToken(user);
        }
       
        public string PasswordReset(PasswordModel PasswordRequest)
        {
            var user = _context.users.SingleOrDefault(x => x.Email == PasswordRequest.email && x.Password == PasswordRequest.oldPassword);
            if(user==null)
            {
                return null;
            }
            user.Password = PasswordRequest.newPassword;
            _context.SaveChanges();
            return "password updated successfully";
        }

        public string AdminPasswordReset(AdminPasswordModel passwordRequest)
        {
            var user = _context.users.SingleOrDefault(a => a.Email == passwordRequest.Email);
            if (user == null)
            {
                return null;
            }
            user.Password = passwordRequest.NewPassword;
            _context.SaveChanges();

            return "Password updated successfully";
        }

        public bool Forgotpassword(string email)
        {
            var user = _context.users.SingleOrDefault(x => x.Email == email);
            
            if (user == null)
            {
                return false;
            }

            var token =  GeneratePasswordResetToken();
            user.otpToken = token;
            user.OtpTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            _context.SaveChanges();

            var callbackUrl = $"https://localhost:7097/swagger/index.html/reset-password?token={token}&email={user.Email}";

            _emailSender.SendEmailAsync(user.Email, "Reset password", $"please reset your password by clicking here:<a href='{callbackUrl}'>link</a>").Wait();

            return true;
        }
        public bool ValidateOtpToken(string email,string token)
        {
            var user = _context.users.SingleOrDefault(x => x.Email == email && x.otpToken == token);
            if (user == null || user.OtpTokenExpiry<DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

       
        public bool resetPasswordWithOtp(ResetPasswordModel resetPasswordRequest)
        {
            if (!ValidateOtpToken(resetPasswordRequest.Email, resetPasswordRequest.Token))
            {
                return false;
            }

            var user=_context.users.SingleOrDefault(x=>x.Email== resetPasswordRequest.Email);

            if (user == null)
            {
                return false;
            }
            user.Password= resetPasswordRequest.NewPassword;
            user.otpToken = null;
            user.OtpTokenExpiry = null;
            _context.SaveChanges();
            return true;
        }
        private  string GeneratePasswordResetToken()
        {
            return Guid.NewGuid().ToString();
        }

        private string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.userId.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,user.role)
                }),
                Expires = DateTime.Now.AddHours(5),
                SigningCredentials = new
                SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
