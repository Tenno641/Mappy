using SharedKernel;

namespace Mappy.Application.Common.Interfaces;

public interface IDestinationsService
{
    public Task<DestinationsResponse> SearchAsync(string? input, CancellationToken cancellationToken = default);
}