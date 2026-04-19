using ErrorOr;
using Mappy.Api.Common;
using Mappy.Api.Common.Validation;
using Mappy.Application.Itineraries;
using Mappy.Domain.Itineraries;
using MediatR;

namespace Mappy.Api.Features.Itineraries;

public class GetItinerary: ISlice
{
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/itineraries/{itineraryId:guid}", async (Guid itineraryId,
            ISender sender) =>
        {
            GetItineraryQuery query = new GetItineraryQuery(itineraryId);

            ErrorOr<Itinerary> result = await sender.Send(query);
            
            return result.IsError
                ? result.Errors.ToProblemDetails()
                : Results.Ok(result);
            
        }).WithName("GetItinerary");
    }
}