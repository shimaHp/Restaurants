using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepositry(RestaurantsDbContext  dbContext) : IRestaurantRepository
{
    public async Task<int> Create(Restaurant entity)
    {
       dbContext.Restaurants.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        dbContext.Restaurants.Remove(entity);
        await dbContext.SaveChangesAsync();
      
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restauators = await dbContext.Restaurants.Include(r => r.Dishes).ToListAsync();
        return restauators;
  
     
    }

    public async Task<(IEnumerable<Restaurant>,int)> GetAllMatchingAsync(string? searchPhrase
        ,int pageSize
        ,int pageNumber
        ,string? sortBy
        ,SortDirection sortDirection
        )
    {
        var searchPhraseToLower = searchPhrase?.ToLower();

        var baseQuery = dbContext
            .Restaurants.Include(r => r.Dishes)
            .Where(r => searchPhraseToLower == null ||
            (r.Description.ToLower().Contains(searchPhraseToLower)
            || r.Name.ToLower().Contains(searchPhraseToLower)));

        var totalCount=await baseQuery.CountAsync();
        if(sortBy != null)
        {
            var columnSelector= new Dictionary<string, Expression<Func<Restaurant, object>>>
            {     

                { nameof(Restaurant.Name), r=>r.Name},
                { nameof(Restaurant.Description), r=>r.Description},
                { nameof(Restaurant.Category), r=>r.Category},
            };
            var selectedColumn = columnSelector[sortBy];
            baseQuery = sortDirection== SortDirection.Asceding
              ?  baseQuery.OrderBy(selectedColumn)
              :  baseQuery.OrderByDescending(selectedColumn);
        }


        var restauators = await baseQuery
            .Skip(pageSize*(pageNumber-1))
            .Take(pageSize)
            .ToListAsync();

      

        return (restauators,totalCount);


    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r=>r.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
        return restaurant;
    }

    public Task saveChanges()
          =>dbContext.SaveChangesAsync();
    
}
