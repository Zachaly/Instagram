using Instagram.Models.Response;

namespace Instagram.Mobile
{
    public class InvalidRequestException : Exception
    {
        public ResponseModel Response { get; }

        public InvalidRequestException(ResponseModel response)
        {
            Response = response;
        }
    }
}
