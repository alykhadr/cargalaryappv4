
using AutoMapper;
using CarGalary.Application.Dtos.Branch;
using CarGalary.Application.Dtos.Branch.Command;
using CarGalary.Domain.Entities;


namespace CarGalary.Application.Mappings.Branch
{
    public class BranchResponseProfile : Profile
    {
        public BranchResponseProfile()
        {

            // Create → Entity
            CreateMap<CreateBrancRequestDto, Branchs>()
                .ForMember(dest => dest.BranchWorkingDays,
                    opt => opt.MapFrom(src => src.CreateBranchWorkingDaysRequestDto));

            // Update → Entity
            CreateMap<UpdateBranchRequestDto, Branchs>();

            CreateMap<CreateBranchWorkingDaysRequestDto, BranchWorkingDays>();


            //
            // Child mapping
            CreateMap<BranchWorkingDays, BranchWorkingDaysResponseDto>();

            // Parent mapping
            CreateMap<Branchs, BranchResponseDto>()
                .ForMember(dest => dest.BranchWorkingDaysResponseDtos,
                           opt => opt.MapFrom(src => src.BranchWorkingDays));
        }
    }
}