using ErrorOr;
using Mappy.Api.Common;
using Mappy.Application.Stops;
using Mappy.Domain.Stops;
using MediatR;

namespace Mappy.Api.Features.Stops;

public class GetStop: ISlice
{
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/itineraries/{itineraryId:guid}/stops/{stopId:guid}", async (Guid itineraryId,
            Guid stopId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            GetStopQuery query = new GetStopQuery(itineraryId, stopId);
            
            ErrorOr<Stop> result = await sender.Send(query, cancellationToken);

            return result.IsError
                ? Results.BadRequest(result.Errors)
                : Results.Ok(result.Value);
        }).WithName("GetStop");
    }
}