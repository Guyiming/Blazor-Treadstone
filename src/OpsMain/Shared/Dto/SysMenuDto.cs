using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.Shared
{
    public class SysMenuDto : BaseDto
    {
        #region 本地
        public string ParentName { get; set; }
        #endregion
     
        public string MenuName { get; set; }

        public string Href { get; set; }

        public long? ParentId { get; set; }

        public long? OrderNo { get; set; }

        public string Icon { get; set; }

        public bool? IsLeafNode => SubMenus == null ? null : (SubMenus.Count > 0);

        public List<SysMenuDto> SubMenus { get; set; } = new List<SysMenuDto>();


    }
}
