using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SolarWatch.Controllers;
using NUnit.Framework;
using SolarWatch.DTOs;
using Moq.Protected;
using System.Net;
using SolarWatch.Repositories;

namespace SolarWatchTest;

public class SunDataControllerTests
{
    private SunDataController _controller = null!;
    private Mock<IConfiguration> _mockConfig = null!;
    private Mock<IHttpClientFactory> _mockHttpClientFactory = null!;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;
    private Mock<ISunDataRepository> _mockSunDataRepository = null!;
    private Mock<ICityRepository> _mockCityRepository = null!;

    [SetUp]
    public void Setup()
    {
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["OPENWEATHERMAP_API"]).Returns("fake_api_key");

        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockSunDataRepository = new Mock<ISunDataRepository>();
        _mockCityRepository = new Mock<ICityRepository>();

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _controller = new SunDataController(_mockConfig.Object, _mockHttpClientFactory.Object, _mockSunDataRepository.Object, _mockCityRepository.Object);
    }

    [Test]
    public async Task Get_WithInvalidDateFormat_ReturnsBadRequest()
    {
        const string invalidDate = "2025-13-45";
        var result = await _controller.Get("Bergen", invalidDate);

        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = (ObjectResult)result;
        Assert.That(objectResult.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Get_Request()
    {
        // Mock OpenWeather API response
        var openWeatherResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"[{""name"":""Bergen"",""country"":""NO"",""lat"":60.3913,""lon"":5.3221}]")
        };

        // Mock sunrise-sunset API response
        var sunDataResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""results"":{""sunrise"":""7:40:19 AM"",""sunset"":""3:24:16 PM""}}")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains("openweathermap")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(openWeatherResponse);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains("sunrise-sunset")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(sunDataResponse);

        const string validDate = "2025-12-24";
        var result = await _controller.Get("Bergen", validDate);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.StatusCode, Is.EqualTo(200));

        Assert.That(okResult.Value, Is.TypeOf<SunDataDTO>());
        var sunData = (SunDataDTO)okResult.Value;
        Assert.That(sunData.Sunrise, Is.EqualTo("7:40:19 AM"));
        Assert.That(sunData.Sunset, Is.EqualTo("3:24:16 PM"));
    }
}