using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace Instagram.Application
{
    public class ResponseFactory : IResponseFactory
    {
        public ResponseModel CreateFailure(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public DataResponseModel<TData> CreateFailure<TData>(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateSuccess()
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateSuccess(long newEntityId)
        {
            throw new NotImplementedException();
        }

        public DataResponseModel<TData> CreateSuccess<TData>(TData data)
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateValidationError(ValidationResult validationResult)
        {
            throw new NotImplementedException();
        }
    }
}
