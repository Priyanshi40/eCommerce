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
                .ForMember(dest => dest.LastName,opt => opt.MapFrom(src => src.UserNavigation.Lastname))
                .ForMember(dest => dest.CountryName,opt => opt.MapFrom(src => src.Country.Name) )
                .ForMember(dest => dest.CityName,opt => opt.MapFrom(src => src.City.Name) )
                .ForMember(dest => dest.StateName,opt => opt.MapFrom(src => src.State.Name) );
        }

}
