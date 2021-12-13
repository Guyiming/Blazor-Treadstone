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
    public class RestFreeService
    {
        HttpClient _httpClient;

        public RestFreeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SysMenuDto>> GetMenuTreeAsync(BasePage page)
        {
            var allMenus = await _httpClient.GetDataAsync<List<SysMenuDto>>(page, "api/menu/getmenutree");
            return allMenus;
        }

    }
}
