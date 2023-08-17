using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using NSubstitute;

namespace Instagram.Tests.Unit
{
    public static class ResponseFactoryMock
    {
        public static IResponseFactory Create()
        {
            var factory = Substitute.For<IResponseFactory>();
            factory.CreateSuccess()
                .Returns(new ResponseModel { Success = true });

            factory.CreateFailure(Arg.Any<string>())
                .ReturnsForAnyArgs((info) 
                    => new ResponseModel 
                    { 
                        Success = false,
                        Error = info.ArgAt<string>(0)
                    });

            factory.CreateSuccess(Arg.Any<long>())
                .ReturnsForAnyArgs((info)
                => new ResponseModel
                {
                    Success = true,
                    NewEntityId = info.ArgAt<long>(0)
                });

            return factory;
        }
    }
}
