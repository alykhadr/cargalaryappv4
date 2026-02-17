using AutoMapper;
using CarGalary.Application.Dtos.MemberService.Command;
using CarGalary.Application.Dtos.MemberService.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class MemberServiceService : IMemberServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MemberServiceService(IUnitOfWork unitOfWork, IMapper mapper){_unitOfWork=unitOfWork;_mapper=mapper;}
        public async Task<List<MemberServiceResponseDto>> GetAllAsync(){var i=await _unitOfWork.MemberServices.GetAllAsync();return _mapper.Map<List<MemberServiceResponseDto>>(i);} 
        public async Task<MemberServiceResponseDto?> GetByIdAsync(int id){var i=await _unitOfWork.MemberServices.GetByIdAsync(id);return i==null?null:_mapper.Map<MemberServiceResponseDto>(i);} 
        public async Task<MemberServiceResponseDto> CreateAsync(CreateMemberServiceRequestDto dto){var e=_mapper.Map<MemberService>(dto);e.CreatedAt=DateTime.UtcNow; await _unitOfWork.MemberServices.CreateAsync(e); await _unitOfWork.SaveChangesAsync(); return _mapper.Map<MemberServiceResponseDto>(e);} 
        public async Task UpdateAsync(int id, UpdateMemberServiceRequestDto dto){var e=await _unitOfWork.MemberServices.GetByIdAsync(id); if(e==null) throw new Exception("MemberService not found"); if(dto.IsAvailable==null) dto.IsAvailable=e.IsAvailable; _mapper.Map(dto,e); await _unitOfWork.MemberServices.UpdateAsync(e); await _unitOfWork.SaveChangesAsync();}
        public async Task DeleteAsync(int id){var e=await _unitOfWork.MemberServices.GetByIdAsync(id); if(e==null) throw new Exception("MemberService not found"); await _unitOfWork.MemberServices.DeleteAsync(e); await _unitOfWork.SaveChangesAsync();}
    }
}
