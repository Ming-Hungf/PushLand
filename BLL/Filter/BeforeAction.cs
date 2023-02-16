using DAL.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using BLL.Service;
using Microsoft.AspNetCore.Http;

namespace BLL.Filter
{
    public class BeforeAction: Attribute,Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            FilterService service = (FilterService)context.HttpContext.RequestServices.GetService(typeof(IFilterService));
            if (context.HttpContext.Request.Headers["AccessToken"].ToString().Equals(""))
            {
                string accessToken = context.HttpContext.Request.Headers["AccessToken"].ToString();
                if (!accessToken.Equals(""))
                {
                    var session = service.GetSession( accessToken);
                    if (session != null&&session.Expire<DateTime.UtcNow)
                    {
                        if (session.RoleID == 1) return;
                        string action = context.RouteData.Values["action"].ToString();
                        string controller = context.RouteData.Values["controller"].ToString();
                        var permission = service.GetPermission(session.RoleID, controller, action);
                        if (permission != null)
                        {
                            return;
                        }

                    }
                }
            }
            context.HttpContext.Response.StatusCode = 200;
            var result = new APIResult()
            {
                Status = 0,
                Data = null,
                Message = "Không đủ quyền truy cập"
            };
            context.Result = new ObjectResult(result);

        }
    }
}
