using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class AdvertisementCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
