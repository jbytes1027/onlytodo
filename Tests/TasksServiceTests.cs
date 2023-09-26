using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using TodoTask = OnlyTodo.Models.Task;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace OnlyTodo.Tests;

public class TaskServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _factory;
    HttpClient _client;

    public TaskServiceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task<string> getResponseJsonString(HttpResponseMessage response)
    {
        bool hasJsonHeader = "application/json; charset=utf-8" == response.Content.Headers.ContentType?.ToString();
        if (!hasJsonHeader)
        {
            throw new Exception("Response type is not Json.");
        }

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<bool> responseContainsJsonArray(HttpResponseMessage response)
    {
        JArray.Parse(await getResponseJsonString(response));
        return true;
    }

    private async Task<bool> responseContainsNonEmptyJsonObject(HttpResponseMessage response)
    {
        return JObject.Parse(await getResponseJsonString(response)).HasValues;
    }

    private async Task<TodoTask> AddSampleTodo()
    {
        TodoTask task = new(Guid.NewGuid(), "Task Title", false);

        await _client.PostAsJsonAsync("tasks", task);

        return task;
    }

    [Fact]
    public async void GetAll_ResponseOK()
    {
        await AddSampleTodo();

        var response = await _client.GetAsync("tasks");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async void Post_WithValidData_SucceedsAndReturnsValidResponse()
    {
        TodoTask task = new(Guid.NewGuid(), "Task Title", false);

        // Act
        var response = await _client.PostAsJsonAsync("tasks", task);
        var createdAtLocationResponse = await _client.GetAsync(response.Headers.Location);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.True(await responseContainsNonEmptyJsonObject(response));

        Assert.NotEqual(HttpStatusCode.NotFound, createdAtLocationResponse.StatusCode);
        Assert.True(await responseContainsNonEmptyJsonObject(createdAtLocationResponse));
    }

    [Fact]
    public async void Post_WithInvalidData_ReturnsFailure()
    {
        string invalidTask = @"{
                ""id"": 1,
                ""title"": ""test"",
                ""completed"": 3
            }";

        // Act
        var response = await _client.PostAsJsonAsync("tasks", invalidTask);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async void Post_Duplicate_ReturnsFailure()
    {
        TodoTask task = new(Guid.NewGuid(), "Task Title", false);

        // Act
        await _client.PostAsJsonAsync("tasks", task);
        var response = await _client.PostAsJsonAsync("tasks", task);

        //Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async void Get_WithInvalidId_ReturnsNotFound()
    {
        string id = Guid.NewGuid().ToString();

        var response = await _client.GetAsync($"tasks/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void Get_WithValidId_ReturnsValidResponseWithData()
    {
        TodoTask task = await AddSampleTodo();

        // Act
        var response = await _client.GetAsync($"tasks/{task.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(await responseContainsNonEmptyJsonObject(response));
    }

    [Fact]
    public async void Delete_WithInvalidId_SucceedsWithNoContent()
    {
        // check if resource is gone
    }
}