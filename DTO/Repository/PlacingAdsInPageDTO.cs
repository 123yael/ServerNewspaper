using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class PlacingAdsInPageDTO
    {
        public int PlaceInPageId { get; set; }

        public int? PageId { get; set; }

        public int? DetailsId { get; set; }
    }
}
