using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IJWTGenerator
    {
        string CreateToken(AppUser appUser);
    }
}
