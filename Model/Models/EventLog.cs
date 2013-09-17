using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class EventLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? ObjectId { get; set; }
        public DateTime CreateDate { get; set; }
        public int LogLevel { get; set; }
        public int LogType { get; set; }
        public string IPAddress { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
