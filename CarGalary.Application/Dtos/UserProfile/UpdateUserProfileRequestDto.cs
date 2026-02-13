using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos
{
    public class UpdateUserProfileRequestDto
    {
          public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IFormFile? File { get; set; }
    }
}