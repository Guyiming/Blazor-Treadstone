using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.Client.Extensions
{
    public static class ConstantString
    {
        public const string User_Create="api/user/create";
        public const string User_Edit = "api/user/edit";
        public const string User_Delete = "api/user/delete";
        public const string User_GetUserMenu = "api/user/getusermenu";
        public const string User_Login = "api/user/login";
        public const string User_GetById = "api/user/getbyid";


        public const string Role_GetById = "api/role/getbyid";

       
        public const string Menu_Search = "api/menu/search";
        public const string Menu_GetById = "api/menu/getbyid";

    }
    public static class PolicyString
    {
        public const string MenuAuthorize = "MenuAuthorize";
    }
}
