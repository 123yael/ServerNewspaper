using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorDesc { get; set; } = string.Empty;
        public int ErrorCode { get; set; } = 0;
    }
}
