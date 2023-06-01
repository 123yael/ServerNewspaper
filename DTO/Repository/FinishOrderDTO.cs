using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class FinishOrderDTO
    {
        public CustomerDTO Customer { get; set; }

        public List<List<DateTime>> ListDates { get; set; }

        public List<OrderDetailDTO> ListOrderDetails { get; set; }

    }
}
