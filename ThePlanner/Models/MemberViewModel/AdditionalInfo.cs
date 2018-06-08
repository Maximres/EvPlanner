using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThePlanner.Models.MemberViewModel
{
    public class AdditionalInfo
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"\d+")]
        public string Phone { get; set; }

        [Required]
        [Range(1, 170)]
        [UIHint("number")]
        public int? Age { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}