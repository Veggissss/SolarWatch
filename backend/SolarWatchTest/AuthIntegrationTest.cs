using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SolarWatch.DTOs;

namespace SolarWatchTest;

public class AuthIntegrationTest
{
    private readonly SolarWatchWebApplicationFactory _app;
    private readonly HttpClient _client;
    private const string Hostname = "http://0.0.0.0:8080";

    public AuthIntegrationTest()
    {
        _app = new SolarWatchWebApplicationFactory();
        _client = _app.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _app.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task ProtectedEndpointWithNoAuthReturnsUnauthorized()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
        var response = await _client.GetAsync($"{Hostname}/api/sundata?city=oslo");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task ProtectedEndpointWithAuthReturnsAuthorized()
    {
        var authResponse = await _client.PostAsync($"{Hostname}/api/auth/login",
            JsonContent.Create(new LoginDTO("admin", "password")));

        if (!authResponse.IsSuccessStatusCode)
        {
            var errorContent = await authResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Login failed with status {authResponse.StatusCode}: {errorContent}");
        }

        var responseContent = await authResponse.Content.ReadAsStringAsync();
        var token = string.IsNullOrEmpty(responseContent) ? throw new Exception("Token response is empty") : responseContent.Trim('"');

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{Hostname}/api/sundata?city=oslo");
        Assert.That(response.StatusCode, Is.Not.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task LoginWithInvalidCredentialsReturnsUnauthorized()
    {
        var authResponse = await _client.PostAsync($"{Hostname}/api/auth/login",
            JsonContent.Create(new LoginDTO("admin", "wrongpassword")));

        Assert.That(authResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}