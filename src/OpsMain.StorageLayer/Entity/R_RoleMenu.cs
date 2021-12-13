using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.Entity
{
    public class R_RoleMenu : BaseEntity
    {
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public SysRole Role { get; set; }
        public SysMenu Menu { get; set; }
    }
}
