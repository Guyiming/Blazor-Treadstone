using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpsMain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain._3rdService.Filter
{
    public class ResultPackAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = new BaseResultDto<object>();
            if (context.Result is ObjectResult re)
            {
                if (re.StatusCode >= 300)
                {
                    result.Success = false;
                    result.Message = $"Request Error: [{re.StatusCode}]--{re.Value}";
                }
                else if (re.StatusCode==null||re.StatusCode>=200)
                {
                    result.Data = re.Value;
                }
            }
            else
            {
                result.Success = false;
                result.Message = $"Server Error: [context.Result is not ObjectResult]";
            }
            context.Result = new JsonResult(result);
        }
    }
}
