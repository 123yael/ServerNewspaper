using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class DatesForOrderDetail
{
    public int DateId { get; set; }

    public int? DetailsId { get; set; }

    public DateTime Date { get; set; }

    public bool? ApprovalStatus { get; set; }

    public virtual OrderDetail? Details { get; set; }
}
