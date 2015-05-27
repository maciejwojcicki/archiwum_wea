using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebArchiwum.WebUI.Models
{
    public class LoginModel
    {   //Logowanie

        /// <summary>
        /// Użytkownik
        /// </summary>
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        /// <summary>
        /// Hasło
        /// </summary>
        [Required]
        [RegularExpression(@"[a-zA-Z0-9]{6,}")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

    }
}