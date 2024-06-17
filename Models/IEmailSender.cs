using System.Threading.Tasks;

namespace RecipeApp.Models
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
