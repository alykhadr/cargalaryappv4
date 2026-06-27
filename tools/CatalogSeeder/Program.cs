using CarGalary.Domain.Entities;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var repoRoot = FindRepoRoot();
var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(repoRoot, "CarGalary.Api"))
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection was not found.");

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .Options;

await using var db = new ApplicationDbContext(options);

if (!await db.Database.CanConnectAsync())
{
    throw new InvalidOperationException("Could not connect to the SQL Server database.");
}

var specs = BuildCatalogSpecs();
var cars = await db.Cars
    .Include(car => car.CarColors)
    .OrderBy(car => car.Id)
    .ToListAsync();

if (cars.Count < specs.Count)
{
    throw new InvalidOperationException($"Expected at least {specs.Count} cars to repurpose, found {cars.Count}.");
}

var brands = await db.Brands.OrderBy(brand => brand.Id).ToListAsync();
var models = await db.CarModels.OrderBy(model => model.Id).ToListAsync();
var placeholderBrands = new Queue<Brand>(brands.Where(IsPlaceholderBrand));
var placeholderModels = new Queue<CarModel>(models.Where(IsPlaceholderModel));

var brandCache = new Dictionary<string, Brand>(StringComparer.OrdinalIgnoreCase);
var modelCache = new Dictionary<string, CarModel>(StringComparer.OrdinalIgnoreCase);

foreach (var spec in specs)
{
    var brand = EnsureBrand(spec.BrandEn, spec.BrandAr);
    var model = EnsureModel(spec.ModelEn, spec.ModelAr, brand.Id);
    spec.BrandId = brand.Id;
    spec.ModelId = model.Id;
}

for (var index = 0; index < specs.Count; index += 1)
{
    var car = cars[index];
    var spec = specs[index];

    car.NameEn = spec.NameEn;
    car.NameAr = spec.NameAr;
    car.DescriptionEn = spec.DescriptionEn;
    car.DescriptionAr = spec.DescriptionAr;
    car.ModelId = spec.ModelId;
    car.Year = spec.Year;
    car.Mileage = spec.Mileage;
    car.Vat = 15m;
    car.ConditionId = 2;
    car.SeatingCapacity = spec.SeatingCapacity;
    car.WeelSizeInch = spec.WheelSizeInch;
    car.FuelTankCapacityLiter = spec.FuelTankCapacityLiter;
    car.TrimLevel = spec.TrimLevel;
    car.VehicleClass = spec.VehicleClass;
    car.TransmisionType = spec.TransmissionType;
    car.Drivetrain = spec.Drivetrain;
    car.FuelType = spec.FuelType;
    car.Cylenders = spec.Cylinders;
    car.UpdatedAt = DateTime.UtcNow;
    car.UpdatedBy = "catalog-seeder";
    car.CreatedBy = "catalog-seeder";
    car.CreatedAt = DateTime.UtcNow.AddDays(-index * 4);
    car.IsAvailable = true;

    var basePrice = Math.Round(spec.Price / 1.15m, 2, MidpointRounding.AwayFromZero);
    foreach (var color in car.CarColors)
    {
        color.PricingPerColor = basePrice;
        color.PricePefore = basePrice;
        color.VatAmount = Math.Round(spec.Price - basePrice, 2, MidpointRounding.AwayFromZero);
        color.Discount = 0m;
        color.DiscountType = 0;
        color.TotalPrice = spec.Price;
        color.StockQuantity = Math.Max(color.StockQuantity ?? 1, 1);
        color.UpdatedAt = DateTime.UtcNow;
        color.UpdatedBy = "catalog-seeder";
        color.IsAvailable = true;
    }
}

foreach (var leftoverCar in cars.Skip(specs.Count))
{
    leftoverCar.IsAvailable = false;
    leftoverCar.UpdatedAt = DateTime.UtcNow;
    leftoverCar.UpdatedBy = "catalog-seeder";
}

await db.SaveChangesAsync();

Console.WriteLine($"Seeded {specs.Count} curated Saudi-market demo cars across {specs.Select(spec => spec.BrandEn).Distinct(StringComparer.OrdinalIgnoreCase).Count()} brands.");
Console.WriteLine(string.Join(Environment.NewLine, specs.Select(spec => $"- {spec.NameEn}: SAR {spec.Price:N0}")));

return;

Brand EnsureBrand(string nameEn, string nameAr)
{
    if (brandCache.TryGetValue(nameEn, out var cached))
    {
        return cached;
    }

    var existing = brands.FirstOrDefault(brand => string.Equals(brand.NameEn, nameEn, StringComparison.OrdinalIgnoreCase));
    if (existing != null)
    {
        brandCache[nameEn] = existing;
        return existing;
    }

    Brand brand;
    if (placeholderBrands.Count > 0)
    {
        brand = placeholderBrands.Dequeue();
        brand.NameEn = nameEn;
        brand.NameAr = nameAr;
        brand.ImageUrl = string.Empty;
        brand.CreatedBy = "catalog-seeder";
        brand.IsAvailable = true;
    }
    else
    {
        brand = new Brand
        {
            NameEn = nameEn,
            NameAr = nameAr,
            ImageUrl = string.Empty,
            CreatedBy = "catalog-seeder",
            CreatedAt = DateTime.UtcNow,
            IsAvailable = true,
        };
        db.Brands.Add(brand);
        brands.Add(brand);
        db.SaveChanges();
    }

    brandCache[nameEn] = brand;
    return brand;
}

