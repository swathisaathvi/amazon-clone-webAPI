using amazonCloneWebAPI.Models;
using amazonCloneWebAPI.Models.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace amazonCloneWebAPI.ProudctServices;
public class ProductServices: IProductServices
{
    private readonly AmazonCloneContext _DBContext;
    private readonly IMapper _mapper;

    public ProductServices(AmazonCloneContext dBContext, IMapper mapper){
        this._DBContext = dBContext;
        _mapper = mapper;
    }

    public async Task<List<ProductEntity>>GetAllProducts(){
        List<ProductEntity> resp = new List<ProductEntity>();
        var product =  await _DBContext.Products.ToListAsync();
        resp = _mapper.Map<List<Product>, List<ProductEntity>>(product);
        return resp;
    }

    public async Task<ProductEntity>GetByProductId(int productId){
        var product =  await _DBContext.Products.FindAsync(productId);
        if(product != null){
            ProductEntity resp = _mapper.Map<Product, ProductEntity>(product);
            return resp;
        }
        else{
            return new ProductEntity();
        }
    }

    public async Task<bool>DeleteProduct(int productId){
        var product =  await _DBContext.Products.FirstOrDefaultAsync(item => item.ProductId == productId);
        if(product != null){
           _DBContext.Products.Remove(product);
           await this._DBContext.SaveChangesAsync();
           return true;
        }
        else{
            return false;
        }

    }

    public async Task<bool> SaveProduct(ProductEntity product){
        var productDetails = this._DBContext.Products.FirstOrDefault(item => item.ProductId == product.ProductId);
        if (productDetails != null){
            productDetails.ProductName = product.ProductName;
            productDetails.Price = product.ProductPrice;
            productDetails.Category = product.Category;
            await this._DBContext.SaveChangesAsync();
        }
        else{
            Product prod = _mapper.Map<ProductEntity, Product>(product);
            this._DBContext.Products.Add(prod);
            await this._DBContext.SaveChangesAsync();
        }
        return true;
    }

}