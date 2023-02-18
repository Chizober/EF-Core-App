using System;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_DAL.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        //[Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
