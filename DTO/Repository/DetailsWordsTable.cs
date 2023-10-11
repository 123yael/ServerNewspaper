using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class DetailsWordsTable
    {
        public int Id { get; set; }

        public string? WordCategoryName { get; set; }

        public string? AdContent { get; set; }

        public string? CustFullName { get; set; }

        public string? CustEmail { get; set; }

        public string? CustPhone { get; set; }

        public int WeekNumber { get; set; }

        public DateTime Date { get; set; }

        public bool? ApprovalStatus { get; set; }

    }
}
