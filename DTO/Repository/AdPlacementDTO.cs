using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class AdPlacementDTO
    {
        public int PlaceId { get; set; }

        public string PlaceName { get; set; } = null!;

        public decimal PlacePrice { get; set; }
    }
}
