using System;
using System.Collections.Generic;

namespace Patholab.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(int userId, string username, string role, List<string> permissions, out DateTime expiration);
    }
}
