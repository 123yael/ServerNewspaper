using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class PlacingAdsInPage
{
    public int PlaceInPageId { get; set; }

    public int? PageId { get; set; }

    public int? DetailsId { get; set; }

    public virtual OrderDetail? Details { get; set; }

    public virtual PagesInNewspaper? Page { get; set; }
}
