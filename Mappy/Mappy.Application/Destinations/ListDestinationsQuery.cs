using Mappy.Application.Common.Interfaces;
using MediatR;
using SharedKernel;

namespace Mappy.Application.Destinations;

public record ListDestinationsQuery(string? Search): IRequest<DestinationsResponse>;

public class ListDestinationsQueryHandler: IRequestHandler<ListDestinationsQuery, DestinationsResponse>
{
    private readonly IDestinationsService _destinationsService;
    
    public ListDestinationsQueryHandler(IDestinationsService destinationsService)
    {
        _destinationsService = destinationsService;
    }

    public async Task<DestinationsResponse> Handle(ListDestinationsQuery request, CancellationToken cancellationToken)
    {
        return await _destinationsService.SearchAsync(request.Search, cancellationToken);
    }
}