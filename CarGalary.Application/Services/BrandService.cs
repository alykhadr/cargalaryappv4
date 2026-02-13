
using AutoMapper;
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await _unitOfWork.Brands.GetBrands();
            return _mapper.Map<List<BrandDto>>(brands);
        }

        // public async Task<BrandDto?> GetByIdAsync(int id)
        // {
        //     var brand = await _repository.GetByIdAsync(id);
        //     return brand == null ? null : _mapper.Map<BrandDto>(brand);
        // }

        public async Task CreateAsync(BrandDto dto)
        {
            var brand = _mapper.Map<CarBrand>(dto);
            await _unitOfWork.Brands.CreateBrand(brand);
            await _unitOfWork.SaveChangesAsync();
        }

        // public async Task<bool> UpdateAsync(int id, BrandDto dto)
        // {
        //     var brand = await _repository.GetByIdAsync(id);
        //     if (brand == null) return false;

        //     _mapper.Map(dto, brand);
        //     await _repository.UpdateAsync(brand);
        //     return true;
        // }

        // public async Task<bool> DeleteAsync(int id)
        // {
        //     var brand = await _repository.GetByIdAsync(id);
        //     if (brand == null) return false;

        //     await _repository.DeleteAsync(brand);
        //     return true;
        // }
    }
}