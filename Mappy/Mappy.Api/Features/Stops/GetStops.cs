using Mappy.Api.Common;
using Mappy.Application.Stops;
using Mappy.Domain.Stops;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mappy.Api.Features.Stops;

public class GetStops: ISlice
{
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/itineraries/{itineraryId:guid}/stops", async (Guid itineraryId,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            GetStopsQuery query = new GetStopsQuery(itineraryId);

            List<Stop> stops = await sender.Send(query, cancellationToken);
            
            return Results.Ok(stops);
        });
    }
}