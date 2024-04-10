using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Repository
{
    public class NewspapersPublishedDTO
    {
        public int NewspaperId { get; set; }

        public string PublicationDate { get; set; }

        public int CountPages { get; set; }

        public int MagazineNumber { get; set; }

    }
}
