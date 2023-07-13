using Instagram.Application;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class DirectMessageFactoryTests
    {
        private readonly DirectMessageFactory _factory;

        public DirectMessageFactoryTests()
        {
            _factory = new DirectMessageFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddDirectMessageRequest
            {
                Content = "con",
                ReceiverId = 1,
                SenderId = 2
            };

            var message = _factory.Create(request);

            Assert.Equal(request.Content, message.Content);
            Assert.Equal(request.SenderId, message.SenderId);
            Assert.Equal(request.ReceiverId, message.ReceiverId);
            Assert.NotEqual(default, message.Created);
            Assert.False(message.Read);
        }
    }
}
