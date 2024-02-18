using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using OnlyTodo.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace OnlyTodo.Tests;

public class TaskServiceTests : IClassFixture<TestingWebApplicationFactory<Program>>
{
    private readonly TestingWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TaskServiceTests(TestingWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private static async Task<string> GetResponseJsonString(HttpResponseMessage response)
    {
        bool hasJsonHeader = "application/json; charset=utf-8" == response.Content.Headers.ContentType?.ToString();
        if (!hasJsonHeader)
        {
            throw new Exception("Response type is not Json");
        }

        return await response.Content.ReadAsStringAsync();
    }

    private static async Task<bool> ResponseContainsNonEmptyJsonObject(HttpResponseMessage response)
    {
        return JObject.Parse(await GetResponseJsonString(response)).HasValues;
    }

    private async Task<TodoTask> AddSampleTodoAsync()
    {
        TodoTaskDTO task = new()
        {
            Title = "Task Title",
            Completed = false,
        };

        HttpResponseMessage postResponse = await _client.PostAsJsonAsync("tasks", task);
        TodoTask? createdTask = await postResponse.Content.ReadFromJsonAsync<TodoTask>()
            ?? throw new Exception("Error creating sample task");

        return createdTask;
    }

    [Fact]
    public async void GetAll_ResponseOK()
    {
        await AddSampleTodoAsync();

        HttpResponseMessage response = await _client.GetAsync("tasks");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async void Post_WithValidData_SucceedsAndReturnsValidResponse()
    {
        // Arrange
        TodoTaskDTO task = new()
        {
            Title = "Task Title",
            Completed = false,
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("tasks", task);
        HttpResponseMessage createdAtLocationResponse = await _client.GetAsync(response.Headers.Location);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.True(await ResponseContainsNonEmptyJsonObject(response));

        Assert.NotEqual(HttpStatusCode.NotFound, createdAtLocationResponse.StatusCode);
        Assert.True(await ResponseContainsNonEmptyJsonObject(createdAtLocationResponse));
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
        HttpResponseMessage response = await _client.PostAsJsonAsync("tasks", invalidTask);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async void Post_WithId_IgnoresId()
    {
        TodoTaskDTO task = new()
        {
            Id = Guid.NewGuid(),
            Title = "Task Title",
            Completed = false,
        };

        HttpResponseMessage postResponse = await _client.PostAsJsonAsync("tasks", task);
        TodoTask? createdTask = await postResponse.Content.ReadFromJsonAsync<TodoTask>();

        Assert.NotEqual(task.Id, createdTask?.Id);
    }

    [Fact]
    public async void Get_WithInvalidId_ReturnsNotFound()
    {
        string id = Guid.NewGuid().ToString();

        HttpResponseMessage response = await _client.GetAsync($"tasks/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void Get_WithValidId_ReturnsValidResponseWithData()
    {
        TodoTask task = await AddSampleTodoAsync();

        // Act
        HttpResponseMessage response = await _client.GetAsync($"tasks/{task.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(await ResponseContainsNonEmptyJsonObject(response));
    }

    [Fact]
    public async void Delete_WithValidId_ReturnsNoContentAndRemoveObject()
    {
        TodoTask task = await AddSampleTodoAsync();

        HttpResponseMessage deleteResponse = await _client.DeleteAsync($"tasks/{task.Id}");
        HttpResponseMessage getResponse = await _client.GetAsync($"tasks/{task.Id}");

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

        HttpResponseMessage response = await _client.PatchAsJsonAsync($"tasks/{id}", new object());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void Patch_WithValidInput_ReturnsSuccessWithData()
    {
        TodoTask task = await AddSampleTodoAsync();
        TodoTaskDTO updateTask = new()
        {
            Id = task.Id,
            Title = "New Title",
        };

        HttpResponseMessage response = await _client.PatchAsJsonAsync($"tasks/{task.Id}", task);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(updateTask.Title, JObject.Parse(await GetResponseJsonString(response)).GetValue("title"));
    }
}
