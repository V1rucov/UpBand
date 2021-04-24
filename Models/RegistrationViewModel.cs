using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace upband.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage ="Укажите вашу почту")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Придумайте имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Придумайте пароль")]
        [MinLength(6, ErrorMessage ="минимальная длина пароля - 6 знаков")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage ="Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
