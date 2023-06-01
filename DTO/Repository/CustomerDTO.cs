using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class CustomerDTO
    {
        public int CustId { get; set; }

        public string? CustFirstName { get; set; }

        public string? CustLastName { get; set; }

        public string CustEmail { get; set; } = null!;

        public string CustPhone { get; set; } = null!;

        public string CustPassword { get; set; } = null!;
    }
}
