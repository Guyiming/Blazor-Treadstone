using OpsMain.Client.Extensions;
using OpsMain.Client.Shared;
using OpsMain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpsMain.Client.RestServices
{
    public class RestMenuService
    {
        HttpClient _httpClient;

        public RestMenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SysMenuDto>> GetByParentIdAsync(BasePage page,long? parentId)
        {
            var result = await _httpClient.GetDataAsync<List<SysMenuDto>>(page, $"api/menu/GetByParentId/{parentId}");
            return result;
        }

        public async Task<SysMenuDto> DeleteAsync(BasePage page,List<string> ids)
        {
            var result = await _httpClient.PostDataAsync<SysMenuDto>(page, "api/menu/delete", ids);
            return result;
        }

        public async Task<SysMenuDto> EditAsync(BasePage page,SysMenuDto menu)
        {
            var editMenu = await _httpClient.PostDataAsync<SysMenuDto>(page, "api/menu/edit", menu);
            return editMenu;
        }

        public async Task<SysMenuDto> CreateAsync(BasePage page,SysMenuDto menu)
        {
            var newMenu = await _httpClient.PostDataAsync<SysMenuDto>(page, "api/menu/create", menu);
            return newMenu;
        }

       
    }
}
