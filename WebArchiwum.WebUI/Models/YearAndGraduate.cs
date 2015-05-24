using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebArchiwum.WebUI.Models
{
    public class YearAndGraduate
    {
        public Year Years { get; set; }
        public Graduate Graduates { get; set; }
    }
}