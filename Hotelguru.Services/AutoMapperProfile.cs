using AutoMapper;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;

namespace Hotelguru.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            CreateMap<Role, RoleDto>().ReverseMap();



            CreateMap<Room, RoomDto>();
            CreateMap<RoomType, RoomTypeDto>();

            CreateMap<Facility, FacilityDto>();

        }
    }
}
