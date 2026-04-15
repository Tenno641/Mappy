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
}