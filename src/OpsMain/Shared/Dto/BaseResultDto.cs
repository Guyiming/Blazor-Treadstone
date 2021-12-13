using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.Shared
{
    /// <summary>
    /// 返回数据，默认success为true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResultDto<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 错误消息/额外消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

    }
}
