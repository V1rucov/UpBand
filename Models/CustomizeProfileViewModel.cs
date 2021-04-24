using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace upband.Models
{
    public class CustomizeProfileViewModel
    {
        [Required(ErrorMessage ="Обязательное поле")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
