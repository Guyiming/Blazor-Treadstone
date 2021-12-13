using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.Client.Pages
{
    public partial class SystemCtl
    {
        string ServiceName { get; set; }
        string ServiceDescription { get; set; }
        string WorkingDirectory { get; set; }
        string ExecCmd { get; set; }

        string GenerateResult { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            base.Title = "SystemCtl";
        }
        private void GenerateService()
        {
            GenerateResult = @$"
#基本信息：描述、启动顺序，启动依赖等
[Unit]
Description={ServiceDescription}
#有网之后再启动
After=network.target  
#Wants=network-online.target

#运行行为：启动命令、默认目录等
[Service]
WorkingDirectory={WorkingDirectory}
ExecStart={ExecCmd}
#ExecStop=kill -9 'cat /tmp/signalriot.pid'

#定义如何安装这个配置文件，即怎样做到开机启动
[Install]
#表示该服务所在的 Target
WantedBy=multi-user.target

";

        }
    }
}
