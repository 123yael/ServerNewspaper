using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class AdPlacement
{
    public int PlaceId { get; set; }

    public string PlaceName { get; set; } = null!;

    public decimal PlacePrice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
