[English](README.zh-cn.md) | 中文
# Blazor-Treadstone
一个基于Blazor和IdentityServer的前后端分离的快速开发框架，采用了RBAC（Role-Based Acccess Control）的权限控制方式。

有了**Treadston**(垫脚石)，你就站在了巨人的肩膀上，可以在此基础上快速构建属于你自己的系统。

技术栈：
- 前端：Blazor WASM + Ant Design Blazor
- 后端：ASP.NET Core 5.0
- 数据库：SQL SERVER 2019 + Entity Framework 5.0
- 认证与授权：IdentityServer


# 1. 项目结构
```
├─Blazor
│  ├─OpsMain.Client                 //WASM Client
│  │  
│  └─OpsMain.Server                 //WASM Host
│
├─OpsMain.3rdService                //Backend Service
│
├─OpsMain.IdsAuthentication         //IdentityServer
│
├─OpsMain.Shared                    //Shared Model
│
├─OpsMain.StorageLayer              //Entity Framework

```


# 2. 使用方式
## 2.1 创建数据库和初始化数据
直接运行项目 **OpsMain.IdsAuthentication** 和 **OpsMain.3rdService** 即可。

初始化完成之后，默认的用户密码为`admin`和`admin`。

## 2.2 调试运行

分别启动以下三个项目即可：
- OpsMain.IdsAuthentication
- OpsMain.3rdService
- OpsMain.Server

运行截图：

![Screenshot](screenshot.gif)







