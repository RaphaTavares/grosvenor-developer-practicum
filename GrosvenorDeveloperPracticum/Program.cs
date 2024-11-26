using Application;
using Domain;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrosvenorInHousePracticum
{
    class Program
    {
        static async Task Main()
        {
            // Configure the dependency container
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
                Console.WriteLine("Checking database...");
                await EnsureDatabaseCreatedAsync(dbContext);
            }

            var server = serviceProvider.GetRequiredService<IServer>();
            await RunApplication(server);
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            var configuration = LoadConfiguration();


            services.AddDbContext<RestaurantDbContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("RestaurantDb")));

            services.AddScoped<IDishRepository, DishRepository>();

            services.AddScoped<IDishManager, DishManager>();
            services.AddScoped<IServer, Server>();

            return services;
        }

        private static IConfiguration LoadConfiguration()
        {
            Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");


            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        private static async Task EnsureDatabaseCreatedAsync(RestaurantDbContext dbContext)
        {
            try
            {
                // Verifica se há migrações pendentes
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    await dbContext.Database.MigrateAsync();
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("Database is up-to-date. No migrations are required.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while ensuring database creation: {ex.Message}");
                throw; // Re-lança a exceção para tratá-la em níveis superiores, se necessário
            }
        }

        private static async Task RunApplication(IServer server)
        {
            DisplayWelcomeArt();
            var allOptions = await server.TakeAllAsync();
            DisplayDishArt(allOptions);
            
            while (true)
            {
                Console.WriteLine("Enter your order:");
                var unparsedOrder = Console.ReadLine();
                var output = await server.TakeOrderAsync(unparsedOrder);
                Console.WriteLine(output);
            }
        }

        private static void DisplayWelcomeArt()
        {
            Console.WriteLine(@"                 __      __     _                        _          _    _                          
                 \ \    / /___ | | __  ___  _ __   ___  | |_  ___  | |_ | |_   ___                  
                  \ \/\/ // -_)| |/ _|/ _ \| '  \ / -_) |  _|/ _ \ |  _|| ' \ / -_)                 
   ___             \_/\_/ \___||_|\__|\___/|_|_|_|\___|  \__|\___/  \__||_||_|\___|             _   
  / __| _ _  ___  _____ __ ___  _ _   ___  _ _  | _ \ ___  ___| |_  __ _  _  _  _ _  __ _  _ _ | |_ 
 | (_ || '_|/ _ \(_-<\ V // -_)| ' \ / _ \| '_| |   // -_)(_-<|  _|/ _` || || || '_|/ _` || ' \|  _|
  \___||_|  \___//__/ \_/ \___||_||_|\___/|_|   |_|_\\___|/__/ \__|\__,_| \_,_||_|  \__,_||_||_|\__|
                                                                                                    ");
        }
        private static void DisplayDishArt(IEnumerable<Dish> allOptions)
        {
            // Agrupa os pratos pelo tipo de serviço
            var groupedByServingType = allOptions
                .GroupBy(dish => dish.ServingTime)
                .OrderBy(group => group.Key);

            foreach (var group in groupedByServingType)
            {
                Console.WriteLine($"=== {group.Key.ToString().ToUpper()} ===");
                Console.WriteLine("┌──────────────┬──────────────┬──────────────┐");
                Console.WriteLine("│ Dish Type    │ Dish Name    │ Max Quantity │");
                Console.WriteLine("├──────────────┼──────────────┼──────────────┤");

                foreach (var dish in group)
                {
                    var quantityDisplay = dish.MaxQuantity == int.MaxValue ? "Unlimited" : dish.MaxQuantity.ToString();
                    Console.WriteLine(
                        $"│ {dish.DishType.ToString().PadRight(8)} ({((int)dish.DishType).ToString()}) │ {dish.Name.PadRight(12)} │ {quantityDisplay.PadRight(12)} │"
                    );
                }

                Console.WriteLine("└──────────────┴──────────────┴──────────────┘\n");
            }
        }



    }
}
