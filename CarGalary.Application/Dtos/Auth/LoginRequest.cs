using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Application.Dtos.Auth
{
   public class LoginRequest
    {
        public string UserName { get; set; } = null!;// may be email or user name
        public string Password { get; set; } = null!;
    }
}