
using AutoMapper;
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Dtos.Brand.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper,ICurrentUserService currentUserService )
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
            this._currentUserService = currentUserService;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await _unitOfWork.Brands.GetBrands();
            return _mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandResponseDto?> GetByIdAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetBrandById(id);
            return brand == null ? null : _mapper.Map<BrandResponseDto>(brand);
        }

      

        public async Task<BrandResponseDto> CreateAsync(CreateBrandRequestDto dto)
        {
            var brand = _mapper.Map<Brand>(dto);
            brand.CreatedAt=DateTime.UtcNow;
            brand.CreatedBy=_currentUserService.UserName;
            await _unitOfWork.Brands.CreateBrand(brand);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BrandResponseDto>(brand);
        }

        public async Task UpdateAsync(int id, UpdateBrandRequestDto dto)
        {
            var brand = await _unitOfWork.Brands.GetBrandById(id);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                dto.ImageUrl = brand.ImageUrl;
            }

            _mapper.Map(dto, brand);
            await _unitOfWork.Brands.UpdateBrand(brand);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetBrandById(id);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            await _unitOfWork.Brands.DeleteBrandById(brand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
