using ErrorOr;
using FluentValidation;
using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries;
using Mappy.Domain.Stops;
using MediatR;

namespace Mappy.Application.Stops;

public record CreateStopCommand(string Name, string? ImageUri, Guid ItineraryId) : IRequest<ErrorOr<Guid>>;

public class CreateStop: IRequestHandler<CreateStopCommand, ErrorOr<Guid>>
{
    private readonly IItinerariesRepository _itinerariesRepository;
    private readonly IStopsRepository _stopsRepository;
    
    public CreateStop(IItinerariesRepository itinerariesRepository, IStopsRepository stopsRepository)
    {
        _itinerariesRepository = itinerariesRepository;
        _stopsRepository = stopsRepository;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateStopCommand request, CancellationToken cancellationToken)
    {
        Itinerary? itinerary = await _itinerariesRepository.GetByIdAsync(request.ItineraryId);
        
        if (itinerary is null)
            return Error.NotFound(description: "Itinerary is not found");
        
        Uri? imageUri = request.ImageUri is null ? null : new Uri(request.ImageUri);

        Stop stop = new Stop(request.Name, imageUri);
        
        ErrorOr<Success> result = itinerary.AddStop(stop);

        if (result.IsError)
            return result.Errors;
        
        await _itinerariesRepository.UpdateAsync(itinerary);
        
        await _stopsRepository.CreateStopAsync(stop);

        return stop.Id;
    }
}

public sealed class CreateStopCommandValidator : AbstractValidator<CreateStopCommand>
{
    public CreateStopCommandValidator()
    {
        RuleFor(s => s.ImageUri)
            .Must(imageUri => Uri.TryCreate(imageUri, UriKind.Absolute, out _))
            .When(s => !string.IsNullOrEmpty(s.ImageUri));

        RuleFor(s => s.Name)
            .MaximumLength(128)
            .NotEmpty();

        RuleFor(s => s.Name)
            .EmailAddress();
    }
}