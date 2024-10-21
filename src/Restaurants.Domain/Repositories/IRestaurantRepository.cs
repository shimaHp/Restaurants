using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>>   GetAllAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task<int> Create (Restaurant entity);
        Task Delete(Restaurant entity);
        Task saveChanges();
        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber,string? sortBy,SortDirection sortDirection);
    }
}
