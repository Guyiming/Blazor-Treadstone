using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpsMain._3rdService.Contract;
using OpsMain._3rdService.Filter;
using OpsMain.Shared;
using OpsMain.Shared.Toolkit;
using OpsMain.StorageLayer;
using OpsMain.StorageLayer.Entity;
using OpsMain.StorageLayer.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OpsMain._3rdService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ResultPack]
    public class UserController : ControllerBase, IControllerContract<SysUserDto>
    {
        ILogger<UserController> _logger;
        IMapper _mapper;
        CommonRepoService<SysUser> _userServie;
        CommonRepoService<R_RoleUser> _roleUserService;
        public UserController(ILogger<UserController> logger, CommonRepoService<SysUser> userServie, CommonRepoService<R_RoleUser> roleUserService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _userServie = userServie;
            _roleUserService = roleUserService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<SysMenuDto>>> GetUserMenu(long userId)
        {
            var user = await _userServie.Query().Where(o => o.Id == userId).Include(o => o.R_RoleUsers).ThenInclude(o => o.Role).ThenInclude(r => r.R_RoleMenus).FirstOrDefaultAsync();//.GetByIdAsync(userId);
            var menus = user.R_RoleUsers.SelectMany(r => r.Role.R_RoleMenus).Select(o => o.Menu).Distinct();
            var dtoMens = _mapper.Map<List<SysMenuDto>>(menus);
            return dtoMens;
        }

        public record UserIn(string UserName, string Password);

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<SysUserDto> Login([FromBody] UserIn dto)
        {
            if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
            {
                return StatusCode(400, "用户名密码不能为空");
            }
            var user = GetByUserName(dto.UserName);
            if (user != null)
            {
                if (user.Password == dto.Password.ToSHA256())
                {
                    return _mapper.Map<SysUserDto>(user);
                }
            }

            return StatusCode(400, "用户名或密码错误");
        }

        [HttpGet]
        public ActionResult<IEnumerable<SysUserDto>> Search(string whereFunc)
        {
            //Console.WriteLine("--->" + HttpContext.User.Identity.IsAuthenticated + "----" + HttpContext.User.Identity.Name);
            //HttpContext.User.Claims.ToList().ForEach(a => Console.WriteLine($"---->{a.Type}:{a.Value}"));

            var users = _userServie.Query().Include(o => o.R_RoleUsers).ThenInclude(re => re.Role).ToList();
            var data = new List<SysUserDto>();
            users.ForEach(u =>
            {
                var dto = _mapper.Map<SysUserDto>(u);
                dto.RoleNames = u.R_RoleUsers.Select(o => o.Role.RoleName).ToList();
                dto.RoleIds = u.R_RoleUsers.Select(o => o.Role.Id).ToList();
                data.Add(dto);
            });
            return data;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SysUserDto>> GetByIdWithExtendAsync(long id)
        {
            var user = await _userServie.Query($"Id={id}").Include(o => o.R_RoleUsers).ThenInclude(re => re.Role).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("未找到对应的用户");
            }
            var dto = _mapper.Map<SysUserDto>(user);
            dto.RoleNames = user.R_RoleUsers.Select(o => o.Role.RoleName).ToList();
            dto.RoleIds = user.R_RoleUsers.Select(o => o.Role.Id).ToList();
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<SysUserDto>> CreateAsync([FromBody] SysUserDto t)
        {
            var user = _mapper.Map<SysUser>(t);
            var existUser = GetByUserName(t.UserName);
            if (existUser != null)
            {
                return StatusCode(400, "用户已存在");
            }
            else
            {
                user.Password = user.Password.ToSHA256();
                await _userServie.CreateAsync(user);
                await _userServie.SaveChangesAsync();
                return _mapper.Map<SysUserDto>(user);

            }
        }
        [HttpPost]
        public async Task<ActionResult<SysUserDto>> EditAsync([FromBody] SysUserDto t)
        {
            //todo获取当前登录用户
            var user = GetByUserName(t.UserName);
            if (user != null)
            {
                await _roleUserService.Query($"UserId=={user.Id}").DeleteBatchAsync();
                await _roleUserService.CreateBulkyAsync(t.RoleIds.Select(o => new R_RoleUser { RoleId = o, UserId = user.Id }));
            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> ChangePwdAsync([FromBody] SysUserDto t)
        {
            //todo获取当前登录用户
            var user = GetByUserName(t.UserName);
            if (user != null&&user.Password==t.Password.ToSHA256())
            {
                user.Password = t.NewPassword.ToSHA256();
                _userServie.Update(user);
                await _userServie.SaveChangesAsync();
                return true;
            }
            return StatusCode(400,"用户名或密码错误");
        }

        [HttpPost]
        public async Task<ActionResult<SysUserDto>> DeleteAsync([FromBody] List<long> ids)
        {
            await _userServie.Query().Where(o => ids.Contains(o.Id)).DeleteBatchAsync();
            return null;
        }

        private SysUser GetByUserName(string userName)
        {
            var user = _userServie.Query().FirstOrDefault(u => u.UserName == userName);
            return user;
        }
    }
}
