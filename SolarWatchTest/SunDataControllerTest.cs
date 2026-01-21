using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.DTOs;
using SolarWatch.Repositories;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SunDataControllerTests
{
    private SunDataController _controller = null!;

    private Mock<ICityLocationService> _mockCityLocationService = null!;
    private Mock<ISunDataService> _mockSunDataService = null!;
    private DateService _mockDateService = null!;

    private Mock<ISunDataRepository> _mockSunDataRepository = null!;
    private Mock<ICityRepository> _mockCityRepository = null!;


    [SetUp]
    public void Setup()
    {
        _mockDateService = new DateService();
        _mockCityLocationService = new Mock<ICityLocationService>();
        _mockSunDataService = new Mock<ISunDataService>();
        
        _mockSunDataRepository = new Mock<ISunDataRepository>();
        _mockCityRepository = new Mock<ICityRepository>();

        _controller = new SunDataController(_mockDateService, _mockCityLocationService.Object,
            _mockSunDataService.Object, _mockSunDataRepository.Object, _mockCityRepository.Object);
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
        // Mock city location service response
        var cityLocationDto = new CityLocationDTO
        {
            Name = "Bergen",
            Country = "NO",
            Lat = 60.3913f,
            Lon = 5.3221f
        };
        _mockCityLocationService
            .Setup(s => s.GetCityLocation("Bergen"))
            .ReturnsAsync(cityLocationDto);

        // Mock sun data service response
        var sunDataDto = new SunDataDTO("7:40:19 AM", "3:24:16 PM");
        _mockSunDataService
            .Setup(s => s.GetSunData(60.3913f, 5.3221f, "2025-12-24"))
            .ReturnsAsync(sunDataDto);

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

    [Test]
    public async Task Get_WithNonexistentCity_ReturnsNotFound()
    {
        // Mock city location service to return null for nonexistent city
        _mockCityLocationService
            .Setup(s => s.GetCityLocation("NonexistentCity"))
            .ReturnsAsync((CityLocationDTO?)null);

        const string validDate = "2025-12-24";
        var result = await _controller.Get("NonexistentCity", validDate);
        Console.WriteLine(result);

        Assert.That(result, Is.TypeOf<ObjectResult>());
        var notFoundResult = (ObjectResult)result;
        Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
    }
}