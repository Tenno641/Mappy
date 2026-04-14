using Mappy.Api.Common;
using Mappy.Domain.Itineraries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mappy.Api.Features.Itineraries;

public class GetItineraries: ISlice
{
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/itineraries", async ([FromQuery] string? search,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            Application.Itineraries.GetItineraries query = new Application.Itineraries.GetItineraries(search);
            
            List<Itinerary> itineraries = await sender.Send(query, cancellationToken);
            
            return Results.Ok(itineraries);
        });
    }
}