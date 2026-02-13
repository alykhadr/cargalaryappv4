
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CarGalary.Infrastructure.Context
{

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Try to find the API project's appsettings.json
            var currentDirectory = Directory.GetCurrentDirectory();
            var apiProjectPath = Path.Combine(currentDirectory, "../CarGalary.API");
            
            // Determine the base path for configuration
            string basePath;
            if (Directory.Exists(apiProjectPath))
            {
                basePath = apiProjectPath;
            }
            else if (File.Exists(Path.Combine(currentDirectory, "appsettings.json")))
            {
                basePath = currentDirectory;
            }
            else
            {
                // Fallback: search parent directories
                var parentDir = Directory.GetParent(currentDirectory);
                basePath = parentDir != null && File.Exists(Path.Combine(parentDir.FullName, "appsettings.json"))
                    ? parentDir.FullName
                    : currentDirectory;
            }

            // Build configuration - USE ConfigurationBuilder (concrete class)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Create DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}