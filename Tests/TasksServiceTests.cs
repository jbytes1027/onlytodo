using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using TodoTask = OnlyTodo.Models.Task;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using OnlyTodo.Models;
using Npgsql.Internal.TypeHandlers.FullTextSearchHandlers;

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

    private async Task<TaskSchema> AddSampleTodo()
    {
        TodoTask task = new()
        {
            Title = "Task Title",
            Completed = false,
        };

        var postResponse = await _client.PostAsJsonAsync("tasks", task);
        var createdTask = await postResponse.Content.ReadFromJsonAsync<TaskSchema>();

        if (createdTask is null) throw new Exception("Error creating sample task.");

        return createdTask;
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
        TodoTask task = new()
        {
            Title = "Task Title",
            Completed = false,
        };

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
    public async void Post_WithId_IgnoresId()
    {
        TodoTask task = new()
        {
            Id = Guid.NewGuid(),
            Title = "Task Title",
            Completed = false,
        };

        var postResponse = await _client.PostAsJsonAsync("tasks", task);
        var createdTask = await postResponse.Content.ReadFromJsonAsync<TodoTask>();

        Assert.NotEqual(task.Id, createdTask?.Id);
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
        var task = await AddSampleTodo();

        // Act
        var response = await _client.GetAsync($"tasks/{task.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(await responseContainsNonEmptyJsonObject(response));
    }

    [Fact]
    public async void Delete_WithValidId_ReturnsNoContentAndRemoveObject()
    {
        var task = await AddSampleTodo();

        var deleteResponse = await _client.DeleteAsync($"tasks/{task.Id}");
        var getResponse = await _client.GetAsync($"tasks/{task.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async void Delete_WithInvalidId_Fails()
    {
        string id = Guid.NewGuid().ToString();

        var response = await _client.DeleteAsync($"tasks/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void Patch_WithInvalidId_Fails()
    {
        string id = Guid.NewGuid().ToString();

        var response = await _client.PatchAsJsonAsync($"tasks/{id}", new object());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void Patch_WithValidInput_ReturnsSuccessWithData()
    {
        var task = await AddSampleTodo();
        TodoTask updateTask = new()
        {
            Id = task.Id,
            Title = "New Title",
        };

        var response = await _client.PatchAsJsonAsync($"tasks/{task.Id}", task);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(updateTask.Title, JObject.Parse(await getResponseJsonString(response)).GetValue("title"));
    }
}