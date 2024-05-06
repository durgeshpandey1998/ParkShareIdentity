

using ParkShareIdentity.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ParkShareIdentity.Shared
{
    public class ValidateModelState
    {
        public dynamic CheckModelStateIsValid(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                List<FieldError> listFieldError = new List<FieldError>();
                foreach (var error in modelState.ToList())
                {
                    FieldError fieldError = new FieldError();
                    fieldError.Property = error.Key.ToString();
                    fieldError.ErrorMessage = error.Value.Errors.FirstOrDefault()
                                              .ErrorMessage.ToString();
                    listFieldError.Add(fieldError);
                }
                return listFieldError;
            }
            return null;
        }
    }
}
