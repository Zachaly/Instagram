using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class PostReportRepositoryTests : RepositoryTest
    {
        private readonly PostReportRepository _repository;

        public PostReportRepositoryTests() : base()
        {
            _repository = new PostReportRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task UpdateByPostIdAsync_UpdatesCorrectReports()
        {
            Insert("User", FakeDataFactory.GenerateUsers(10));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]");

            const long PostId = 1;

            Insert("PostReport", FakeDataFactory.GeneratePostReports(PostId, userIds));
            Insert("PostReport", FakeDataFactory.GeneratePostReports(2, userIds));

            var request = new UpdatePostReportRequest { Accepted = true, Resolved = true };

            await _repository.UpdateByPostIdAsync(request, PostId);

            var reports = GetFromDatabase<PostReport>("SELECT * FROM [PostReport]");

            Assert.All(reports.Where(x => x.PostId == PostId), report =>
            {
                Assert.Equal(request.Accepted, report.Accepted);
                Assert.Equal(request.Resolved, report.Resolved);
            });
            Assert.All(reports.Where(x => x.PostId != PostId), report =>
            {
                Assert.Null(report.Accepted);
                Assert.False(report.Resolved);
            });
        }

        [Fact]
        public async Task UpdateByIdAsync_UpdatesCorrectReport()
        {
            Insert("User", FakeDataFactory.GenerateUsers(10));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]");

            Insert("PostReport", FakeDataFactory.GeneratePostReports(1, userIds));
            Insert("PostReport", FakeDataFactory.GeneratePostReports(2, userIds));

            var reportId = GetFromDatabase<long>("SELECT Id FROM PostReport").First();

            var request = new UpdatePostReportRequest { Accepted = true, Resolved = true };

            await _repository.UpdateByIdAsync(request, reportId);

            var reports = GetFromDatabase<PostReport>("SELECT * FROM [PostReport]");

            Assert.Contains(reports, x => x.Id == reportId && x.Accepted == request.Accepted && x.Resolved == request.Resolved);
            Assert.All(reports.Where(x => x.Id != reportId), report =>
            {
                Assert.Null(report.Accepted);
                Assert.False(report.Resolved);
            });
        }
    }
}
