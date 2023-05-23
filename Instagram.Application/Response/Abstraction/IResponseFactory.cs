using Instagram.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace Instagram.Application.Abstraction
{
    public interface IResponseFactory
    {
        ResponseModel CreateSuccess();
        ResponseModel CreateSuccess(long newEntityId);
        DataResponseModel<TData> CreateSuccess<TData>(TData data);

        ResponseModel CreateFailure(string errorMessage);
        DataResponseModel<TData> CreateFailure<TData>(string errorMessage);
        ResponseModel CreateValidationError(ValidationResult validationResult);
    }
}
