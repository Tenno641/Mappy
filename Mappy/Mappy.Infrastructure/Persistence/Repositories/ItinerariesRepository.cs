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
            .AsNoTracking()
            .Where(i =>
                search == null 
                || i.Name.ToLower().Contains(search.ToLower())
                || (i.Description != null && i.Description.ToLower().Contains(search.ToLower())))
            .ToListAsync();
    }
    
    public async Task<Itinerary?> GetByIdAsync(Guid itineraryId)
    {
        return await _dbContext.Itineraries
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == itineraryId);
    }
    
    public async Task UpdateAsync(Itinerary itinerary)
    {
        _dbContext.Itineraries.Update(itinerary);
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task AddAsync(Itinerary itinerary)
    {
        _dbContext.Itineraries.Add(itinerary);
        
        await _dbContext.SaveChangesAsync();
    }
}