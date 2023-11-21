using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{

    enum Role
    {
        Admin,
    }

    public class UserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        public UserDTO() { }

    }
}
