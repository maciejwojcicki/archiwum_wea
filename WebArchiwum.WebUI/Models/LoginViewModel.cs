using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebArchiwum.WebUI.Models
{
    public class LoginViewModel
    {   //Logowanie

        /// <summary>
        /// Użytkownik
        /// </summary>
        [Required]
        [Display(Name = "użytkownik")]
        public string Username { get; set; }

        /// <summary>
        /// Hasło
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "hasło")]
        public string Password { get; set; }

    }
}