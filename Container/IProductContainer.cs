
using amazonCloneWebAPI.Entity;
using amazonCloneWebAPI.Models;
namespace amazonCloneWebAPI.Container;

public interface IProductContainer{
    Task<List<ProductEntity>> GetAllProducts();
    Task<ProductEntity>GetByProductId(int code);
    Task<bool> DeleteProduct(int code);
    Task<bool> SaveProduct(ProductEntity _product);

}