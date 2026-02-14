

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

            services.AddScoped<ICarColorRepository, CarColorRepository>();
            services.AddScoped<ICarFeatureRepository, CarFeatureRepository>();
            services.AddScoped<ICarGalleryImageRepository, CarGalleryImageRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarTypeRepository, CarTypeRepository>();
            services.AddScoped<ICompanyInformationRepository, CompanyInformationRepository>();
            services.AddScoped<IContactSalesOfficerRepository, ContactSalesOfficerRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();
            services.AddScoped<IFavoritesRepository, FavoritesRepository>();
            services.AddScoped<IMemberServiceRepository, MemberServiceRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IServicesRepository, ServicesRepository>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
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
