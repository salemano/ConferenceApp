using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class FileData
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public byte[] Data { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }

    public class Image
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public int FileSize { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("FileData")]
        public int FileDataId { get; set; }

        public virtual FileData FileData { get; set; }
        public virtual User User { get; set; }
    }
}
