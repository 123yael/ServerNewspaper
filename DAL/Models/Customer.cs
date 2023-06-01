using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Customer
{
    public int CustId { get; set; }

    public string? CustFirstName { get; set; }

    public string? CustLastName { get; set; }

    public string CustEmail { get; set; } = null!;

    public string CustPhone { get; set; } = null!;

    public string CustPassword { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
