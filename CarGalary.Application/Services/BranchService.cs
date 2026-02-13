
using AutoMapper;
using CarGalary.Application.Dtos.Branch;
using CarGalary.Application.Dtos.Branch.Command;
using CarGalary.Application.Interfaces;
using CarGalary.Application.Validations.Branch;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._currentUserService = currentUserService;
        }

        public async Task<bool> ActiveAsync(UpdateBranchWorkingDayRequestDto updateBranchWorkingDayRequestDto)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(updateBranchWorkingDayRequestDto.Id);
            if (branch == null)
                throw new Exception("Branch not found");
            branch.IsAvailable = updateBranchWorkingDayRequestDto.IsAvailable;
            foreach (var wd in branch.BranchWorkingDays)
            {
                wd.IsAvailable = updateBranchWorkingDayRequestDto.IsAvailable;
            }
            

            await _unitOfWork.Branches.ActiveAsync(branch);
            await _unitOfWork.SaveChangesAsync();
            return true;


        }

        public async Task<BranchResponseDto> CreateAsync(CreateBrancRequestDto createBrancRequestDto)
        {
            // 1️⃣ Map DTO → Entity
            var branchEntity = _mapper.Map<Branchs>(createBrancRequestDto);
            // 2️⃣ Save to DB
            branchEntity.CreatedAt = DateTime.UtcNow;
            var userName = _currentUserService.UserName;
            branchEntity.CreatedBy = userName;
            //
            await _unitOfWork.Branches.CreateAsync(branchEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<BranchResponseDto>(branchEntity);
            return response;
        }



        public async Task<IEnumerable<BranchResponseDto>> GetAllAsync()
        {
            var branches = await _unitOfWork.Branches.GetAllAsync();
            var branchResponseDto = _mapper.Map<IEnumerable<BranchResponseDto>>(branches);
            return branchResponseDto;
        }
    }
}