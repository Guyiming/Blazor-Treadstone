using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.Shared.Dto
{
    public class SysRoleDto : BaseDto
    {
        public string RoleName { get; set; }
        public IEnumerable<SysMenuDto> Menus { get; set; } = new List<SysMenuDto>();

    }
}
