using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Stops;
using Microsoft.EntityFrameworkCore;

namespace Mappy.Infrastructure.Persistence.Repositories;

public class StopsRepository: IStopsRepository
{
    private readonly MappyDbContext _dbContext;
    
    public StopsRepository(MappyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Stop>> GetStopsByItineraryId(Guid itineraryId)
    {
        return await _dbContext.Stops.Where(s => s.ItineraryId == itineraryId).ToListAsync();
    }
    
    public async Task CreateStopAsync(Stop stop)
    {
        _dbContext.Stops.Add(stop);
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Stop?> GetStopAsync(Guid stopId)
    {
        return await _dbContext.Stops.FirstOrDefaultAsync(s => s.Id == stopId);
    }
}