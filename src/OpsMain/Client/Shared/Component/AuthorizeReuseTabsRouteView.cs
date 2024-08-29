using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using OpsMain.Client.Pages;

namespace OpsMain.Client.Shared.Component
{
    public sealed class AuthorizeReuseTabsRouteView : RouteView
    {
        private sealed class AuthorizeRouteViewCore : AuthorizeViewCore
        {
            [Parameter]
            public RouteData RouteData { get; set; } = default!;

            protected override IAuthorizeData[] GetAuthorizeData()
                => AttributeAuthorizeDataCache.GetAuthorizeDataForType(RouteData.PageType);
        }

        private static readonly RenderFragment<AuthenticationState> _defaultNotAuthorizedContent
            = state => builder => builder.AddContent(0, "Not authorized");
        private static readonly RenderFragment _defaultAuthorizingContent
            = builder => builder.AddContent(0, "Authorizing...");

        private readonly RenderFragment _renderAuthorizeRouteViewCoreDelegate;
        private readonly RenderFragment<AuthenticationState> _renderAuthorizedDelegate;
        private readonly RenderFragment<AuthenticationState> _renderNotAuthorizedDelegate;
        private readonly RenderFragment _renderAuthorizingDelegate;

        /// <summary>
        /// Initialize a new instance of a <see cref="AuthorizeRouteView"/>.
        /// </summary>
        public AuthorizeReuseTabsRouteView()
        {
            RenderFragment renderBaseRouteViewDelegate = builder => base.Render(builder);
            _renderAuthorizedDelegate = authenticateState => renderBaseRouteViewDelegate;
            _renderNotAuthorizedDelegate = authenticationState => builder => RenderNotAuthorizedInDefaultLayout(builder, authenticationState);
            _renderAuthorizingDelegate = RenderAuthorizingInDefaultLayout;
            _renderAuthorizeRouteViewCoreDelegate = RenderAuthorizeRouteViewCore;
        }

        /// <summary>
        /// The content that will be displayed if the user is not authorized.
        /// </summary>
        [Parameter]
        public RenderFragment<AuthenticationState> NotAuthorized { get; set; }

        /// <summary>
        /// The content that will be displayed while asynchronous authorization is in progress.
        /// </summary>
        [Parameter]
        public RenderFragment Authorizing { get; set; }

        /// <summary>
        /// The resource to which access is being controlled.
        /// </summary>
        [Parameter]
        public object Resource { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> ExistingCascadedAuthenticationState { get; set; }


        [Parameter]
        public Type AuthorizingLayout { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            if (ExistingCascadedAuthenticationState != null)
            {
                _renderAuthorizeRouteViewCoreDelegate(builder);
            }
            else
            {
                builder.OpenComponent<CascadingAuthenticationState>(0);
                builder.AddAttribute(1, nameof(CascadingAuthenticationState.ChildContent), _renderAuthorizeRouteViewCoreDelegate);
                builder.CloseComponent();
            }
        }

        private void RenderAuthorizeRouteViewCore(RenderTreeBuilder builder)
        {
            builder.OpenComponent<AuthorizeRouteViewCore>(0);
            builder.AddAttribute(1, nameof(AuthorizeRouteViewCore.RouteData), RouteData);
            builder.AddAttribute(2, nameof(AuthorizeRouteViewCore.Authorized), _renderAuthorizedDelegate);//调用base的render进行渲染
            builder.AddAttribute(3, nameof(AuthorizeRouteViewCore.Authorizing), _renderAuthorizingDelegate);
            builder.AddAttribute(4, nameof(AuthorizeRouteViewCore.NotAuthorized), _renderNotAuthorizedDelegate);
            builder.AddAttribute(5, nameof(AuthorizeRouteViewCore.Resource), Resource);
            builder.CloseComponent();
        }

        private void RenderContentInDefaultLayout(RenderTreeBuilder builder, RenderFragment content)
        {
            builder.OpenComponent<EmptyPage>(0);
            builder.AddAttribute(1, nameof(EmptyPage.ChildContent), content);
            builder.CloseComponent();
        }

        private void RenderNotAuthorizedInDefaultLayout(RenderTreeBuilder builder, AuthenticationState authenticationState)
        {
            var content = NotAuthorized ?? _defaultNotAuthorizedContent;
            RenderContentInDefaultLayout(builder, content(authenticationState));
        }

        private void RenderAuthorizingInDefaultLayout(RenderTreeBuilder builder)
        {
            var content = Authorizing ?? _defaultAuthorizingContent;
            RenderContentInDefaultLayout(builder, content);
        }


    }
}
