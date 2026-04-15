using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Mappy.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest: IRequest<TResponse>
    where TResponse: IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    
    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validator is null)
            return await next(cancellationToken);
        
        ValidationResult validationResult = _validator.Validate(request);
        
        if (validationResult.IsValid)
            return await next(cancellationToken);

        List<Error> errors = validationResult.Errors
            .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
            .ToList();

        return (dynamic)errors;
    }
}