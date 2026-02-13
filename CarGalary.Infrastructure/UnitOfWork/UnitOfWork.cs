
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
        public ICarColorRepository CarColors { get; }
        public ICarFeatureRepository CarFeatures { get; }
        public ICarGalleryImageRepository CarGalleryImages { get; }
        public ICarRepository Cars { get; }
        public ICarTypeRepository CarTypes { get; }
        public ICompanyInformationRepository CompanyInformations { get; }
        public IContactSalesOfficerRepository ContactSalesOfficers { get; }
        public IContactUsRepository ContactUs { get; }
        public IFAQRepository FAQs { get; }
        public IFavoritesRepository Favorites { get; }
        public IMemberServiceRepository MemberServices { get; }
        public IOfferRepository Offers { get; }
        public IServicesRepository Services { get; }
        public IServiceTypeRepository ServiceTypes { get; }
        public IUserProfileRepository UserProfiles { get; }

        public IIdentityRepository identities {get;}

        public UnitOfWork(
            ApplicationDbContext context,
            IBrandRepository brandRepository,
            IBranchRepository branchRepository,
            IIdentityRepository identityRepository,
            
            ICarColorRepository carColorRepository,
             ICarFeatureRepository carFeatureRepository,
            ICarGalleryImageRepository carGalleryImageRepository,
            ICarRepository carRepository,
            ICarTypeRepository carTypeRepository,
            ICompanyInformationRepository companyInformationRepository,
            IContactSalesOfficerRepository contactSalesOfficerRepository,
            IContactUsRepository contactUsRepository,
            IFAQRepository fAQRepository,
            IFavoritesRepository favoritesRepository,
            IMemberServiceRepository memberServiceRepository,
            IOfferRepository offerRepository,
            IServicesRepository servicesRepository,
             IServiceTypeRepository serviceTypeRepository,
             IUserProfileRepository userProfileRepository)
        {
            _context = context;
            CarColors = carColorRepository;
            CarFeatures = carFeatureRepository;
            CarGalleryImages = carGalleryImageRepository;
            Brands = brandRepository;
            Branches = branchRepository;
            Cars = carRepository;
            CarTypes=carTypeRepository;
            CompanyInformations=companyInformationRepository;
            ContactSalesOfficers=contactSalesOfficerRepository;
            ContactUs=contactUsRepository;
            FAQs=fAQRepository;
            Favorites=favoritesRepository;
            Offers= offerRepository;
            Services=servicesRepository;
            ServiceTypes=serviceTypeRepository;
            MemberServices=memberServiceRepository;
            UserProfiles=userProfileRepository;
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