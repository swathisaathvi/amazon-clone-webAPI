using amazonCloneWebAPI.Handler;
using amazonCloneWebAPI.Models;
using AutoMapper;
using FluentAssertions;
using TestAmazonCloneWebAPI.MockData;
using amazonCloneWebAPI.ProudctServices;
using amazonCloneWebAPI.Models.Entity;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TestAmazonCloneWebAPI.Services;


public class TestProductService : IDisposable
{
    private AmazonCloneContext _dbcontext;
    private IMapper _mapper;

    public TestProductService()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperHandler());
        });
        _mapper = mappingConfig.CreateMapper();

        var options = new DbContextOptionsBuilder<AmazonCloneContext>()
       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _dbcontext = new AmazonCloneContext(options);

        _dbcontext.Database.EnsureCreated();

    }

    [Fact]
    public async Task GetAllProductsCollection()
    {
        _dbcontext.Products.AddRange(MockData.ProductsMockData.ProductCollection());
        _dbcontext.SaveChanges();

        var products = new ProductServices(_dbcontext, _mapper);
        var result = await products.GetAllProducts();
        result.Should().HaveCount(ProductsMockData.ProductCollection().Count);
    }

    [Fact]
    public async Task GetProductByProductID()
    {
        _dbcontext.Products.AddRange(MockData.ProductsMockData.ProductCollection()[0]);
        _dbcontext.SaveChanges();

        var products = new ProductServices(_dbcontext, _mapper);
        var result = await products.GetByProductId(1);
        Assert.Equal("Pen", result.ProductName);
    }

    [Fact]
    public async Task SaveProductTodB()
    {
        int prodcount = MockData.ProductsMockData.ProductCollection().Count();
        _dbcontext.Products.AddRange(MockData.ProductsMockData.ProductCollection());
        _dbcontext.SaveChanges();

        ProductEntity product = _mapper.Map<Product, ProductEntity>(MockData.ProductsMockData.SingleProduct());

        var products = new ProductServices(_dbcontext, _mapper);
        await products.SaveProduct(product);

        _dbcontext.Products.Should().HaveCount(prodcount + 1);
    }

    [Fact]
    public async Task DeleteProductByProductID()
    {
        int prodCount = MockData.ProductsMockData.ProductCollection().Count();
        _dbcontext.Products.AddRange(MockData.ProductsMockData.ProductCollection());
        _dbcontext.SaveChanges();

        var products = new ProductServices(_dbcontext, _mapper);
        var result = await products.DeleteProduct(1);
        _dbcontext.Products.Should().HaveCount(prodCount - 1);
    }

    public void Dispose()
    {
        _dbcontext.Database.EnsureDeleted();
        _dbcontext.Dispose();
    }
}
