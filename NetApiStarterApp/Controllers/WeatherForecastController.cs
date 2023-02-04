using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetApiStarterLibrary.Permissions;

namespace AuthSample.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HasPermission(PermissionType.EditData)]
    [Route("b")]
    [HttpGet]
    public IEnumerable<NewObj> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new NewObj
        {
            Data = index
        })
        .ToArray();
    }

    [HasPermission(PermissionType.ViewData)]
    [Route("a")]
    [HttpGet]
    public IEnumerable<NewObj> Get2()
    {
        return Enumerable.Range(1, 5).Select(index => new NewObj
        {
            Data = index
        })
        .ToArray();
    }
}

public class NewObj
{
    public int Data { get; set; }
}
