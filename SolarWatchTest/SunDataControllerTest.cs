using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Repositories;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SunDataControllerTests
{
    private SunDataController _controller = null!;

    private IDateService _dateService = null!;
    private Mock<ISunDataRepository> _mockSunDataRepository = null!;
    private Mock<ICityRepository> _mockCityRepository = null!;


    [SetUp]
    public void Setup()
    {
        _dateService = new DateService();
        
        _mockSunDataRepository = new Mock<ISunDataRepository>();
        _mockCityRepository = new Mock<ICityRepository>();

        _controller = new SunDataController(_dateService, _mockSunDataRepository.Object, _mockCityRepository.Object);
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
        // Mock city repository response
        var city = new City { Id = 1, Name = "Bergen" };
        _mockCityRepository
            .Setup(r => r.GetByName("Bergen"))
            .ReturnsAsync(city);

        // Mock sun data repository response
        var sunData = new SunData { Id = 1, CityId = 1, Date = "2025-12-24", Sunrise = "7:40:19 AM", Sunset = "3:24:16 PM" };
        _mockSunDataRepository
            .Setup(r => r.GetByCityAndDate(city, "2025-12-24"))
            .ReturnsAsync(sunData);

        const string validDate = "2025-12-24";
        var result = await _controller.Get("Bergen", validDate);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.StatusCode, Is.EqualTo(200));

        Assert.That(okResult.Value, Is.TypeOf<SunDataDTO>());
        var sunDataDto = (SunDataDTO)okResult.Value;
        Assert.That(sunDataDto.Sunrise, Is.EqualTo("7:40:19 AM"));
        Assert.That(sunDataDto.Sunset, Is.EqualTo("3:24:16 PM"));
    }

    [Test]
    public async Task Get_WithNonexistentCity_ReturnsNotFound()
    {
        // Mock city repository to return null for nonexistent city
        _mockCityRepository
            .Setup(r => r.GetByName("NonexistentCity"))
            .ReturnsAsync((City?)null);

        const string validDate = "2025-12-24";
        var result = await _controller.Get("NonexistentCity", validDate);
        Console.WriteLine(result);

        Assert.That(result, Is.TypeOf<ObjectResult>());
        var notFoundResult = (ObjectResult)result;
        Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
    }
}