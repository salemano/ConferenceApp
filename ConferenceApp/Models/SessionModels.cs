using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceApp.Models
{
    public class CreateNewSession
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        //[Display(Name = "logo")]
        ////[DataType(DataType.ImageUrl)]
        //public int PhotoId { get; set; }

        [Required]
        [Display(Name = "Overview")]
        public string Overview { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Start")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime Start { get; set; }


        [Display(Name = "End")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime End { get; set; }

        [Required]
        [Display(Name = "Registration closed at")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime RegistrationClosedAt { get; set; }
    }
    public class EditSession
    {
        [Display(Name = "Start")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime Start { get; set; }

        [Display(Name = "End")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime End { get; set; }

        [Display(Name = "Registration closed at")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.mm.yy}")]
        public DateTime RegistrationClosedAt { get; set; }
    }
}