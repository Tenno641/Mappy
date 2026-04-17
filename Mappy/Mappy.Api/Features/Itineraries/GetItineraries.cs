using Mappy.Api.Common;
using Mappy.Domain.Itineraries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mappy.Api.Features.Itineraries;

public sealed class GetItineraries: ISlice
{
    private static AuthorizationPolicy AuthorizationPolicy => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("feature", "get-itineraries")
        .Build();
    
    public void AddEndPoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/itineraries", async ([FromQuery] string? search,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            Application.Itineraries.GetItineraries query = new Application.Itineraries.GetItineraries(search);
            
            List<Itinerary> itineraries = await sender.Send(query, cancellationToken);
            
            return Results.Ok(itineraries);
        }).RequireAuthorization(AuthorizationPolicy);
    }
}