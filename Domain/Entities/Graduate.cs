using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Graduate
    {
        [Required]
        public int GraduateId { get; set; }
        [Required]
        [Display(Name = "imię")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Opis")]
        public string BIO { get; set; }

        [ForeignKey("YearId")]
        public virtual Year Year { get; set; }
        public int? YearId { get; set; }
    }
}
