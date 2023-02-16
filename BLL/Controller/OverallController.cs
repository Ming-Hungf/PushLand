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
        
        [NonAction]
        public UserSession GetCurrentSession()
        {
            if (Request.Headers.Any(x => x.Key.Equals("AccessToken")))
            {
                var token = Request.Headers["AccessToken"];
                UserSession session = _context.UserSession.AsQueryable().Where(x => x.AccessToken.Equals(token) && x.Status == 1).FirstOrDefault();
                if (session != null&&session.Expire<DateTime.UtcNow)
                {
                    return session;
                }
            }
            return null;
            
        }
    }
}
