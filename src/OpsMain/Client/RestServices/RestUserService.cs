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
    public class RestUserService
    {
        HttpClient _httpClient;

        public RestUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SysUserDto>> SearchAsync(BasePage page, string searchStr)
        {
            var data = await _httpClient.GetDataAsync<List<SysUserDto>>(page, "api/user/search");
            return data;
        }
        public async Task<SysUserDto> EditAsync(BasePage page,string userName,List<long> roleIds)
        {
            var data =await _httpClient.PostDataAsync<SysUserDto>(page, "api/user/edit", new {UserName=userName, RoleIds =roleIds});
            return data;
        }
        public async Task<SysUserDto> GetByIdAsync(BasePage page,long id)
        {
            var data = await _httpClient.GetDataAsync<SysUserDto>(page, $"api/user/GetByIdWithExtend/{id}");
            return data;
        }

        public async Task<bool> ChangePwdAsync(BasePage page,SysUserDto dto)
        {
            var result = await _httpClient.PostDataAsync<bool>(page, "api/user/ChangePwd", dto);
            return result;
        }


    }
}
