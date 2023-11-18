using amazonCloneWebAPI.Container;
using amazonCloneWebAPI.Entity;
using amazonCloneWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace amazonCloneWebAPI.Controllers;

// [Authorize(Roles="Admin")]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private readonly IProductContainer _DBContext;

    public ProductController(IProductContainer DBContext)
    {
        this._DBContext = DBContext;
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        Log.Information("Customer Getall triggered");
        var product = await _DBContext.GetAllProducts();
        return Ok(product);
    }

    [HttpGet("GetByCode/{productId}")]
    public async Task<IActionResult> GetByProductId(int productId){
        var product = await _DBContext.GetByProductId(productId);
        return Ok(product);
    }

    [HttpDelete("DeleteProduct/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId){
        var product = await _DBContext.DeleteProduct(productId);
        return Ok(true);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> SaveProduct([FromBody] ProductEntity product){
        var productDetails = await _DBContext.SaveProduct(product);
        return Ok(true);
    }

}
