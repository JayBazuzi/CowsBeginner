using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ServerTrack.Web
{
    public class CreateErrorResponseIfUnsetActionArgumentsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var unsetActionArgumentNames = actionContext.ActionArguments
                .Where(_ => _.Value == null)
                .Select(_ => _.Key)
                .ToList();
            if (unsetActionArgumentNames.Any())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "unset action arguments: " +
                    string.Join(", ", unsetActionArgumentNames));
            }
        }
    }
}