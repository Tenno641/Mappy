using ErrorOr;

namespace Mappy.Api.Common.Validation;

public static class ErrorExtensions
{
    public static IResult ToValidationProblem(this List<Error> errors)
    {
        var validationProblems = errors.GroupBy(error => error.Code)
            .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray());

        return Results.ValidationProblem(validationProblems);
    }

    public static IResult ToProblemDetails(this List<Error> errors)
    {
        Error error = errors.First();
        
        return Results.Problem(
            statusCode: ErrorTypeToStatusCode(error.Type),
            type: error.Code,
            detail: error.Description);
    }
    
    public static int ErrorTypeToStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };
}