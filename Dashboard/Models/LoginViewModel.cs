using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class LoginViewModel
    {
        
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(20, ErrorMessage = "Mínimo 4 caracteres. ", MinimumLength = 4)]
        public string Password { get; set; }
    }
}