CarModel EnsureModel(string nameEn, string nameAr, int brandId)
{
    var key = $"{brandId}:{nameEn}";
    if (modelCache.TryGetValue(key, out var cached))
    {
        return cached;
    }

    var existing = models.FirstOrDefault(model =>
        model.BrandId == brandId &&
        string.Equals(model.NameEn, nameEn, StringComparison.OrdinalIgnoreCase));

    if (existing != null)
    {
        modelCache[key] = existing;
        return existing;
    }

    CarModel model;
    if (placeholderModels.Count > 0)
    {
        model = placeholderModels.Dequeue();
        model.NameEn = nameEn;
        model.NameAr = nameAr;
        model.BrandId = brandId;
        model.ImageUrl = string.Empty;
        model.CreatedBy = "catalog-seeder";
        model.IsAvailable = true;
    }
    else
    {
        model = new CarModel
        {
            NameEn = nameEn,
            NameAr = nameAr,
            BrandId = brandId,
            ImageUrl = string.Empty,
            CreatedBy = "catalog-seeder",
            CreatedAt = DateTime.UtcNow,
            IsAvailable = true,
        };
        db.CarModels.Add(model);
        models.Add(model);
        db.SaveChanges();
    }

    modelCache[key] = model;
    return model;
}

static bool IsPlaceholderBrand(Brand brand)
{
    var name = brand.NameEn?.Trim();
    return string.IsNullOrWhiteSpace(name) || string.Equals(name, "brand", StringComparison.OrdinalIgnoreCase);
}

static bool IsPlaceholderModel(CarModel model)
{
    var name = model.NameEn?.Trim();
    return string.IsNullOrWhiteSpace(name) || string.Equals(name, "brand", StringComparison.OrdinalIgnoreCase);
}

static string FindRepoRoot()
{
    var current = new DirectoryInfo(Directory.GetCurrentDirectory());

    while (current != null)
    {
        if (Directory.Exists(Path.Combine(current.FullName, "CarGalary.Api")) &&
            Directory.Exists(Path.Combine(current.FullName, "CarGalary.Infrastructure")))
        {
            return current.FullName;
        }

        current = current.Parent;
    }

    throw new InvalidOperationException("Could not find the repository root.");
}

