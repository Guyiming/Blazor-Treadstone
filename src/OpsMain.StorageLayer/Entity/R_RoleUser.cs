using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.Entity
{
    public class R_RoleUser : BaseEntity
    {
        public long RoleId { get; set; }
        public long UserId { get; set; }
        public SysRole Role { get; set; }
        public SysUser User { get; set; }
    }
}
