using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Model.Models
{
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("Photo")]
        public int? PhotoId { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }

        public Guid? ActivationToken { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime? RegisteredAt { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public Guid? PasswordRecoveryToken { get; set; }

        public bool IsAdministrator { get; set; }

        public virtual Image Photo { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<UsersInSessions> Sessions { get; set; }

        public bool IsDeleted { get; set; }        
                                             
        [NotMapped]
        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                    return Email;

                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
