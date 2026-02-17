using AutoMapper;
using CarGalary.Application.Dtos.Offer.Command;
using CarGalary.Application.Dtos.Offer.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
        public OfferService(IUnitOfWork unitOfWork, IMapper mapper){_unitOfWork=unitOfWork;_mapper=mapper;}
        public async Task<List<OfferResponseDto>> GetAllAsync(){var i=await _unitOfWork.Offers.GetAllAsync(); return _mapper.Map<List<OfferResponseDto>>(i);} 
        public async Task<OfferResponseDto?> GetByIdAsync(int id){var i=await _unitOfWork.Offers.GetByIdAsync(id); return i==null?null:_mapper.Map<OfferResponseDto>(i);} 
        public async Task<OfferResponseDto> CreateAsync(CreateOfferRequestDto dto){var e=_mapper.Map<Offer>(dto); e.CreatedAt=DateTime.UtcNow; await _unitOfWork.Offers.CreateAsync(e); await _unitOfWork.SaveChangesAsync(); return _mapper.Map<OfferResponseDto>(e);} 
        public async Task UpdateAsync(int id, UpdateOfferRequestDto dto){var e=await _unitOfWork.Offers.GetByIdAsync(id); if(e==null) throw new Exception("Offer not found"); if(dto.IsAvailable==null) dto.IsAvailable=e.IsAvailable; _mapper.Map(dto,e); await _unitOfWork.Offers.UpdateAsync(e); await _unitOfWork.SaveChangesAsync(); }
        public async Task DeleteAsync(int id){var e=await _unitOfWork.Offers.GetByIdAsync(id); if(e==null) throw new Exception("Offer not found"); await _unitOfWork.Offers.DeleteAsync(e); await _unitOfWork.SaveChangesAsync(); }
    }
}
