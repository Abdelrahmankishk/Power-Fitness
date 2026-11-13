using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;


namespace GymManagementSystem.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Member Mapping
        CreateMap<Member, MemberViewModel>()
            .ForMember(dst => dst.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .ForMember(dst => dst.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString()));

        CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
        CreateMap<Member, UpdateMemberViewModel>()
            .ForMember(dst => dst.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dst => dst.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dst => dst.City, opt => opt.MapFrom(src => src.Address.City));

        CreateMap<UpdateMemberViewModel, Member>()
            .ForMember(dst => dst.Name, opt => opt.Ignore())
            .ForMember(dst => dst.Photo, opt => opt.Ignore())
            .AfterMap((src, dst) =>
            {
                dst.Address.BuildingNumber = src.BuildingNumber;
                dst.Address.Street = src.Street;
                dst.Address.City = src.City;
            });
        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dst => dst.Address, opt => opt.MapFrom(src => new Address()
            {
                Street = src.Street,
                City = src.City,
                BuildingNumber = src.BuildingNumber
            }))
            .ForMember(dst => dst.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        #endregion

        #region Plan Mapping
        CreateMap<Plan, PLanViewModel>();

        CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();
        #endregion

        #region Trainer Mapping
        CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(dst => dst.Address, opt => opt.MapFrom(src => new Address()
            {
                Street = src.Street,
                City = src.City,
                BuildingNumber = src.BuildingNumber
            }))
            .ForMember(dst => dst.Specialtie, opt => opt.MapFrom(src => src.Specialties));

        CreateMap<Trainer, TrainerViewModel>()
            .ForMember(dst => dst.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .ForMember(dst => dst.DateofBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString()));

        CreateMap<Trainer, UpdateTrainerViewModel>()
            .ForMember(dst => dst.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dst => dst.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dst => dst.City, opt => opt.MapFrom(src => src.Address.City));

        CreateMap<UpdateTrainerViewModel,Trainer>()
            .ForMember(dst=>dst.Name,opt=>opt.Ignore())
            .AfterMap((src,dst) =>
            {
                dst.Address.BuildingNumber = src.BuildingNumber;
                dst.Address.Street = src.Street;
                dst.Address.City = src.City;
            });
        #endregion
    }
}
