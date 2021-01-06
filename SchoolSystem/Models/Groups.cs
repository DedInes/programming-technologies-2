using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolSystem.Models
{
    public class Groups
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = " The field {0} is required!")]
        [StringLength(50, ErrorMessage = " The field {0} can accept maximum {1} and minimum {2} characters ", MinimumLength = 3)]
        [Index("GroupDescriptionIndex", IsUnique = true)]
        public string Description { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User Teacher { get; set; }

        [JsonIgnore]
        public virtual ICollection<GroupsDetails> GroupsDetails { get; set; }

    }
}