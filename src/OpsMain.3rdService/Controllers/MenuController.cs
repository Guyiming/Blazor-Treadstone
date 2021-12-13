using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpsMain._3rdService.Contract;
using OpsMain._3rdService.Filter;
using OpsMain.Shared;
using OpsMain.StorageLayer;
using OpsMain.StorageLayer.Entity;
using OpsMain.StorageLayer.IService;

namespace OpsMain._3rdService._Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ResultPack]
    public class MenuController : ControllerBase, IControllerContract<SysMenuDto>
    {
        private readonly IMapper _mapper;
        private readonly CommonRepoService<SysMenu> _menuService;

        public MenuController(IMapper mapper, CommonRepoService<SysMenu> menuService)
        {
            _mapper = mapper;
            _menuService = menuService;
        }


        [HttpPost]
        public async Task<ActionResult<SysMenuDto>> CreateAsync([FromBody] SysMenuDto t)
        {
            if (IsExist(t.MenuName) != null)
            {
                return StatusCode(400, "菜单名称已存在");
            }
            var sysmenu = _mapper.Map<SysMenu>(t);
            await _menuService.CreateAsync(sysmenu);
            await _menuService.SaveChangesAsync();
            return _mapper.Map<SysMenuDto>(sysmenu);

        }

        [HttpPost]
        public async Task<ActionResult<SysMenuDto>> DeleteAsync([FromBody] List<long> ids)
        {
            var delEntities = _menuService.Query().Where(o => ids.Contains(o.Id));
            if (delEntities?.Count() > 0)
            {
                await _menuService.DeleteBatchAsync(delEntities);
                await _menuService.SaveChangesAsync();
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult<SysMenuDto>> EditAsync([FromBody] SysMenuDto t)
        {
            var item = _menuService.Query().FirstOrDefault(m => m.Id == t.Id);
            if (item == null)
            {
                return NotFound("菜单不存在,无法更新");
            }
            item.MenuName = t.MenuName;
            item.ParentId = t.ParentId;
            item.Icon = t.Icon;
            item.Href = t.Href;
            _menuService.Update(item);
            await _menuService.SaveChangesAsync();
            return _mapper.Map<SysMenuDto>(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SysMenuDto>> GetByIdWithExtendAsync(long id)
        {
            var item = await _menuService.GetByIdAsync(id);
            return _mapper.Map<SysMenuDto>(item);
        }

        [HttpGet("{parentId?}")]
        public async Task<ActionResult<List<SysMenuDto>>> GetByParentIdAsync(long? parentId)
        {
            var items = await _menuService.Query().Where(o => o.ParentId == parentId).ToListAsync();
            return _mapper.Map<List<SysMenuDto>>(items);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SysMenuDto>>> GetMenuTreeAsync()
        {
            var tree = await _menuService.Query().Where(o => o.ParentId == null).Include(o => o.SubMenus).OrderBy(o => o.OrderNo).ToListAsync();
            return _mapper.Map<List<SysMenuDto>>(tree);
        }

        [HttpGet]
        public ActionResult<IEnumerable<SysMenuDto>> Search(string whereFunc)
        {
            var data = _menuService.Query().ToList();
            return _mapper.Map<List<SysMenuDto>>(data);
        }

        private SysMenu IsExist(string menuName)
        {
            var menu = _menuService.Query().Where(o => o.MenuName == menuName).FirstOrDefault();
            return menu;
        }
    }
}
