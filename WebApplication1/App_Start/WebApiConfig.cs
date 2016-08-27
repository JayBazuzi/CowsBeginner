using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication1.Controllers;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ValidateModelAttribute());
        }
    }
}
