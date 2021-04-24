using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace upband.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Введите эл.почту")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Введите пароль")]
        public string Password { get; set; }
    }
}
