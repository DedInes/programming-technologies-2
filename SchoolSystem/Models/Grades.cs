using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolSystem.Models
{
    public class Grades
    {
        [Key]
        public int GradeId { get; set; }

        public int GroupsDetailsId { get; set; }

        [Required(ErrorMessage = "Enter the field {0}")]
        [Range(0, 1, ErrorMessage = "Field {0} must be {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float Percentage { get; set; }

        [Required(ErrorMessage = "Enter the field {0}")]
        [Range(0, 5, ErrorMessage = "Field {0} must be {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Grade { get; set; }

        public virtual GroupsDetails GroupsDetails { get; set; }

    }
}