static List<CatalogSpec> BuildCatalogSpecs()
{
    return new List<CatalogSpec>
    {
        new("Toyota", "تويوتا", "Camry", "كامري", "Toyota Camry GLX 2023", "تويوتا كامري GLX 2023", "Reliable midsize sedan inspired by popular Saudi listings on Syarah and CarSwitch.", "سيدان متوسطة مناسبة للاستخدام اليومي ومستلهمة من أكثر السيارات انتشارا في السوق السعودي.", 2023, 42000, 112900m, 5, "18", 60m, 3, 2, 1, 1, 4),
        new("Toyota", "تويوتا", "Land Cruiser", "لاند كروزر", "Toyota Land Cruiser GXR 2021", "تويوتا لاند كروزر GXR 2021", "Large 4WD family SUV positioned around mainstream Saudi market pricing.", "دفع رباعي عائلي كبير بتسعير قريب من عروض السوق السعودي.", 2021, 78000, 287000m, 7, "20", 110m, 4, 5, 1, 4, 6),
        new("Toyota", "تويوتا", "Corolla", "كورولا", "Toyota Corolla XLI 2022", "تويوتا كورولا XLI 2022", "Economical daily commuter with strong resale appeal in KSA.", "سيارة اقتصادية يومية مع طلب قوي في السوق السعودي.", 2022, 51000, 78900m, 5, "16", 50m, 2, 1, 3, 1, 4),
        new("Toyota", "تويوتا", "Fortuner", "فورتشنر", "Toyota Fortuner GX2 2022", "تويوتا فورتشنر GX2 2022", "Popular seven-seat SUV aligned with Saudi family-car demand.", "سيارة SUV سبعة مقاعد مناسبة للعائلة ومرغوبة في السوق السعودي.", 2022, 63000, 129500m, 7, "18", 80m, 3, 5, 1, 4, 6),
        new("BMW", "بي ام دبليو", "5 Series", "الفئة الخامسة", "BMW 520i Executive 2021", "بي ام دبليو 520i إكزكتيف 2021", "Premium executive sedan priced in line with used luxury stock in Saudi Arabia.", "سيدان فاخرة تنفيذية ضمن نطاق أسعار السيارات الأوروبية المستعملة في السعودية.", 2021, 47000, 168000m, 5, "18", 68m, 4, 4, 1, 2, 4),
        new("BMW", "بي ام دبليو", "X5", "اكس 5", "BMW X5 xDrive40i 2020", "بي ام دبليو X5 xDrive40i 2020", "Luxury AWD SUV suited to higher-end Riyadh and Jeddah demand.", "SUV فاخرة بالدفع الكلي تناسب الطلب المرتفع في الرياض وجدة.", 2020, 69000, 249000m, 5, "20", 83m, 4, 4, 1, 3, 6),
        new("Mercedes", "مرسيدس", "C200", "سي 200", "Mercedes C200 Avantgarde 2022", "مرسيدس C200 أفانتجارد 2022", "Refined German sedan benchmarked against premium KSA listings.", "سيدان ألمانية راقية مستوحاة من عروض السيارات الفاخرة في السعودية.", 2022, 39000, 191000m, 5, "18", 66m, 4, 4, 1, 2, 4),
        new("Mercedes", "مرسيدس", "GLC 300", "جي ال سي 300", "Mercedes GLC 300 2021", "مرسيدس GLC 300 2021", "Balanced luxury SUV with strong showroom appeal for Saudi buyers.", "SUV فاخرة متوازنة بجاذبية عالية للمشترين في السوق السعودي.", 2021, 54000, 223000m, 5, "19", 66m, 4, 5, 1, 3, 4),
        new("Honda", "هوندا", "Accord", "أكورد", "Honda Accord EX 2021", "هوندا أكورد EX 2021", "Trusted midsize sedan positioned for practical KSA shoppers.", "سيدان متوسطة موثوقة تناسب الباحثين عن سيارة عملية في السعودية.", 2021, 58000, 96900m, 5, "17", 56m, 3, 2, 3, 1, 4),
        new("Honda", "هوندا", "CR-V", "سي آر في", "Honda CR-V Touring 2020", "هوندا CR-V تورينج 2020", "Family-focused crossover inspired by popular SUV segments on Saudi marketplaces.", "كروس أوفر عائلية مستلهمة من أكثر فئات الـSUV طلبا في مواقع السيارات السعودية.", 2020, 72000, 118000m, 5, "18", 57m, 4, 5, 3, 1, 4),
        new("Volvo", "فولفو", "XC60", "اكس سي 60", "Volvo XC60 B5 2021", "فولفو XC60 B5 2021", "Elegant Scandinavian SUV tailored to premium-used inventory trends.", "SUV اسكندنافية أنيقة ضمن اتجاهات السيارات الأوروبية المستعملة الفاخرة.", 2021, 44000, 154000m, 5, "19", 71m, 4, 4, 1, 3, 4),
        new("Tesla", "تسلا", "Model 3", "موديل 3", "Tesla Model 3 Long Range 2022", "تسلا موديل 3 لونج رينج 2022", "Modern EV entry inspired by growing electric-car interest in KSA.", "سيارة كهربائية حديثة مستلهمة من ارتفاع الاهتمام بالمركبات الكهربائية في السعودية.", 2022, 31000, 179000m, 5, "19", 0m, 4, 3, 1, 3, 0, fuelType: 4),
        new("Bugatti", "بوغاتي", "Chiron", "شيرون", "Bugatti Chiron Sport 2019", "بوغاتي شيرون سبورت 2019", "Halo supercar listing for the luxury segment found across Gulf marketplaces.", "سيارة خارقة للفئة الفاخرة جدا كما يظهر في أسواق الخليج للسيارات المميزة.", 2019, 9000, 1150000m, 2, "21", 100m, 4, 3, 4, 4, 16),
    };
}

sealed class CatalogSpec(
    string brandEn,
    string brandAr,
    string modelEn,
    string modelAr,
    string nameEn,
    string nameAr,
    string descriptionEn,
    string descriptionAr,
    int year,
    int mileage,
    decimal price,
    int seatingCapacity,
    string wheelSizeInch,
    decimal fuelTankCapacityLiter,
    int trimLevel,
    int vehicleClass,
    int transmissionType,
    int drivetrain,
    int cylinders,
    int fuelType = 1)
{
    public string BrandEn { get; } = brandEn;
    public string BrandAr { get; } = brandAr;
    public string ModelEn { get; } = modelEn;
    public string ModelAr { get; } = modelAr;
    public string NameEn { get; } = nameEn;
    public string NameAr { get; } = nameAr;
    public string DescriptionEn { get; } = descriptionEn;
    public string DescriptionAr { get; } = descriptionAr;
    public int Year { get; } = year;
    public int Mileage { get; } = mileage;
    public decimal Price { get; } = price;
    public int SeatingCapacity { get; } = seatingCapacity;
    public string WheelSizeInch { get; } = wheelSizeInch;
    public decimal FuelTankCapacityLiter { get; } = fuelTankCapacityLiter;
    public int TrimLevel { get; } = trimLevel;
    public int VehicleClass { get; } = vehicleClass;
    public int TransmissionType { get; } = transmissionType;
    public int Drivetrain { get; } = drivetrain;
    public int Cylinders { get; } = cylinders;
    public int FuelType { get; } = fuelType;
    public int BrandId { get; set; }
    public int ModelId { get; set; }
}
