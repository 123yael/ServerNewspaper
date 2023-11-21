using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class FinishOrderDTO
    {
        public List<string> ListDates { get; set; } = new List<string>();

        public List<OrderDetailDTO> ListOrderDetails { get; set; } = new List<OrderDetailDTO>();

        public List<IFormFile> ImageList { get; set; } = new List<IFormFile>();

    }
}
