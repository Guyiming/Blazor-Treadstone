using Microsoft.AspNetCore.Mvc;
using OpsMain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain._3rdService.Contract
{
    interface IControllerContract<T>
    {
        [HttpGet]
        ActionResult<IEnumerable<T>> Search(string whereFunc);

        [HttpGet("{id}")]
        Task<ActionResult<T>> GetByIdWithExtendAsync(long id);


        [HttpPost]
        Task<ActionResult<T>> CreateAsync([FromBody] T t);


        [HttpPost]
        Task<ActionResult<T>> EditAsync([FromBody] T t);


        [HttpPost]
        Task<ActionResult<T>> DeleteAsync([FromBody] List<long> ids);

    }
}
