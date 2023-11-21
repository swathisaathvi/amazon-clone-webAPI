using amazonCloneWebAPI.Controllers;
using amazonCloneWebAPI.Handler;
using amazonCloneWebAPI.Models;
using amazonCloneWebAPI.Models.Entity;
using amazonCloneWebAPI.ProudctServices;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestAmazonCloneWebAPI.Controllers;

public class TestProductContoller
{
    private IMapper _mapper;
    public TestProductContoller()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperHandler());
        });
        _mapper = mappingConfig.CreateMapper();
    }
    [Fact]
    public async Task GetAllProductsFromDB()
    {
        List<ProductEntity> products = _mapper.Map<List<Product>, List<ProductEntity>>
            (MockData.ProductsMockData.ProductCollection());
        var productServiceMock = new Mock<IProductServices>();
        productServiceMock.Setup(x => x.GetAllProducts()).ReturnsAsync(products);
        var loggerMock = new Mock<ILogger<ProductServices>>();

        var productController = new ProductController(productServiceMock.Object, loggerMock.Object);

        var result = await productController.GetAllProducts() as OkObjectResult;
        var returnedProducts = result?.Value as List<ProductEntity>;
        Assert.NotNull(returnedProducts);
        Assert.Equal(returnedProducts?.Count, products.Count);
    }

    [Fact]
    public async Task GetProductByProductID()
    {
        ProductEntity product = _mapper.Map<Product, ProductEntity>
            (MockData.ProductsMockData.ProductCollection()[1]);
        var productServiceMock = new Mock<IProductServices>();
        productServiceMock.Setup(x => x.GetByProductId(2)).ReturnsAsync(product);
        var loggerMock = new Mock<ILogger<ProductServices>>();

        var productController = new ProductController(productServiceMock.Object, loggerMock.Object);

        var result = await productController.GetByProductId(2) as OkObjectResult;
        var returnedProduct = result?.Value as ProductEntity;
        Assert.NotNull(returnedProduct);
        Assert.Equal("Kitchen",returnedProduct?.Category);
    }

    [Fact]
    public async Task DeleteProductByProductID()
    {
        var productServiceMock = new Mock<IProductServices>();
        productServiceMock.Setup(x => x.DeleteProduct(2));
        var loggerMock = new Mock<ILogger<ProductServices>>();

        var productController = new ProductController(productServiceMock.Object, loggerMock.Object);

        var result = await productController.DeleteProduct(2);
        productServiceMock.Verify(x => x.DeleteProduct(2), Times.Exactly(1));
    }

    [Fact]
    public async Task AddProduct()
    {
        ProductEntity product = _mapper.Map<Product, ProductEntity>
            (MockData.ProductsMockData.ProductCollection()[1]);
        var productServiceMock = new Mock<IProductServices>();
        productServiceMock.Setup(x => x.SaveProduct(product));
        var loggerMock = new Mock<ILogger<ProductServices>>();

        var productController = new ProductController(productServiceMock.Object, loggerMock.Object);

        var result = await productController.SaveProduct(product);
        productServiceMock.Verify(x => x.SaveProduct(product), Times.Exactly(1));
    }

}
