using Mappy.Api.Common;
using Mappy.Application.Stops;
using MediatR;
using ErrorOr;
using Mappy.Domain.Itineraries;

namespace Mappy.Api.Features.Stops;

public class CreateStop: ISlice
{
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/itineraries/{itineraryId:guid}/stops", async (Guid itineraryId,
            CreateStopRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            CreateStopCommand command = new CreateStopCommand(request.Name, request.ImageUri, itineraryId);
            
            ErrorOr<Guid> result = await sender.Send(command, cancellationToken);
            
            return result.IsError 
                ? Results.BadRequest(result.Errors)
                : Results.CreatedAtRoute("GetStop", new { itineraryId = itineraryId, StopId = result.Value }, result.Value);
        });
    }

    private abstract record CreateStopRequest(string Name, string? ImageUri);
}