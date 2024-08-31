using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;

namespace OpsMain._3rdService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HeartController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
    }
}
