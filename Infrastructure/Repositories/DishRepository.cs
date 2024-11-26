using Domain;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public sealed class DishRepository : IDishRepository
    {
        private readonly RestaurantDbContext _context;

        public DishRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Dish>> GetByFilterAsync(Expression<Func<Dish, bool>> filter)
        {
            return await _context.Dishes
                .Where(filter)
                .Select(dish => new Dish(dish.Name, dish.ServingTime, dish.DishType, dish.MaxQuantity))
                .ToListAsync();
        }
    }
}
