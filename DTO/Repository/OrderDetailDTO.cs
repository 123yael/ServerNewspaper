using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class OrderDetailDTO
    {
        public int DetailsId { get; set; }

        public int? OrderId { get; set; }

        public int? CategoryId { get; set; }

        public int? WordCategoryId { get; set; }

        public string? AdContent { get; set; }

        public string? AdFile { get; set; }

        public int? SizeId { get; set; }

        public int PlaceId { get; set; }

        public int? AdDuration { get; set; }
    }
}
