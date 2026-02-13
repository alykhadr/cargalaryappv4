
using CarGalary.Domain.Entities;
using CarGalary.Infrastructure.Configuration;
using CarGalary.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.Context;
  public class ApplicationDbContext :  IdentityDbContext<ApplicationUser, ApplicationRole,Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarBrand> CarBrands => Set<CarBrand>();
    public DbSet<CarType> CarTypes => Set<CarType>();


    public DbSet<ApplicationRole> Roles { get; set; }
 
    public DbSet<UserProfile> Profiles { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }
    public DbSet<CarFeature> CarFeatures { get; set; }
    public DbSet<CarCarFeature> CarCarFeatures { get; set; }
    public DbSet<CarColor> CarColors { get; set; }
    public DbSet<CarGalleryImage> CarGalleryImages { get; set; }
    public DbSet<EngineSpecification> EngineSpecifications { get; set; }
    public DbSet<Transmission> Transmissions { get; set; }
    public DbSet<ExtraFeature> ExtraFeatures { get; set; }
    public DbSet<EaseAndComfort> EaseAndComforts { get; set; }
    public DbSet<AudioAndCommunicationSystem> AudioAndCommunicationSystems { get; set; }
    public DbSet<Exterior> Exteriors { get; set; }
    public DbSet<Safety> Safeties { get; set; }
    public DbSet<Seating> Seatings { get; set; }
    public DbSet<CarModel> CarModels { get; set; }
    public DbSet<Branchs> Branches { get; set; }
    public DbSet<CompanyInformation> CompanyInformations { get; set; }
    public DbSet<BranchWorkingDays> BranchWorkingDays { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }

    public DbSet<Offer> Offers { get; set; }
    public DbSet<ContactSalesOfficer> ContactSalesOfficers { get; set; }

    public DbSet<ServiceType> ServiceTypes { get; set; }
    public DbSet<Services> Services { get; set; }
    public DbSet<MemberService>  MemberServices { get; set; }
       public DbSet<FAQ>   FAQs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CarBrandConfiguration());
        modelBuilder.ApplyConfiguration(new CarTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CarConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        modelBuilder.ApplyConfiguration(new UserFavoriteConfiguration());
        modelBuilder.ApplyConfiguration(new CarCarFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new CarFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new CarColorConfiguration());
        modelBuilder.ApplyConfiguration(new CarCarColorConfiguration());
        modelBuilder.ApplyConfiguration(new CarGalleryImageConfiguration());
        modelBuilder.ApplyConfiguration(new EngineSpecificationConfiguration());
        modelBuilder.ApplyConfiguration(new TransmissionConfiguration());
        modelBuilder.ApplyConfiguration(new ExtraFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new AudioAndCommunicationSystemConfiguration());
        modelBuilder.ApplyConfiguration(new EaseAndComfortConfiguration());
        modelBuilder.ApplyConfiguration(new ExteriorConfiguration());
        modelBuilder.ApplyConfiguration(new SafetyConfiguration());
        modelBuilder.ApplyConfiguration(new SeatingConfiguration());
        modelBuilder.ApplyConfiguration(new CarModelConfiguration());
        modelBuilder.ApplyConfiguration(new BranchConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyInformationConfiguration());
        modelBuilder.ApplyConfiguration(new BranchWorkingDaysConfiguration());
        modelBuilder.ApplyConfiguration(new ContactUsConfiguration());
        modelBuilder.ApplyConfiguration(new OfferConfiguration());
        modelBuilder.ApplyConfiguration(new ContactSalesOfficerConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceTypeConfiguration());

        modelBuilder.ApplyConfiguration(new ServicesConfiguration());
          modelBuilder.ApplyConfiguration(new MemberServiceConfiguration());

              modelBuilder.ApplyConfiguration(new FAQConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}