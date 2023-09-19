using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class NewspapersPublished
{
    public int NewspaperId { get; set; }

    public DateTime PublicationDate { get; set; }

    public int CountPages { get; set; }
}
