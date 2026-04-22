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

            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<ReservationCreateDto, Reservation>();
            CreateMap<ReservationCancelDto, Reservation>();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<RoleCreateDto, Role>();


            CreateMap<Room, RoomDto>();
            CreateMap<RoomType, RoomTypeDto>();
            CreateMap<RoomCreateDto, Room>();

            CreateMap<Facility, FacilityDto>();
            CreateMap<RoomFacility, RoomFacilityDto>();

            CreateMap<FacilityCreateDto, Facility>();
            CreateMap<FacilityUpdateDto, Facility>();

        }
    }
}
