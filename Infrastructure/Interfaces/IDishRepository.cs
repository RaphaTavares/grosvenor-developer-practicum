using Domain;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IDishRepository
    {
        Task<IReadOnlyCollection<Dish>> GetByFilterAsync(Expression<Func<Dish, bool>> filter);
    }
}
