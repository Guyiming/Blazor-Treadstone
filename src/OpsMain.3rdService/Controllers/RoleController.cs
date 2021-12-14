using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpsMain._3rdService.Contract;
using OpsMain._3rdService.Filter;
using OpsMain.Shared;
using OpsMain.Shared.Dto;
using OpsMain.StorageLayer.Entity;
using OpsMain.StorageLayer.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OpsMain._3rdService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ResultPack]
    public class RoleController : ControllerBase, IControllerContract<SysRoleDto>
    {
        IMapper _mapper;
        CommonRepoService<SysRole> _roleService;
        CommonRepoService<R_RoleMenu> _roleMenuSrv;

        public RoleController(IMapper mapper, CommonRepoService<SysRole> roleService, CommonRepoService<R_RoleMenu> roleMenuSrv)
        {
            _mapper = mapper;
            _roleService = roleService;
            _roleMenuSrv = roleMenuSrv;
        }

        [HttpPost]
        public async Task<ActionResult<SysRoleDto>> CreateAsync([FromBody] SysRoleDto t)
        {
            var isExist = _roleService.Query().FirstOrDefault(o => o.RoleName == t.RoleName) != null;
            if (isExist)
            {
                return BadRequest("该角色已存在");
            }

            SysRole sr = new SysRole()
            {
                RoleName = t.RoleName
            };
            await _roleService.CreateAsync(sr);
            await _roleService.SaveChangesAsync();
            await _roleMenuSrv.CreateAsync(t.Menus.Select(o => new R_RoleMenu { MenuId = o.Id, RoleId = sr.Id }));

            await _roleMenuSrv.SaveChangesAsync();


            return _mapper.Map<SysRoleDto>(sr);
        }

        [HttpPost]
        public async Task<ActionResult<SysRoleDto>> DeleteAsync([FromBody] List<long> ids)
        {
            await _roleService.Query().Where(o => ids.Contains(o.Id)).DeleteBatchAsync();
            await _roleMenuSrv.Query().Where(o => ids.Contains(o.RoleId)).DeleteBatchAsync();

            return null;
        }

        [HttpPost]
        public async Task<ActionResult<SysRoleDto>> EditAsync([FromBody] SysRoleDto t)
        {
            var tRole = await _roleService.GetByIdAsync(t.Id);
            if (tRole == null)
            {
                return NotFound("该角色不存在");
            }
            tRole.RoleName = t.RoleName;
            _roleService.Update(tRole);
            await _roleMenuSrv.Query(null).Where(o => o.RoleId == tRole.Id).DeleteBatchAsync();
            await _roleMenuSrv.CreateAsync(t.Menus.Select(m => new R_RoleMenu { MenuId = m.Id, RoleId = tRole.Id }));
            await _roleService.SaveChangesAsync();
            return _mapper.Map<SysRoleDto>(tRole);

        }

        /// <summary>
        /// 搜索角色，附带返回对应的菜单
        /// </summary>
        /// <param name="predicate">搜索表达式，用法：City=="Paris"&amp;&amp;Age>50</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<IEnumerable<SysRoleDto>> Search([FromBody]string predicate=null)
        {
            List<SysRoleDto> roles = new List<SysRoleDto>();
            var data = _roleService.Query(predicate).Include(o => o.R_RoleMenus).ThenInclude(o => o.Menu).Select(o=>new SysRoleDto { RoleName=o.RoleName, Id=o.Id, CreateTime=o.CreateTime, UpdateTime=o.UpdateTime, Menus=_mapper.Map<List<SysMenuDto>>(o.R_RoleMenus.Select(r=>r.Menu))}).ToList();
            return data;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SysRoleDto>> GetByIdWithExtendAsync(long id)
        {
            return _mapper.Map<SysRoleDto>(await _roleService.GetByIdAsync(id));
        }


    }
}
