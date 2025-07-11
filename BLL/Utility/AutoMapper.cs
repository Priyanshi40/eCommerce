using AutoMapper;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Utility;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Address, AddressViewModel>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserNavigation.Firstname))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserNavigation.Lastname))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.State.Name));
        // .ReverseMap();

        CreateMap<AddressViewModel, Address>()
        .ForMember(dest => dest.UserNavigation, opt => opt.Ignore())
        .ForMember(dest => dest.Country, opt => opt.Ignore())
        .ForMember(dest => dest.City, opt => opt.Ignore())
        .ForMember(dest => dest.State, opt => opt.Ignore());

        // CreateMap<Product, ProductViewModel>()
        //     .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl)))
        //     .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            // .ForMember(dest => dest.WishlistProductIds, opt => opt.Ignore());

        // CreateMap<ProductViewModel,Product>()
        //     .ForMember(dest => dest.Images, opt => opt.Ignore())
        //     .ForMember(dest => dest.WishList, opt => opt.Ignore())
        //     .ForMember(dest => dest.Category, opt => opt.Ignore());


        CreateMap<Category, CategoryViewModel>();
        CreateMap<CategoryViewModel, Category>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<VendorDetails, VendorDetailsViewModel>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserNavigation.IUser.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.UserNavigation.IUser.PhoneNumber))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.UserNavigation.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.UserNavigation.Lastname));
        CreateMap<VendorDetailsViewModel, VendorDetails>();
        CreateMap<VendorViewModel, VendorDetails>();
    }
}
