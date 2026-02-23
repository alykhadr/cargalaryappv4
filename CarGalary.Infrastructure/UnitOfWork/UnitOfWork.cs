
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Infrastructure.Context;

namespace CarGalary.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBrandRepository Brands { get; }
        public IBranchRepository Branches { get; }
        public ICarModelRepository CarModels { get; }
        public ICarColorRepository CarColors { get; }
        public ICarFeatureRepository CarFeatures { get; }
        public ICarGalleryImageRepository CarGalleryImages { get; }
        public ICarRepository Cars { get; }
        public ICarTypeRepository CarTypes { get; }
        public ICarExtraDetailsRepository  CarExtraDetails { get; }
        public ICarCarColorRepository CarCarColors { get; }
        public ICompanyInformationRepository CompanyInformations { get; }
        public IContactSalesOfficerRepository ContactSalesOfficers { get; }
        public IContactUsRepository ContactUs { get; }
        public IEaseAndComfortRepository EaseAndComforts { get; }
        public IEngineSpecificationRepository EngineSpecifications { get; }
        public IExteriorRepository Exteriors { get; }
        public IExtraFeatureRepository ExtraFeatures { get; }
        public IFAQRepository FAQs { get; }
        public IFavoritesRepository Favorites { get; }
        public IMeasurementsRepository Measurements { get; }
        public IMemberServiceRepository MemberServices { get; }
        public IOfferRepository Offers { get; }
        public ISafetyRepository Safeties { get; }
        public ISeatingRepository Seatings { get; }
        public IServicesRepository Services { get; }
        public IServiceTypeRepository ServiceTypes { get; }
        public ITransmissionRepository Transmissions { get; }


        public IIdentityRepository identities {get;}

        public UnitOfWork(
            ApplicationDbContext context,
            IBrandRepository brandRepository,
            IBranchRepository branchRepository,
            ICarModelRepository carModelRepository,
            IIdentityRepository identityRepository,
            ICarExtraDetailsRepository carExtraDetailsRepository,
            ICarColorRepository carColorRepository,
             ICarFeatureRepository carFeatureRepository,
            ICarGalleryImageRepository carGalleryImageRepository,
            ICarRepository carRepository,
            ICarTypeRepository carTypeRepository,
            ICarCarColorRepository carCarColorRepository,
            ICompanyInformationRepository companyInformationRepository,
            IContactSalesOfficerRepository contactSalesOfficerRepository,
            IContactUsRepository contactUsRepository,
            IEaseAndComfortRepository easeAndComfortRepository,
            IEngineSpecificationRepository engineSpecificationRepository,
            IExteriorRepository exteriorRepository,
            IExtraFeatureRepository extraFeatureRepository,
            IFAQRepository fAQRepository,
            IFavoritesRepository favoritesRepository,
            IMeasurementsRepository measurementsRepository,
            IMemberServiceRepository memberServiceRepository,
            IOfferRepository offerRepository,
            ISafetyRepository safetyRepository,
            ISeatingRepository seatingRepository,
            IServicesRepository servicesRepository,
             IServiceTypeRepository serviceTypeRepository,
             ITransmissionRepository transmissionRepository)
        {
            _context = context;
            CarColors = carColorRepository;
            CarFeatures = carFeatureRepository;
            CarGalleryImages = carGalleryImageRepository;
            Brands = brandRepository;
            Branches = branchRepository;
            CarModels = carModelRepository;
            Cars = carRepository;
            CarTypes=carTypeRepository;
            CarExtraDetails = carExtraDetailsRepository;
            CarCarColors = carCarColorRepository;
            CompanyInformations=companyInformationRepository;
            ContactSalesOfficers=contactSalesOfficerRepository;
            ContactUs=contactUsRepository;
            EaseAndComforts=easeAndComfortRepository;
            EngineSpecifications=engineSpecificationRepository;
            Exteriors=exteriorRepository;
            ExtraFeatures=extraFeatureRepository;
            FAQs=fAQRepository;
            Favorites=favoritesRepository;
            Measurements=measurementsRepository;
            Offers= offerRepository;
            Safeties=safetyRepository;
            Seatings=seatingRepository;
            Services=servicesRepository;
            ServiceTypes=serviceTypeRepository;
            Transmissions=transmissionRepository;
            MemberServices=memberServiceRepository;

            identities=identityRepository;

        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
