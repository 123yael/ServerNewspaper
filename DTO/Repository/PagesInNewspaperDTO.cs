using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class PagesInNewspaperDTO
    {
        public int PageId { get; set; }

        public int PageNumber { get; set; }

        public int? NewspaperId { get; set; }
    }
}
