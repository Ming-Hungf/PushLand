using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OverallController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OverallController(AppDbContext context)
        {
            _context = context;
        }
        private bool Remember(string str)
        {
            return str.Equals("1");
        }
        [NonAction]
        public UserSession GetCurrentSession()
        {
            if ( Request.Cookies["Remember"] == null)
            {
                return null;
            }
            var remember = Request.Cookies["Remember"].ToString();
            string token = "";
            if (Remember(remember))
            {
                token = Request.Cookies["AccessToken"];
            }
            else
            {
                token = HttpContext.Session.GetString("AccessToken");
            }
            UserSession session = _context.UserSession.Where(x =>  x.AccessToken.Equals(token)&&x.Status==1).FirstOrDefault();
            if (session == null)
            {
                if (Remember(remember))
                {
                    Response.Cookies.Delete("AccessToken");
                }
                else
                {
                    HttpContext.Session.Remove("AccessToken");
                }
                return null;
            }
            
            return session;
        }
    }
}
