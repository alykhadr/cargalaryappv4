using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Application.Dtos
{
   public class FavoriteCarDto
{
    public int CarId { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public decimal Price { get; set; }
}
}