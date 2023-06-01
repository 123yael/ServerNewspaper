using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class AdSize
{
    public int SizeId { get; set; }

    public string SizeName { get; set; } = null!;

    public decimal SizeHeight { get; set; }

    public decimal SizeWidth { get; set; }

    public decimal SizePrice { get; set; }

    public string SizeImg { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
