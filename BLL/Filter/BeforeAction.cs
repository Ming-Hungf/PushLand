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
            string accessToken="";
            
            if (context.HttpContext.Request.Cookies["Remember"] != null)
            {
                var remember = context.HttpContext.Request.Cookies["Remember"];
                if (remember.ToString().Equals("0"))
                {
                    accessToken = context.HttpContext.Session.GetString("AccessToken");
                }
                else
                {
                    accessToken = context.HttpContext.Request.Cookies["AccessToken"].ToString();
                }
                if (!accessToken.Equals(""))
                {
                    var session = service.GetSession( accessToken);
                    if (session != null)
                    {
                        var user = service.GetUser(session.UserID);
                        if (user != null)
                        {   
                            if (user.IsAdmin==1) return;
                            string action = context.RouteData.Values["action"].ToString();
                            string controller = context.RouteData.Values["controller"].ToString();
                            var permission = service.GetPermission(user.RoleID, controller, action);
                            if (permission != null)
                            {
                                return;

                            }
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
