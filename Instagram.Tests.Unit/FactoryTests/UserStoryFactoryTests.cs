using Instagram.Application;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class UserStoryFactoryTests
    {
        private readonly UserStoryFactory _factory;

        public UserStoryFactoryTests()
        {
            _factory = new UserStoryFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            const long UserId = 1;
            const string FileName = "file";

            var image = _factory.Create(UserId, FileName);

            Assert.Equal(FileName, image.FileName);
            Assert.Equal(UserId, image.UserId);
            Assert.NotEqual(default, image.Created);
        }
    }
}
