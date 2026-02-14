using AutoMapper;
using CarGalary.Application.Dtos.CarModel.Command;
using CarGalary.Application.Dtos.CarModel.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CarModelService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CarModelResponseDto>> GetAllAsync()
        {
            var models = await _unitOfWork.CarModels.GetAllAsync();
            return _mapper.Map<List<CarModelResponseDto>>(models);
        }

        public async Task<CarModelResponseDto?> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.CarModels.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<CarModelResponseDto>(model);
        }

        public async Task<CarModelResponseDto> CreateAsync(CreateCarModelRequestDto dto)
        {
            var brand = await _unitOfWork.Brands.BrandExists(dto.BrandId);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            var model = _mapper.Map<CarGalary.Domain.Entities.CarModel>(dto);
            model.CreatedAt = DateTime.UtcNow;
            model.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.CarModels.CreateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarModelResponseDto>(model);
        }

        public async Task UpdateAsync(int id, UpdateCarModelRequestDto dto)
        {
            var existing = await _unitOfWork.CarModels.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CarModel not found");
            }

            var brand = await _unitOfWork.Brands.BrandExists(dto.BrandId);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                dto.ImageUrl = existing.ImageUrl;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.CarModels.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.CarModels.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("CarModel not found");
            }

            await _unitOfWork.CarModels.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
