using Mappy.Api.Common;
using Mappy.Application.Stops;
using MediatR;
using ErrorOr;
using Mappy.Api.Common.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mappy.Api.Features.Stops;

public class CreateStop: ISlice
{
    private static AuthorizationPolicy AuthorizationPolicy => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("scope", "write")
        .Build();
    
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/itineraries/{itineraryId:guid}/stops", async (Guid itineraryId,
            CreateStopRequest request,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            CreateStopCommand command = new CreateStopCommand(request.Name, request.ImageUri, itineraryId);
            
            ErrorOr<Guid> result = await sender.Send(command, cancellationToken);
            
            return result.IsError 
                ? result.Errors.ToValidationProblem()
                : Results.CreatedAtRoute("GetStop", new { itineraryId = itineraryId, StopId = result.Value }, result.Value);
            
        }).RequireAuthorization(AuthorizationPolicy);
    }

    private record CreateStopRequest(string Name, string? ImageUri);
}