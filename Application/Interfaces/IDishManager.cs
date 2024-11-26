using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{

    public interface IDishManager
    {
        /// <summary>
        /// Constructs a list of dishes, each dish with a name and a count
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<List<Dish>> GetDishesAsync(Order order);
    }
}