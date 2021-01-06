using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(100, ErrorMessage = "The field {0} can accept maximum {1} and minimum {2} characters", MinimumLength = 7)]
        [DataType(DataType.EmailAddress)]
        [Index("UserNameIndex", IsUnique = true)]
        public string UserName { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(50, ErrorMessage = " The field {0} can accept maximum {1} and minimum {2} characters ", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(50, ErrorMessage = " The field {0} can accept maximum {1} and minimum {2} characters", MinimumLength = 2)]
        public string Surname { get; set; }

        [Display(Name = "User")]
        public string FullName { get { return string.Format("{0} {1}", this.Name, this.Surname); } }

        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(20, ErrorMessage = " The field {0} can accept maximum {1} and minimum {2} characters", MinimumLength = 7)]
        public string Phone { get; set; }

        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(100, ErrorMessage = " The field {0} can accept maximum {1} and minimum {2} characters ", MinimumLength = 7)]
        public string Address { get; set; }

        [Display(Name = "Image")]
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [Display(Name = "Student")]
        public bool Student { get; set; }

        [Display(Name = "Teacher")]
        public bool Teacher { get; set; }

        public virtual ICollection<Groups> Groups { get; set; }

        [JsonIgnore]
        public virtual ICollection<GroupsDetails> GroupsDetails { get; set; }

    }
}