
using System.ComponentModel.DataAnnotations;

namespace MyAirbnb.Models;

public static class ObjectValidationHelper
{
    /// <summary>
    /// Validate a model with data annotation
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <returns>Error Message if any</returns>
    public static string ValidateAndGetFirstErrorMessage<TModel>(TModel model)
    {
        if (model is null) return null;

        var context = new ValidationContext(model, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(model, context, validationResults, true);

        if (isValid is false && validationResults is { Count: > 0})
        {
            return validationResults.First().ErrorMessage ?? "Object is not valid!";
        }

        return null!;
    }
}
