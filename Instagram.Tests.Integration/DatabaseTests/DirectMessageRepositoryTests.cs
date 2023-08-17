using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class DirectMessageRepositoryTests : RepositoryTest
    {
        private readonly DirectMessageRepository _repository;

        public DirectMessageRepositoryTests() : base()
        {
            _repository = new DirectMessageRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperEntities()
        {
            Insert("DirectMessage", FakeDataFactory.GenerateDirectMessages(1, 2, 3));

            var message = GetFromDatabase<DirectMessage>("SELECT * FROM DirectMessage").Last();

            var request = new UpdateDirectMessageRequest { Id = message.Id, Read = true };

            await _repository.UpdateAsync(request);

            var updatedMessage = GetFromDatabase<DirectMessage>("SELECT * FROM DirectMessage WHERE Id=@Id", new { Id = message.Id }).First();

            Assert.Equal(message.Id, updatedMessage.Id);
            Assert.Equal(message.Created, updatedMessage.Created);
            Assert.Equal(message.SenderId, updatedMessage.SenderId);
            Assert.Equal(message.ReceiverId, updatedMessage.ReceiverId);
            Assert.Equal(message.Content, updatedMessage.Content);
            Assert.Equal(request.Read, updatedMessage.Read);
        }
    }
}
