namespace Contracts;

public record GetDestinationsRequest(string? Input);

public record DestinationsResponse(List<DestinationResponse> Destinations, List<Error>? Errors = null);

public record DestinationResponse(string Name, string? Description, Uri? ImageUri);

public record Error(string Message);