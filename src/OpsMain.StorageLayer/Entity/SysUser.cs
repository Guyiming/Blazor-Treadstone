using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.Entity
{
    [Index(nameof(UserName))]
    public class SysUser : BaseEntity
    {
        [MaxLength(200)]
        [Comment("用户名")]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        public ICollection<R_RoleUser> R_RoleUsers { get; set; }
    }
}
