using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UsersInSessions
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Session")]
        public int SessionId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual Session Session { get; set; }
        public virtual User User { get; set; }
    }

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
        public string City { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime RegistrationClosedAt { get; set; }
        public SessionType Type { get; set; }
        public DateTime? UserSubmittedAt { get; set; }
        public DateTime? AdminSubmittedAt { get; set; }
        public string RejectionReason { get; set; } 
        public bool? IsAccepted { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<UsersInSessions> Users { get; set; }

        [NotMapped]
        public SessionStatus Status {
            get
            {
                if (AdminSubmittedAt != null)
                {
                    return SessionStatus.Processed;
                }
                if (UserSubmittedAt != null)
                    return SessionStatus.InProgress;

                return SessionStatus.NotSubmitted;
            }
        }
    }

    public enum SessionType
    {
        Lection = 0, Seminar = 1, MasterClass = 2, Other = 3
    }

    public enum SessionStatus
    {
        NotSubmitted = 0,
        InProgress = 1,
        Processed = 2
    }
}
