using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderDetail
{
    public int DetailsId { get; set; }

    public int? OrderId { get; set; }

    public int? WordCategoryId { get; set; }

    public string? AdContent { get; set; }

    public string? AdFile { get; set; }

    public int? SizeId { get; set; }

    public int PlaceId { get; set; }

    public int? AdDuration { get; set; }

    public virtual ICollection<DatesForOrderDetail> DatesForOrderDetails { get; } = new List<DatesForOrderDetail>();

    public virtual Order? Order { get; set; }

    public virtual AdPlacement Place { get; set; } = null!;

    public virtual AdSize? Size { get; set; }

    public virtual WordAdSubCategory? WordCategory { get; set; }
}
