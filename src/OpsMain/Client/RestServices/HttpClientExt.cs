using OpsMain.Client.Shared;
using OpsMain.Shared;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OpsMain.Client.Extensions
{
    public static class HttpClientExt
    {
        /// <summary>
        /// 获取接口返回数据
        /// </summary>
        /// <typeparam name="TReturn">可以是object，也可以是List<object></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="basePage"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns>data or default(TReturn)</returns>
        public static async Task<TReturn> PostDataAsync<TReturn>(this HttpClient httpClient, BasePage basePage, string url, object data)
        {
            var response = await httpClient.PostAsJsonAsync<object>(url, data);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BaseResultDto<TReturn>>();
                if (result.Success)
                {
                    return result.Data;
                }
                else
                {
                    basePage?.ShowNotice(AntDesign.NotificationType.Error, result.Message);
                }
            }
            else
            {
                basePage?.ShowNotice(AntDesign.NotificationType.Error, response.StatusCode.ToString());
            }
            return default(TReturn);
        }
        /// <summary>
        /// 获取数据-HttpGet
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="basePage"></param>
        /// <param name="url"></param>
        /// <returns>data or default(TReturn)</returns>
        public static async Task<TReturn> GetDataAsync<TReturn>(this HttpClient httpClient, BasePage basePage, string url)
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BaseResultDto<TReturn>>();
                if (result.Success)
                {
                    return result.Data;
                }
                else
                {
                    basePage?.ShowNotice(AntDesign.NotificationType.Error, result.Message);
                }
            }
            else
            {
                basePage?.ShowNotice(AntDesign.NotificationType.Error, response.StatusCode.ToString());
            }
            return default(TReturn);
        }
    }
}
