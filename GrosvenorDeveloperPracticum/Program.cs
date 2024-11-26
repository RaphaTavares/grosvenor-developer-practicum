using Application;
using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            var server = serviceProvider.GetRequiredService<IServer>();
            await RunApplication(server);
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<RestaurantDbContext>();

            services.AddTransient<DishRepository>();

            services.AddScoped<IDishManager, DishManager>();
            services.AddScoped<IServer, Server>();

            return services;
        }

        private static async Task RunApplication(IServer server)
        {
            while (true)
            {
                Console.WriteLine("Enter your order:");
                var unparsedOrder = Console.ReadLine();
                var output = await server.TakeOrderAsync(unparsedOrder);
                Console.WriteLine(output);
            }
        }

    }
}
