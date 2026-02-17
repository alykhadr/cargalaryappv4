

using CarGalary.Application.Interfaces;
using CarGalary.Application.Services;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Domain.UnitOfWork;
using CarGalary.Infrastructure.Identity;
using CarGalary.Infrastructure.ImplementRepositories;
using CarGalary.Infrastructure.Repositories;
using CarGalary.Infrastructure.UnitOfWork;

namespace CarGalary.Admin.Api
{
    public static class IOC
    {
        /// <summary>
        /// Registers all repository interfaces and implementations
        /// </summary>

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICarModelRepository, CarModelRepository>();

            services.AddScoped<ICarColorRepository, CarColorRepository>();
            services.AddScoped<ICarFeatureRepository, CarFeatureRepository>();
            services.AddScoped<ICarGalleryImageRepository, CarGalleryImageRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarTypeRepository, CarTypeRepository>();
            services.AddScoped<IAudioAndCommunicationSystemRepository, AudioAndCommunicationSystemRepository>();
            services.AddScoped<ICarCarColorRepository, CarCarColorRepository>();
            services.AddScoped<ICompanyInformationRepository, CompanyInformationRepository>();
            services.AddScoped<IContactSalesOfficerRepository, ContactSalesOfficerRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IEaseAndComfortRepository, EaseAndComfortRepository>();
            services.AddScoped<IEngineSpecificationRepository, EngineSpecificationRepository>();
            services.AddScoped<IExteriorRepository, ExteriorRepository>();
            services.AddScoped<IExtraFeatureRepository, ExtraFeatureRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();
            services.AddScoped<IFavoritesRepository, FavoritesRepository>();
            services.AddScoped<IMeasurementsRepository, MeasurementsRepository>();
            services.AddScoped<IMemberServiceRepository, MemberServiceRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<ISafetyRepository, SafetyRepository>();
            services.AddScoped<ISeatingRepository, SeatingRepository>();
            services.AddScoped<IServicesRepository, ServicesRepository>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<ITransmissionRepository, TransmissionRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            return services;
        }

        /// <summary>
        /// Registers the Unit of Work
        /// </summary>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        /// <summary>
        /// Registers all application services
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICarModelService, CarModelService>();
            services.AddScoped<ICarTypeService, CarTypeService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarFeatureService, CarFeatureService>();
            services.AddScoped<ICarGalleryImageService, CarGalleryImageService>();
            services.AddScoped<ICompanyInformationService, CompanyInformationService>();
            services.AddScoped<IContactSalesOfficerService, ContactSalesOfficerService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<IEaseAndComfortService, EaseAndComfortService>();
            services.AddScoped<IEngineSpecificationService, EngineSpecificationService>();
            services.AddScoped<IExteriorService, ExteriorService>();
            services.AddScoped<IExtraFeatureService, ExtraFeatureService>();
            services.AddScoped<IFAQService, FAQService>();
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IMeasurementsService, MeasurementsService>();
            services.AddScoped<IMemberServiceService, MemberServiceService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<ISafetyService, SafetyService>();
            services.AddScoped<ISeatingService, SeatingService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IServiceTypeService, ServiceTypeService>();
            services.AddScoped<ITransmissionService, TransmissionService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IAudioAndCommunicationSystemService, AudioAndCommunicationSystemService>();
            services.AddScoped<ICarColorService, CarColorService>();
            services.AddScoped<ICarCarColorService, CarCarColorService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }

        // <summary>
        /// Registers AutoMapper profiles
        /// </summary>
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });
            return services;
        }

        /// <summary>
        /// Registers all project dependencies in one call
        /// </summary>
        public static IServiceCollection AddCarGalaryDependencies(this IServiceCollection services)
        {
            services.AddRepositories()
                    .AddUnitOfWork()
                    .AddApplicationServices()
                    .AddMapper();

            return services;
        }
    }
}
