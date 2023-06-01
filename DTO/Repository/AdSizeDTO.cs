using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class AdSizeDTO
    {
        public int SizeId { get; set; }

        public string SizeName { get; set; } = null!;

        public decimal SizeHeight { get; set; }

        public decimal SizeWidth { get; set; }

        public decimal SizePrice { get; set; }

        public string SizeImg { get; set; } = null!;
    }
}
