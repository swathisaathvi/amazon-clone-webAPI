using amazonCloneWebAPI.ProudctServices;
using amazonCloneWebAPI.Models;
using amazonCloneWebAPI.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace amazonCloneWebAPI.Controllers;
[Authorize]
//[Authorize(Policy = "DevelopmentPolicy")]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private readonly IProductServices _container;
    private readonly ILogger<ProductServices> _logger;

    public ProductController(IProductServices container, ILogger<ProductServices> logger)
    {
        _container = container;
        _logger = logger;
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        _logger.LogInformation("Trying to get all the products");
        var product = await _container.GetAllProducts();
        return Ok(product);
    }

    [HttpGet("GetByCode/{productId}")]
    public async Task<IActionResult> GetByProductId(int productId){
        var product = await _container.GetByProductId(productId);
        return Ok(product);
    }

    [HttpDelete("DeleteProduct/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId){
        await _container.DeleteProduct(productId);
        return Ok(true);
    }

    //[Authorize(Policy = "AdminToCreateProduct")]
    [HttpPost("SaveProduct")]
    public async Task<IActionResult> SaveProduct([FromBody] ProductEntity product){
        await _container.SaveProduct(product);
        return Ok(true);
    }
    
    [HttpGet("/logic-engine/symbols")]
    public IActionResult GetSymbols([FromQuery] string? sort, [FromQuery] string? filter)
    {
        var symbols = new[]
        {
            new { symbolname = "DeviceCount", type = "INS", description = "Total number of devices connected", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:32:45Z" },
            new { symbolname = "ErrorCount", type = "INS", description = "Total number of system errors recorded", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:33:10Z" },
            new { symbolname = "UptimeSeconds", type = "INS", description = "Total uptime of system in seconds", category = "SystemTags", unit = "seconds", lastUpdated = "2025-11-07T10:34:25Z" },
            new { symbolname = "PowerConsumption", type = "INS", description = "Power consumption in watts", category = "SystemTags", unit = "W", lastUpdated = "2025-11-07T10:35:12Z" },
            new { symbolname = "RequestCount", type = "INS", description = "Total number of API requests handled", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:36:00Z" },
            new { symbolname = "BackupCount", type = "INS", description = "Total backups completed today", category = "LabTags", unit = "count", lastUpdated = "2025-11-07T10:38:15Z" },
            new { symbolname = "SuccessfulLogins", type = "INS", description = "Number of successful user logins today", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:36:45Z" },
            new { symbolname = "FailedLogins", type = "INS", description = "Number of failed login attempts", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:37:10Z" },
            new { symbolname = "ActiveSessions", type = "INS", description = "Number of active user sessions", category = "SystemTags", unit = "sessions", lastUpdated = "2025-11-07T10:37:40Z" },
            new { symbolname = "AlarmTriggered", type = "INS", description = "Number of alarms triggered", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:38:55Z" },
            new { symbolname = "ErrorCount", type = "INS", description = "Total number of system errors recorded", category = "WeatherTags", unit = "count", lastUpdated = "2025-11-07T10:33:10Z" },
            new { symbolname = "CPUUsage", type = "FLT", description = "Average CPU utilization percentage", category = "SystemTags", unit = "percent", lastUpdated = "2025-11-07T10:39:20Z" },
            new { symbolname = "MemoryUsage", type = "FLT", description = "System memory usage in MB", category = "SystemTags", unit = "MB", lastUpdated = "2025-11-07T10:39:45Z" },
            new { symbolname = "IsServerOnline", type = "BOOL", description = "True if the server is currently online", category = "SystemTags", unit = "boolean", lastUpdated = "2025-11-07T10:40:05Z" },
            new { symbolname = "EnvironmentMode", type = "STR", description = "Operating environment: Production, Test, or Dev", category = "SystemTags", unit = "string", lastUpdated = "2025-11-07T10:40:25Z" },
            new { symbolname = "ConnectionStatus", type = "ENUM", description = "Network state enumeration (0=Disconnected,1=Connected,2=Reconnecting)", category = "SystemTags", unit = "enum", lastUpdated = "2025-11-07T10:40:50Z" },
            new { symbolname = "DeviceCount", type = "INS", description = "Total number of devices connected", category = "EnvironmentalTags", unit = "count", lastUpdated = "2025-11-07T10:32:45Z" },
            new { symbolname = "DowntimeSeconds", type = "INS", description = "Total uptime of system in seconds", category = "SystemTags", unit = "seconds", lastUpdated = "2025-11-07T10:34:25Z" },
            new { symbolname = "FailedLogins", type = "INS", description = "Number of failed login attempts", category = "DataTags", unit = "count", lastUpdated = "2025-11-07T10:37:10Z" }

        };

        var filteredSymbols = string.IsNullOrWhiteSpace(filter)? symbols : symbols.Where(x => x.category.StartsWith(filter, StringComparison.OrdinalIgnoreCase));

        var sortedSymbols = sort.ToLower() == "asc" ? filteredSymbols.OrderBy(x => x.symbolname) : filteredSymbols.OrderByDescending(x => x.symbolname);

        return Ok(sortedSymbols);
    }

    [HttpGet("/logic-engine/symbols/{SymbolName}")]
    public IActionResult GetSymbolDetails(string symbolName)
    {

        var random = new Random();
        var now = DateTime.UtcNow;

        DateTime GetRandomTime()
        {
            var randomDays = random.Next(0, 7);
            var randomHours = random.Next(0, 24);
            var randomMinutes = random.Next(0, 60);
            return now.AddDays(-randomDays).AddHours(-randomHours).AddMinutes(-randomMinutes);
        }

        var symbolDetails = new[]
        {
            new { symbolname = "DeviceCount", type = "INS", description = "Total number of devices connected", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:32:45Z", stVal = random.Next(0, 100), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "ErrorCount", type = "INS", description = "Total number of system errors recorded", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:33:10Z", stVal = random.Next(100, 200), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "UptimeSeconds", type = "INS", description = "Total uptime of system in seconds", category = "SystemTags", unit = "seconds", lastUpdated = "2025-11-07T10:34:25Z", stVal = random.Next(300, 500), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "PowerConsumption", type = "INS", description = "Power consumption in watts", category = "SystemTags", unit = "W", lastUpdated = "2025-11-07T10:35:12Z", stVal = random.Next(500, 700), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "RequestCount", type = "INS", description = "Total number of API requests handled", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:36:00Z", stVal = random.Next(700, 1000), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "SuccessfulLogins", type = "INS", description = "Number of successful user logins today", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:36:45Z", stVal = random.Next(1000, 1300), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "FailedLogins", type = "INS", description = "Number of failed login attempts", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:37:10Z", stVal = random.Next(1300, 1600), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "ActiveSessions", type = "INS", description = "Number of active user sessions", category = "SystemTags", unit = "sessions", lastUpdated = "2025-11-07T10:37:40Z", stVal = random.Next(1700, 2000), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "BackupCount", type = "INS", description = "Total backups completed today", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:38:15Z", stVal = random.Next(2100, 2500), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            new { symbolname = "AlarmTriggered", type = "INS", description = "Number of alarms triggered", category = "SystemTags", unit = "count", lastUpdated = "2025-11-07T10:38:55Z", stVal = random.Next(2500, 2890), t = GetRandomTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
        };

        var filteredSymbol = symbolDetails.FirstOrDefault(s => s.symbolname.Equals(symbolName, StringComparison.OrdinalIgnoreCase));

        return Ok(filteredSymbol);
    }

}
