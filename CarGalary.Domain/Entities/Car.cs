

namespace CarGalary.Domain.Entities
{

public class Car:BaseEntity
{

    public int ModelId { get; set; }
    public CarModel? CarModel { get; set; } 

    public int TypeId { get; set; }
    public CarType? Type { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string? DescriptionAr { get; set; }
     public string? DescriptionEn { get; set; }
    public string? CreatedBy { get; set; }

    public ICollection<UserFavorite> FavoritedBy { get; set; }
        = new List<UserFavorite>();

        public ICollection<CarCarFeature> CarCarFeatures { get; set; }
        = new List<CarCarFeature>();


        // Navigation property for many-to-many
    public ICollection<CarCarColor> CarColors { get; set; } = new List<CarCarColor>();

     // Images
    public ICollection<CarGalleryImage> CarImages { get; set; } = new List<CarGalleryImage>();

    // Navigation property
        public ICollection<EngineSpecification> EngineSpecifications { get; set; }= new List<EngineSpecification>();

        // Navigation property
        public ICollection<Transmission>  Transmissions { get; set; }=new List<Transmission>();
         public ICollection<Measurements>   Measurements { get; set; }=new List<Measurements>();
         public ICollection<ExtraFeature>    ExtraFeatures { get; set; }=new List<ExtraFeature>();
           public ICollection<AudioAndCommunicationSystem>    AudioAndCommunicationSystems { get; set; }=new List<AudioAndCommunicationSystem>();
           public ICollection<EaseAndComfort>    EaseAndComforts { get; set; }=new List<EaseAndComfort>();
          public ICollection<Exterior>     Exteriors { get; set; }=new List<Exterior>();
          public ICollection<Safety>     Safeties { get; set; }=new List<Safety>();
           public ICollection<Seating>     Seatings { get; set; }=new List<Seating>();
}
}
