using OpsMain.Client.Extensions;
using OpsMain.Client.Shared;
using OpsMain.Shared;
using OpsMain.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpsMain.Client.RestServices
{
    public class RestRoleService
    {
        HttpClient _httpClient;
        public RestRoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SysRoleDto>> SearchAsync(BasePage basePage,string searchString)
        {
            var data=await _httpClient.PostDataAsync<List<SysRoleDto>>(basePage, "api/role/search", searchString);
            return data;
        }

        public async Task<SysRoleDto> CreatedAsync(BasePage basePage,string roleName,string[] menuIds)
        {
            SysRoleDto result = await _httpClient.PostDataAsync<SysRoleDto>(basePage, "api/role/create", new SysRoleDto { RoleName =roleName, Menus = menuIds.Select(o => new SysMenuDto { Id = Convert.ToInt64(o) }) });

            return result;
        }
        public async Task<SysRoleDto> EditAsync(BasePage page,SysRoleDto dto)
        {
            var role = await _httpClient.PostDataAsync<SysRoleDto>(page, "api/role/edit", dto);
            return role;
        }

        public async Task<SysRoleDto> DeleteAsync(BasePage page,List<long> ids)
        {
           var result= await _httpClient.PostDataAsync<SysRoleDto>(page, "api/role/delete", ids);
            return result;
        }
    }
}
