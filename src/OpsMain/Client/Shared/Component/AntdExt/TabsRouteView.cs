using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpsMain.Client.Shared.Component.AntdExt
{
    public class TabsRouteView : RouteView
    {
        protected override void Render(RenderTreeBuilder builder)
        {
      
            base.Render(builder);

     
            builder.Clear();

            var layoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType ?? DefaultLayout;

            var body = CreateBody(RouteData, Navmgr.Uri);

            builder.OpenComponent<CascadingValue<RouteView>>(0);
            builder.AddAttribute(1, "Name", "RouteView");
            builder.AddAttribute(2, "Value", this);


            builder.AddAttribute(3, "ChildContent", (RenderFragment)(b =>
            {
                b.OpenComponent(20, layoutType);
                b.AddAttribute(21, "Body", body);
                b.CloseComponent();
            }));


            builder.CloseComponent();
        }

        private RenderFragment CreateBody(RouteData routeData, string url)
        {
            return builder =>
            {
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }

                //builder.AddComponentReferenceCapture(2, @ref =>
                //{
                //    GetPageInfo(_pageMap[url], routeData.PageType, url, @ref);
                //});

                builder.CloseComponent();
            };
        }

        private void GetPageInfo(ReuseTabsPageItem pageItem, Type pageType, string url, object page)
        {
            if (page is IReuseTabsPage resuse)
            {
                pageItem.Title ??= resuse.GetPageTitle();
            }

            var attributes = pageType.GetCustomAttributes(true);

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageTitleAttribute) is ReuseTabsPageTitleAttribute titleAttr && titleAttr != null)
            {
                pageItem.Title ??= titleAttr.Title?.ToRenderFragment();
            }

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageAttribute) is ReuseTabsPageAttribute attr && attr != null)
            {
                pageItem.Title ??= attr.Title?.ToRenderFragment();
                pageItem.Ignore = attr.Ignore;
                pageItem.Closable = attr.Closable;
            }

            pageItem.Title ??= new Uri(url).PathAndQuery.ToRenderFragment();
        }
    }
}
