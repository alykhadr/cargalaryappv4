
using CarGalary.Domain.Entities;
using CarGalary.Infrastructure.Configuration;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.Context;
  public class ApplicationDbContext :  IdentityDbContext<ApplicationUser, ApplicationRole,Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<CarType> CarTypes => Set<CarType>();


    public DbSet<ApplicationRole> Roles { get; set; }
 
    public DbSet<UserFavorite> UserFavorites { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<CarFeature> CarFeatures { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<CarColor> CarColors { get; set; }
    public DbSet<CarGalleryImage> CarGalleryImages { get; set; }

    public DbSet<CarExtraDetails> CarExtraDetails { get; set; }

    public DbSet<CarModel> CarModels { get; set; }
    public DbSet<Branchs> Branches { get; set; }
    public DbSet<CompanyInformation> CompanyInformations { get; set; }
    public DbSet<BranchWorkingDays> BranchWorkingDays { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }

    public DbSet<Offer> Offers { get; set; }
    public DbSet<ContactSalesOfficer> ContactSalesOfficers { get; set; }

    public DbSet<Services> Services { get; set; }
    public DbSet<MemberService>  MemberServices { get; set; }
       public DbSet<FAQ>   FAQs { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<LookupDetails> LookupDetails { get; set; }
    public DbSet<Quotation> Quotations { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CarBrandConfiguration());
        modelBuilder.ApplyConfiguration(new CarTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CarConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserFavoriteConfiguration());
        modelBuilder.ApplyConfiguration(new CarCarFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new CarFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new CarColorConfiguration());
        modelBuilder.ApplyConfiguration(new CarCarColorConfiguration());
        modelBuilder.ApplyConfiguration(new CarGalleryImageConfiguration());

        modelBuilder.ApplyConfiguration(new CarExtraDetailsConfiguration());

        modelBuilder.ApplyConfiguration(new CarModelConfiguration());
        modelBuilder.ApplyConfiguration(new BranchConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyInformationConfiguration());
        modelBuilder.ApplyConfiguration(new BranchWorkingDaysConfiguration());
        modelBuilder.ApplyConfiguration(new ContactUsConfiguration());
        modelBuilder.ApplyConfiguration(new OfferConfiguration());
        modelBuilder.ApplyConfiguration(new ContactSalesOfficerConfiguration());

        modelBuilder.ApplyConfiguration(new ServicesConfiguration());
          modelBuilder.ApplyConfiguration(new MemberServiceConfiguration());

              modelBuilder.ApplyConfiguration(new FAQConfiguration());
              modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
              modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
              modelBuilder.ApplyConfiguration(new LookupDetailsConfiguration());
              modelBuilder.ApplyConfiguration(new QuotationConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
