﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Data;
using Xunit;

namespace TimeTracker.Tests.IntegrationTests
{
    public class UsersApiTests : IDisposable
    {
        private const string SkipTestReason =
            "No integration tests, workaround can be created with using SQLite in memory provider, but default migrations won't work.";

        private readonly HttpClient _client;
        private readonly string _nonAdminToken;
        private readonly string _adminToken;

        public UsersApiTests()
        {
            const string issuer = "http://localhost:44387";
            const string key = "some-long-secret-key";

            //// Must initialize and open Sqlite connection in order to keep in-memory database tables
            //_connection = new SqliteConnection("DataSource=:memory:");
            //_connection.Open();

            //Startup.ConfigureDbContext = (configuration, builder) => builder.UseSqlite(_connection);

            var server = new TestServer(new WebHostBuilder()
                .UseSetting("Tokens:Issuer", issuer)
                .UseSetting("Tokens:Key", key)
                .UseStartup<Startup>()
                .UseUrls("https://localhost:44387"))
            {
                BaseAddress = new Uri("https://localhost:44387")
            };

            // Force creation of InMemory database
            var dbContext = server.Services.GetService<TimeTrackerDbContext>();
            dbContext.Database.EnsureCreated();

            _client = server.CreateClient();

            _nonAdminToken = JwtTokenGenerator.Generate(
                "aspnetcore-workshop-demo", false, issuer, key);
            _adminToken = JwtTokenGenerator.Generate(
                "aspnetcore-workshop-demo", true, issuer, key);
        }

        public void Dispose()
        {
            //_connection.Dispose();
        }

        [Fact(Skip = SkipTestReason)]
        public async Task Delete_NoAuthorizationHeader_ReturnsUnauthorized()
        {
            _client.DefaultRequestHeaders.Clear();
            var result = await _client.DeleteAsync("/api/v2/users/1");

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact(Skip = SkipTestReason)]
        public async Task Delete_NotAdmin_ReturnsForbidden()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_nonAdminToken}" });

            var result = await _client.DeleteAsync("/api/v2/users/1");

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact(Skip = SkipTestReason)]
        public async Task Delete_NoId_ReturnsMethodNotAllowed()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_adminToken}" });

            var result = await _client.DeleteAsync("/api/v2/users/ ");

            Assert.Equal(HttpStatusCode.MethodNotAllowed, result.StatusCode);
        }

        [Fact(Skip = SkipTestReason)]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_adminToken}" });

            var result = await _client.DeleteAsync("/api/v2/users/0");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact(Skip = SkipTestReason)]
        public async Task Delete_ExistingId_ReturnsOk()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_adminToken}" });

            var result = await _client.DeleteAsync("/api/v2/users/1");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
