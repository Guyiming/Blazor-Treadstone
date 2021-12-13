using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OpsMain.Client.RestServices;
using OpsMain.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpsMain.Client.Shared
{
    public class BasePage : ComponentBase, IReuseTabsPage
    {
        [Inject]
        public NotificationService NotifiService { get; set; }

        [Inject]
        public IHttpClientFactory ClientFactory { get; set; }

        [Inject]
        public ModalService DialogService { get; set; }

        [Inject]
        public RestUserService UserService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public string Title { get; set; }

        protected SysUserDto CurrentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (CurrentUser == null)
            {
                CurrentUser = await GetLoginUserInfoAsync();
            }
        }

        protected async Task<SysUserDto> GetLoginUserInfoAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                long.TryParse(user.FindFirst("sub")?.Value, out long userId);
                if (userId > 0)
                {
                    //请求接口获取登录用户信息
                    var result = await UserService.GetByIdAsync(this, userId);
                    if (result == null)
                    {
                        this.ShowNotice(NotificationType.Error, $"ERROR:未找到ID={userId}的用户");
                    }
                    return result;
                }
            }
            return null;
        }




        #region Utils

        /// <summary>
        /// 右上角显示消息提醒，自动消失
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="msg">内容</param>
        /// <param name="title">标题</param>
        public async void ShowNotice(NotificationType type, string msg, string title = null)
        {
            await NotifiService.Open(new NotificationConfig
            {
                NotificationType = type,
                Message = title,
                Description = msg,
                Placement = NotificationPlacement.TopRight
            });
        }

        /// <summary>
        /// 显示确定/取消对话框
        /// </summary>
        /// <param name="msg">内容</param>
        /// <param name="title">标题</param>
        /// <param name="isDelete">Ok按钮是否标红突出</param>
        /// <param name="onOk">点击Ok回调</param>
        /// <param name="onCancel">点击Cancel回调</param>
        public void ShowOkCancelDialog(string msg, string title = null, bool isDelete = false, Func<ModalClosingEventArgs, Task> onOk = null, Func<ModalClosingEventArgs, Task> onCancel = null)
        {
            var opt = new ConfirmOptions()
            {
                Title = title,
                Content = msg,
                OnOk = onOk,
                OnCancel = onCancel,
            };

            RenderFragment icon = builder =>
            {
                builder.OpenComponent<Icon>(0);
                builder.AddAttribute(1, "Type", "exclamation-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };

            if (isDelete)
            {
                opt.OkType = "danger";
                opt.Icon = icon;
            }

            DialogService.Confirm(opt);
        }

        public RenderFragment GetPageTitle()
        {
            return builder =>
            {
                builder.AddContent(0, Title);
            };
        }

        #endregion Utils
    }
}