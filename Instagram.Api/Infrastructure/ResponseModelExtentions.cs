using Instagram.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Api.Infrastructure
{
    public static class ResponseModelExtentions
    {
        public static ActionResult<DataResponseModel<T>> ReturnOkOrBadRequest<T>(this DataResponseModel<T> response)
        {
            if (response.Success)
            {
                return new OkObjectResult(response.Data);
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult<ResponseModel> ReturnOkOrBadRequest(this ResponseModel response)
        {
            if (response.Success)
            {
                return new OkObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult<ResponseModel> ReturnCreatedOrBadRequest(this ResponseModel response, string location)
        {
            if(response.Success)
            {
                return new CreatedResult(location + response.NewEntityId.ToString(), response);
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult<ResponseModel> ReturnNoContentOrBadRequest(this ResponseModel response)
        {
            if(response.Success)
            {
                return new NoContentResult();
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult<T> ReturnOkOrNotFound<T>(T data)
        {
            if(data is null)
            {
                return new NotFoundObjectResult(null);
            }

            return data;
        }
    }
}
