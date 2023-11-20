using amazonCloneWebAPI.ProudctServices;
using amazonCloneWebAPI.Models;
using amazonCloneWebAPI.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace amazonCloneWebAPI.Controllers;

[Authorize(Policy = "DevelopmentPolicy")]
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

    [Authorize(Policy = "AdminToCreateProduct")]
    [HttpPost("SaveProduct")]
    public async Task<IActionResult> SaveProduct([FromBody] ProductEntity product){
        await _container.SaveProduct(product);
        return Ok(true);
    }

}
