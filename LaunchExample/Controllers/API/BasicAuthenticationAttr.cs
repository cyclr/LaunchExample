using System;
using System.Configuration;
using System.Text;

namespace Cyclr.LaunchExample.Controllers.API
{
    /// <summary>
    /// Provide basic auth support for API action or controller.
    /// Username and password specified in web.config.
    /// </summary>
    public class BasicAuthentication : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken;
                try
                {
                    authToken = Encoding.UTF8.GetString(Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter));
                }
                catch(Exception)
                {
                    authToken = string.Empty;
                }

                var index = authToken.IndexOf(":");
                if (index > -1)
                {
                    string username = authToken.Substring(0, authToken.IndexOf(":"));
                    string password = authToken.Substring(authToken.IndexOf(":") + 1);

                    if (username.Equals(ConfigurationManager.AppSettings["ApiUsername"], StringComparison.OrdinalIgnoreCase) &&
                       password.Equals(ConfigurationManager.AppSettings["ApiPassword"]))
                    {
                        base.OnActionExecuting(actionContext);
                        return;
                    }
                }
            }
            
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}