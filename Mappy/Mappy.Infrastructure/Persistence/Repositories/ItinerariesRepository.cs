using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries;
using Microsoft.EntityFrameworkCore;

namespace Mappy.Infrastructure.Persistence.Repositories;

public class ItinerariesRepository: IItinerariesRepository
{
    private readonly MappyDbContext _dbContext;
    
    public ItinerariesRepository(MappyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Itinerary>> ListItinerariesAsync(string? search)
    {
        return await _dbContext.Itineraries
            .Where(i =>
                search == null ||
                i.Name.ToLower().Contains(search.ToLower())
                || (i.Description != null && i.Description.ToLower().Contains(search.ToLower())))
            // TODO: is not null doesn't work here WHY TF 
            .ToListAsync();
    }
}