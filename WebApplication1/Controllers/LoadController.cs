using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Swashbuckle.Swagger.Annotations;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Load")]
    public class LoadController : ApiController
    {
        public class CpuAndRamLoad
        {
            [Required]
            public double? CpuLoad { get; set; }

            [Required]
            public double? RamLoad { get; set; }
        }

        [HttpPost]
        [Route("{serverName}")]
        public void Post(string serverName, [FromBody] CpuAndRamLoad value)
        {
        }
    }

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var unsetActionArgumentNames = actionContext.ActionArguments
                .Where(_ => _.Value == null)
                .Select(_ => _.Key);
            if (unsetActionArgumentNames.Any())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "unset action arguments: " + string.Join(", ", unsetActionArgumentNames));
                return;
            }

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    actionContext.ModelState);
                return;
            }
        }
    }
}