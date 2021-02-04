using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    // Repo for communicationg with redis
    public interface IBasketRepository
    {
         Task<CustomerBasket> GetBasketAsync(string basketId);
         Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
         Task<bool> DeleteBasketAsync(string basketId);

    }
}