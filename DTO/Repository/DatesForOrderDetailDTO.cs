using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class DatesForOrderDetailDTO
    {
        public int DateId { get; set; }

        public int? DetailsId { get; set; }

        public DateTime Date { get; set; }

        public bool? DateStatus { get; set; }
    }
}
