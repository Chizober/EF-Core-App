using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_DAL.Entities
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }
}
