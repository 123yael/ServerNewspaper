using DAL.Models;
using DTO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Jwt
{
    public interface IJwtService
    {
        public string CreateToken(CustomerDTO customer);

        public int GetIdFromToken(string jwtToken);

        public string GetPasswordFromToken(string jwtToken);

        public string GetEmailFromToken(string jwtToken);
    }
}
