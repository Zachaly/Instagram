using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class DirectMessageServiceTests
    {
        private readonly IDirectMessageRepository _directMessageRepository;
        private readonly IDirectMessageFactory _directMessageFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly DirectMessageService _service;

        public DirectMessageServiceTests()
        {
            _directMessageRepository = Substitute.For<IDirectMessageRepository>();
            _directMessageFactory = Substitute.For<IDirectMessageFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new DirectMessageService(_directMessageRepository, _directMessageFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var messages = new List<DirectMessage>();

            const long NewId = 1;
            _directMessageRepository.InsertAsync(Arg.Any<DirectMessage>())
                .Returns(NewId)
                .AndDoes(info => messages.Add(info.Arg<DirectMessage>()));

            _directMessageFactory.Create(Arg.Any<AddDirectMessageRequest>())
                .Returns(info => new DirectMessage { Content = info.Arg<AddDirectMessageRequest>().Content });

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

            _directMessageFactory.Create(Arg.Any<AddDirectMessageRequest>()).Throws(new Exception(Error));

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

            _directMessageRepository.UpdateAsync(Arg.Any<UpdateDirectMessageRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => message.Read = info.Arg<UpdateDirectMessageRequest>().Read.Value);

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

            _directMessageRepository.UpdateAsync(Arg.Any<UpdateDirectMessageRequest>()).Throws(new Exception(Error));

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

            _directMessageRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => messages.RemoveAll(m => m.Id == info.Arg<long>()));

            var res = await _service.DeleteByIdAsync(IdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(messages, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task DeleteByIdAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _directMessageRepository.DeleteByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteByIdAsync(2137);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
