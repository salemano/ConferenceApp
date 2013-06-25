using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Session
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        //public int Logo { get; set; }
        public string Overview { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsAccepted { get; set; }
        public string City { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime RegistrationClosedAt { get; set; }
    }
}
