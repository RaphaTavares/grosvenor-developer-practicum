using Domain;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public class DishManager : IDishManager
    {
        private readonly DishRepository _dishRepository;

        public DishManager(DishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        /// <summary>
        /// Takes an Order object, sorts the orders and builds a list of dishes to be returned. 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<List<Dish>> GetDishesAsync(Order order)
        {
            var returnValue = new List<Dish>();
            order.Dishes.Sort();
            foreach (var dishType in order.Dishes)
            {
                await AddOrderToListAsync(dishType, order.ServingTime, returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Takes an int, representing an order type, tries to find it in the list.
        /// If the dish type does not exist, add it and set count to 1
        /// If the type exists, check if multiples are allowed and increment that instances count by one
        /// else throw error
        /// </summary>
        /// <param name="order">int, represents a dishtype</param>
        /// <param name="returnValue">a list of dishes, - get appended to or changed </param>
        private async Task AddOrderToListAsync(int order, ServingTime servingTime, List<Dish> returnValue)
        {
            var dish = await GetOrderAsync(order, servingTime); // note: could refactor this to get all needed order names, making only 1 database call
            var existingOrder = returnValue.SingleOrDefault(x => x.Name == dish.Name.ToLower());
            if (existingOrder == null)
            {
                returnValue.Add(dish); // note: refactored Dish constructor to meet encapsulation

            } else if (existingOrder.IsQuantityAllowed(1)) // note: refactored method to place it inside Dish class and verify if the specified amount is allowed
            {
                existingOrder.Add(1);
            }
            else
            {
                throw new ApplicationException(string.Format("Quantity requested for {0}(s) is not allowed", dish));
            }
        }

        private async Task<Dish> GetOrderAsync(int orderType, ServingTime servingTime)
        {
            var result = await _dishRepository.GetByFilterAsync(x => (int)x.DishType == orderType && x.ServingTime == servingTime);

            return result?.FirstOrDefault() ?? throw new ApplicationException("Dish does not exist");
        }
    }
}