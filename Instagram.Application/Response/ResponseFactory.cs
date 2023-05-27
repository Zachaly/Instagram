using FluentValidation.Results;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class ResponseFactory : IResponseFactory
    {
        public ResponseModel CreateFailure(string errorMessage)
            => new ResponseModel
            {
                Error = errorMessage,
                Success = false
            };

        public DataResponseModel<TData> CreateFailure<TData>(string errorMessage)
            => new DataResponseModel<TData> 
            {
                Error = errorMessage,
                Success = false,
                Data = default
            };

        public ResponseModel CreateSuccess()
            => new ResponseModel { Success = true };

        public ResponseModel CreateSuccess(long newEntityId)
            => new ResponseModel { Success = true, NewEntityId = newEntityId };

        public DataResponseModel<TData> CreateSuccess<TData>(TData data)
            => new DataResponseModel<TData>
            {
                Success = true,
                Data = data
            };

        public ResponseModel CreateValidationError(ValidationResult validationResult)
            => new ResponseModel
            {
                Success = false,
                ValidationErrors = validationResult.ToDictionary()
            };
    }
}
