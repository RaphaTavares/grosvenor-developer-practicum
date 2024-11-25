using System;
using Application;

namespace GrosvenorInHousePracticum
{
    class Program
    {
        static void Main()
        {
            var server = new Server(new DishManager());
            while (true)
            {
                var unparsedOrder = Console.ReadLine();
                var output = server.TakeOrder(unparsedOrder);
                Console.WriteLine(output);
            }
        }
    }
}
