namespace CarGalary.Domain.Entities
{
    public class Quotation : BaseEntity
    {
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int VehicleOwnerType { get; set; }
        public int PaymentMethod { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public int CarId { get; set; }
        public Car Car { get; set; } = default!;
        public string? Notes { get; set; }

        public LookupDetails? VehicleOwnerTypeLookup { get; set; }
        public LookupDetails? PaymentMethodLookup { get; set; }
        public LookupDetails? RegionLookup { get; set; }
        public LookupDetails? CityLookup { get; set; }
    }
}
