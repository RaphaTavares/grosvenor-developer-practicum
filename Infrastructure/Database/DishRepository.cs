using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Database
{
    public sealed class DishRepository
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
                .ToListAsync();
        }
    }
}
