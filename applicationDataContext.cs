using Microsoft.EntityFrameworkCore;
using recipe.Models;
using RecipeApp.Models;

namespace RecipeApp
{
    public class applicationDataContext : DbContext
    {
        public applicationDataContext(DbContextOptions<applicationDataContext> options) : base(options) { }

        public DbSet<UserModel> users { get; set; }
        public DbSet<Recipe> recipes { get; set; }
        public DbSet<Ingredient> ingredients { get; set; }
        public DbSet<RecipeIngredient> recipeIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>()
                .Property(u => u.otpToken)
                .IsRequired(false)
                .HasDefaultValue(null);

            modelBuilder.Entity<UserModel>()
                .Property(u => u.OtpTokenExpiry)
                .IsRequired(false)
                .HasDefaultValue(null);

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.recipe)
                .WithMany(r => r.recipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.ingredient)
                .WithMany(i => i.recipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);
        }

        public void SeedData()
        {
            var adminExists = this.users.Any(u => u.userName == "admin" && u.role == "admin");
            if (!adminExists)
            {
                this.Database.ExecuteSqlRaw(@"
                    INSERT INTO users (Username, Email, Password, role, phoneNumber, emailConfirmed, age, gender) 
                    VALUES ('admin', 'adityabisht8436@gmail.com', 'Aditya@1234', 'admin', 1234, true, 18, 'Male')");
            }
        }
    }
}
