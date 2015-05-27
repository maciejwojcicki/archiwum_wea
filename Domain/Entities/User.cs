using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
       [Required]
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z0-9]{6,}")]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
