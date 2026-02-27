
using CarGalary.Domain.RepositoryInterfaces;

namespace CarGalary.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brands { get; }
        IBranchRepository Branches { get; }
        ICarModelRepository CarModels { get; }
        public IIdentityRepository identities { get; }
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
       
        public IFAQRepository FAQs { get; }
        public IFavoritesRepository Favorites { get; }
      
        public IMemberServiceRepository MemberServices { get; }
        public IOfferRepository Offers { get; }
       
        public IServicesRepository Services { get; }
        


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    }
}
