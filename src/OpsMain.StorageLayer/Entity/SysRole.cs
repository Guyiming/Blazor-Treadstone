using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.Entity
{
    public class SysRole : BaseEntity
    {
        [MaxLength(200)]
        [Comment("角色名称")]
        public string RoleName { get; set; }


        public ICollection<R_RoleMenu> R_RoleMenus { get; set; }


        public ICollection<R_RoleUser> R_RoleUsers { get; set; }
    }
    
   
}
