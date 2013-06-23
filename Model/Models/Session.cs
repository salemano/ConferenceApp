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
        public string Subject { get; set; }
        //public int TypeId { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public bool? IsConfirmed { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime RegistrationClosedAt { get; set; }
    }
}
