using OpsMain.Shared.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.Shared
{
    public class SysUserDto:BaseDto
    {
        #region 前端字段
        [Required(ErrorMessage ="不能为空")]
        public string NewPassword { get; set; }
        #endregion
        public string UserName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "不能为空")]
        public string Password { get; set; }

        public List<string> RoleNames { get; set; } = new List<string>();
        public List<long> RoleIds { get; set; } = new List<long>();
    }

  
}
