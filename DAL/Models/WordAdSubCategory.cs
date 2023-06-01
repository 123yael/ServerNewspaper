using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class WordAdSubCategory
{
    public int WordCategoryId { get; set; }

    public string WordCategoryName { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
