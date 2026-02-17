namespace CarGalary.Application.Dtos.Car.Command
{
    public class CreateCarRequestDto
    {
        public int ModelId { get; set; }
        public int TypeId { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
    }
}
