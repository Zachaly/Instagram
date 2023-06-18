namespace Instagram.Tests.Integration
{
    internal static class Constants
    {
        public const string ConnectionString = "server=localhost; database=InstagramTest; Trusted_Connection=true";
        public const string MasterConnection = "server=localhost; database=master; Trusted_Connection=true";
        public readonly static string[] TruncateQueries =
        {
            "TRUNCATE TABLE [User]",
            "TRUNCATE TABLE [Post]",
            "TRUNCATE TABLE [PostImage]",
            "TRUNCATE TABLE [UserFollow]",
            "TRUNCATE TABLE [PostLike]",
            "TRUNCATE TABLE [PostComment]",
            "TRUNCATE TABLE [PostTag]"
        };
    }
}
