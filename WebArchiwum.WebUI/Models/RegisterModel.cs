using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebArchiwum.WebUI.Models
{
    public class RegisterModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z0-9]{6,}")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}