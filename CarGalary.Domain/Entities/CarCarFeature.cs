// CarFeature = what features exist
// CarCarFeature = which car has which feature
// Many-to-many requires a join table
// Never store features as a string
// This pattern is industry standard


using CarGalary.Domain.Entities;


///
/// Summary (Plain English)
/// 
public class CarCarFeature
{
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public int CarFeatureId { get; set; }
    public CarFeature CarFeature { get; set; } = null!;
}