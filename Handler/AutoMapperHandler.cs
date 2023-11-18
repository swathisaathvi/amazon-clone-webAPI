using AutoMapper;
using amazonCloneWebAPI.Entity;
using amazonCloneWebAPI.Models;

// namespace amazonCloneWebAPI.Handler;

public class AutoMapperHandler: Profile {
    public AutoMapperHandler(){
        CreateMap<Product, ProductEntity>().ForMember(item=>item.ProductPrice, opt=> opt.MapFrom(item=>item.Price))
        .ForMember(item=>item.Status,opt=>opt.MapFrom(item=> item.Price > 10 ? "High" : "Low"))
        .ReverseMap();
    }
    
}