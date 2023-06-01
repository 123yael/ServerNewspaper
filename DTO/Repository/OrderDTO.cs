using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int CustId { get; set; }

        public decimal OrderFinalPrice { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
