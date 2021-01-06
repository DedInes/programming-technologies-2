using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolSystem.Models
{
    [NotMapped]
    public class UserPassword : User
    {
        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(15, ErrorMessage = " The field {0} can accept maximum {1} and minimum {2} characters", MinimumLength = 3)]
        public string Password { get; set; }
    }
}