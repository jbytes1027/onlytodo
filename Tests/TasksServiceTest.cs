using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace OnlyTodo.Tests;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _factory;

    public UnitTest1(WebApplicationFactory<Program> factory) {
        _factory = factory;
    }

    [Fact]
    public async void Test1()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("tasks");

        response.EnsureSuccessStatusCode();
        // Assert.True(true);
    }
}