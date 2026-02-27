

namespace CarGalary.Domain.Entities
{

    public class Car : BaseEntity
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;

        public int ModelId { get; set; }
        public CarModel? CarModel { get; set; }

        public int TypeId { get; set; }
        public CarType? Type { get; set; }
        public int Year { get; set; }

        // عدد الأميال التي قطعتها السيارة
        public int Mileage { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? CreatedBy { get; set; }
        public decimal? Vat { get; set; }

        // New or used
        public int? ConditionId { get; set; }
        // عدد الركاب 
        public int? SeatingCapacity { get; set; }
        //يعني البوصة — وحدة قياس للطول. في سياق السيارات، يشير حجم العجلة بالبوصة إلى قطر العجلة. كلما زاد حجم العجلة، زاد قطرها، مما يؤثر على مظهر السيارة وأدائها. العجلات الأكبر توفر مظهرًا رياضيًا وتحسينًا في الثبات، بينما العجلات الأصغر قد توفر راحة أفضل في القيادة.
        public string? WeelSizeInch { get; set; }

        //سعة البنزين تعني كمية الوقود التي يستطيع خزان السيارة حمله، وتقاس عادةً باللترات أو الجالونات. سعة البنزين تؤثر على مدى السيارة، حيث كلما زادت السعة، زادت المسافة التي يمكن للسيارة قطعها قبل الحاجة إلى إعادة التزود بالوقود. سعة البنزين مهمة للسائقين الذين يخططون لرحلات طويلة أو يستخدمون السيارة بشكل يومي لمسافات طويلة.
        public decimal? FuelTankCapacityLiter { get; set; }

        //   ستاندر  	Standard
        // نص فل	Mid Option
        // فل	Full Option
        // فل كامل	Full Option / Premium
        public int? TrimLevel { get; set; }

        // Economy	اقتصادي
        // Standard	عادي
        // Sport	رياضي
        // Luxury	فاخر
        // SUV	دفع رباعي
        // Pickup	بيك أب
        public int? VehicleClass { get; set; }

        public string? PlateNumberAr { get; set; }
        public string? PlateNumberEn { get; set; }
        // (AT/MT/CVT/DCT)
        public int? TransmisionType { get; set; }
        //DRIVETRAIN (FWD/RWD/AWD/4WD)
        public int? Drivetrain { get; set; }
        public int? Cylenders { get; set; }
        //(Gasoline, Diesel, Hybrid, Electric)
        public int? FuelType { get; set; }
        public string? EnginNumber { get; set; }
        // all nationalities 
        public int?  ManufactureCountryId { get; set; }

        public int BranchId { get; set; }
        public Branchs? Branchs { get; set; }

        public ICollection<UserFavorite> FavoritedBy { get; set; }
            = new List<UserFavorite>();

        public ICollection<CarFeature> CarFeatures { get; set; }
        = new List<CarFeature>();


        // Navigation property for many-to-many
        public ICollection<CarColor> CarColors { get; set; } = new List<CarColor>();

        // Images
        public ICollection<CarGalleryImage> CarImages { get; set; } = new List<CarGalleryImage>();



        public ICollection<CarExtraDetails> CarExtraDetails { get; set; } = new List<CarExtraDetails>();

    }
}
