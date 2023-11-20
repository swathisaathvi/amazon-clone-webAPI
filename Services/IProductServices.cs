using amazonCloneWebAPI.Models;
using amazonCloneWebAPI.Models.Entity;

namespace amazonCloneWebAPI.ProudctServices;

public interface IProductServices{
    Task<List<ProductEntity>> GetAllProducts();
    Task<ProductEntity>GetByProductId(int code);
    Task<bool> DeleteProduct(int code);
    Task<bool> SaveProduct(ProductEntity _product);

}