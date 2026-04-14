using Mappy.Api.Common;
using Mappy.Application.Destinations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Mappy.Api.Features.Destinations;

public class GetDestinations: ISlice
{
    public void AddEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/destinations", async ([FromQuery] string? search, 
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            ListDestinationsQuery query = new ListDestinationsQuery(search);
            
            DestinationsResponse response = await sender.Send(query, cancellationToken);
            
            return response.Errors is not null
                ? Results.BadRequest(response.Errors)
                : Results.Ok(response.Destinations);
        });
    }
}