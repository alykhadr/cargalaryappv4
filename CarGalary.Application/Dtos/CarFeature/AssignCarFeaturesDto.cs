using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Application.Dtos
{
    public class AssignCarFeaturesDto
{
    public int CarId { get; set; }
    public List<int> FeatureIds { get; set; } = new();
}

}