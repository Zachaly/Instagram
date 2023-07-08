using Instagram.Application.Command;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class PostReportControllerTests : ApiTest
    {
        const string Endpoint = "/api/post-report";

        [Fact]
        public async Task GetAsync_ReturnsReports()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            Insert("PostReport", FakeDataFactory.GeneratePostReports(1, users.Select(x => x.Id)));
            Insert("PostReport", FakeDataFactory.GeneratePostReports(2, users.Select(x => x.Id)));

            var reportIds = GetFromDatabase<long>("SELECT Id FROM PostReport");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostReportModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(reportIds, content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_Success_ReturnsCorrectReport()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            Insert("PostReport", FakeDataFactory.GeneratePostReports(1, users.Select(x => x.Id)));
            Insert("PostReport", FakeDataFactory.GeneratePostReports(2, users.Select(x => x.Id)));

            var report = GetFromDatabase<PostReport>("SELECT * FROM PostReport").First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{report.Id}");
            var content = await response.Content.ReadFromJsonAsync<PostReportModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(report.Id, content.Id);
            Assert.Equal(report.Reason, content.Reason);
            Assert.Equal(report.Accepted, content.Accepted);
            Assert.Equal(report.ResolveTime, content.ResolveTime);
            Assert.Equal(report.PostId, content.PostId);
            Assert.Equal(report.ReportingUserId, content.ReportingUserId);
        }

        [Fact]
        public async Task GetByIdAsync_Failure_ReportNotFound()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(5));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]");

            var reports = FakeDataFactory.GeneratePostReports(1, userIds);
            reports.AddRange(FakeDataFactory.GeneratePostReports(2, userIds));

            Insert("PostReport", reports);

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(reports.Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsReport()
        {
            await Authorize();

            var request = new AddPostReportRequest
            {
                PostId = 1,
                ReportingUserId = _authorizedUserId,
                Reason = "min 1 letter reason"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var reports = GetFromDatabase<PostReport>("SELECT * FROM PostReport");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(reports, x => x.ReportingUserId == request.ReportingUserId 
                && x.PostId == request.PostId 
                && x.Reason == request.Reason);
        }

        [Fact]
        public async Task ResolveAsync_ReportNotAccepted_SinglePostReportResolvedAndNotAccepted()
        {
            await AuthorizeModerator();

            const long PostId = 1;
            var userIds = new long[] { 1, 2, 3 };

            Insert("PostReport", FakeDataFactory.GeneratePostReports(PostId, userIds));
            Insert("PostReport", FakeDataFactory.GeneratePostReports(2, userIds));

            var reportId = GetFromDatabase<long>("SELECT Id FROM PostReport WHERE PostId=@PostId", new { PostId }).First();

            var command = new ResolvePostReportCommand
            {
                Accepted = false,
                Id = reportId,
                PostId = PostId
            };

            var response = await _httpClient.PutAsJsonAsync($"{Endpoint}/resolve", command);

            var reports = GetFromDatabase<PostReport>("SELECT * FROM PostReport");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.All(reports.Where(x => x.Id != command.Id), report =>
            {
                Assert.Null(report.Accepted);
                Assert.False(report.Resolved);
                Assert.Null(report.ResolveTime);
            });
            Assert.All(reports.Where(x => x.Id == command.Id), report =>
            {
                Assert.Equal(command.Accepted, report.Accepted);
                Assert.True(report.Resolved);
                Assert.NotNull(report.ResolveTime);
            });
        }
    }
}
