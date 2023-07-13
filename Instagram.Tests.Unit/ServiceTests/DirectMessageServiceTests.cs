using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class DirectMessageServiceTests
    {
        private readonly Mock<IDirectMessageRepository> _directMessageRepository;
        private readonly Mock<IDirectMessageFactory> _directMessageFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly DirectMessageService _service;

        public DirectMessageServiceTests()
        {
            _directMessageRepository = new Mock<IDirectMessageRepository>();
            _directMessageFactory = new Mock<IDirectMessageFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            _service = new DirectMessageService(_directMessageRepository.Object, _directMessageFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var messages = new List<DirectMessage>();

            const long NewId = 1;
            _directMessageRepository.Setup(x => x.InsertAsync(It.IsAny<DirectMessage>()))
                .Callback((DirectMessage message) => messages.Add(message))
                .ReturnsAsync(NewId);

            _directMessageFactory.Setup(x => x.Create(It.IsAny<AddDirectMessageRequest>()))
                .Returns((AddDirectMessageRequest request) => new DirectMessage { Content = request.Content });

            var request = new AddDirectMessageRequest
            {
                Content = "con",
                SenderId = 2,
                ReceiverId = 3
            };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Equal(NewId, res.NewEntityId);
            Assert.Contains(messages, x => x.Content == request.Content);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _directMessageFactory.Setup(x => x.Create(It.IsAny<AddDirectMessageRequest>()))
                .Throws(new Exception(Error));

            var res = await _service.AddAsync(new AddDirectMessageRequest());

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var message = new DirectMessage
            {
                Read = false
            };

            _directMessageRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateDirectMessageRequest>()))
                .Callback((UpdateDirectMessageRequest request) =>
                {
                    message.Read = request.Read.Value;
                });

            var request = new UpdateDirectMessageRequest
            {
                Read = true
            };

            var res = await _service.UpdateAsync(request);

            Assert.True(res.Success);
            Assert.Equal(request.Read, message.Read);
        }

        [Fact]
        public async Task UpdateAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _directMessageRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateDirectMessageRequest>()))
                .Throws(new Exception(Error));

            var res = await _service.UpdateAsync(new UpdateDirectMessageRequest());

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteByIdAsync_Success()
        {
            const long IdToDelete = 2;

            var messages = new List<DirectMessage>
            {
                new DirectMessage { Id = 1, },
                new DirectMessage { Id = IdToDelete, },
                new DirectMessage { Id = 3, },
            };

            _directMessageRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => messages.RemoveAll(x => x.Id == id));

            var res = await _service.DeleteByIdAsync(IdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(messages, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task DeleteByIdAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _directMessageRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Throws(new Exception(Error));

            var res = await _service.DeleteByIdAsync(2137);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
