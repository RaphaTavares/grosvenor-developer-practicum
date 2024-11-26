using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public sealed class RestaurantDbContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Configura o SQLite
        //    optionsBuilder.UseSqlite("Data Source=C:\\Users\\rapha\\Downloads\\GrosvenorDeveloperPracticum\\GrosvenorDeveloperPracticum-master\\Infrastructure\\restaurant.db");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da tabela Dish
            modelBuilder.Entity<Dish>(entity =>
            {
                // Configura o Id para ser gerado automaticamente ao adicionar
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // Outras configurações de propriedades
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.MaxQuantity)
                      .IsRequired();

                entity.Property(e => e.DishType)
                      .IsRequired();

                entity.Property(e => e.ServingTime)
                      .IsRequired();

                Dish steak = new("steak", ServingTime.Evening, DishType.Entree, 1) { Id = 1};
                Dish potato = new("potato", ServingTime.Evening, DishType.Side, int.MaxValue) { Id = 2};
                Dish wine = new("wine", ServingTime.Evening, DishType.Drink, 1) { Id = 3};
                Dish cake = new("cake", ServingTime.Evening, DishType.Dessert, 1) { Id = 4 };
                Dish egg = new("egg", ServingTime.Morning, DishType.Entree, 1) { Id = 5 };
                Dish toast = new("toast", ServingTime.Morning, DishType.Side, 1) { Id = 6 };
                Dish coffee = new("coffee", ServingTime.Morning, DishType.Drink, int.MaxValue) { Id = 7 };

        entity.HasData(
                    steak,
                    potato,
                    wine,
                    cake,
                    egg,
                    toast,
                    coffee);
            });
        }
    }
}
