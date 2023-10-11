using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class OrderDetailsTable
    {
        public int Id { get; set; }

        public string? AdFile { get; set; }

        public string? SizeName { get; set; }

        public string? CustFullName { get; set; }

        public string? CustEmail { get; set; }

        public string? CustPhone { get; set; }

        public int? WeekNumber { get; set; }

        public DateTime Date { get; set; }

        public bool? ApprovalStatus { get; set; }

    }
}
