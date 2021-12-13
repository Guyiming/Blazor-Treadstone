using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.Entity
{
    public class SysMenu:BaseEntity
    {
        [Comment("菜单名称")]
        [MaxLength(200)]
        public string MenuName { get; set; }
        [Comment("菜单Url")]
        [MaxLength(200)]
        public string Href { get; set; }
        
        [Comment("菜单图标")]
        [MaxLength(1100)]
        public string Icon { get; set; }

        public long? ParentId { get; set; }

        public long OrderNo { get; set; }

        public SysMenu ParentMenu { get; set; }

        public ICollection<SysMenu> SubMenus { get; set; }//下属子菜单

        public ICollection<R_RoleMenu> R_RoleMenus { get; set; }
    }
}
