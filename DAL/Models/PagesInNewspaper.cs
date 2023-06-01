using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class PagesInNewspaper
{
    public int PageId { get; set; }

    public int PageNumber { get; set; }

    public int? NewspaperId { get; set; }

    public virtual NewspapersPublished? Newspaper { get; set; }

    public virtual ICollection<PlacingAdsInPage> PlacingAdsInPages { get; } = new List<PlacingAdsInPage>();
}
