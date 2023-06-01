using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustId { get; set; }

    public decimal OrderFinalPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual Customer Cust { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
