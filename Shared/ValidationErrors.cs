using FluentValidation.Results;

namespace ServiClientes.Shared
{
    public static class ValidationErrors
    {
        public static List<string> getValidationErrors(ValidationResult validationResult)
        {
            List<string> errors = new List<string>();

            foreach (var items in validationResult.Errors)
            {
                errors.Add(items.ErrorMessage);
            }

            return errors;
        }
    }
}
