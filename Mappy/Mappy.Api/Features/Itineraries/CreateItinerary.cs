using Mappy.Api.Common;
using Mappy.Application.Common.Interfaces;
using Mappy.Application.Itineraries;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace Mappy.Api.Features.Itineraries;

public class CreateItinerary: ISlice
{
    private AuthorizationPolicy AuthorizationPolicy => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("scope", "write")
        .Build();
        
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/itineraries", async (CreateItineraryRequest request, 
            ICurrentUserService currentUserService,
            ISender sender) =>
        {
             if (currentUserService.UserId is null || !Guid.TryParse(currentUserService.UserId, out var userId))
                 return Results.Unauthorized();

             CreateItineraryCommand command = new CreateItineraryCommand(request.Name, request.Description, userId);
             
             Guid result = await sender.Send(command);
             
             return Results.CreatedAtRoute("GetItinerary", new {  itineraryId = result }, result);
             
        }).RequireAuthorization(AuthorizationPolicy);
    }
    
    private record CreateItineraryRequest(string Name, string? Description);
}