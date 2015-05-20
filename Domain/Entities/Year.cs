using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Year
    {
        public int YearId { get; set; }
        public string Name { get; set; }

        public virtual List<Graduate> Graduates { get; set; }
    }
}
