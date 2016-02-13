using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Client.Backoffice.Controllers.Api
{
    public class ErrorLogOperationsController : BackofficeOperationsController
    {
        private readonly IErrorLogManager _manager;

        public ErrorLogOperationsController(IErrorLogManager manager)
        {
            _manager = manager;
        }

        [Route("api/backoffice/errorlogs/clearlogs/")]
        [HttpPost]
        public async Task<IActionResult> ClaimsForRoles([FromBody]IEnumerable<string> ids)
        {
            await _manager.ClearLogs(ids);
            return Ok();
        }
    }
}