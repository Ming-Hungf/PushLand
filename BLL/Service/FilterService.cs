using DAL.Component.Enum;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class FilterService:IFilterService
    {
        private readonly AppDbContext _context;

        public FilterService(AppDbContext context)
        {
            _context = context;
        }
        public UserSession? GetSession(string token)
        {
            var session = _context.UserSession.FirstOrDefault(x => x.AccessToken == token && x.LoginResult == 1);
            return session;
        }
        
        public PermissionView? GetPermission(int roleid,string controller,string action)
        {
            var permission = _context.PermissionView.FirstOrDefault(x => x.RoleID == roleid && x.Controller.Equals(controller) && x.Action.Equals(action));
            return permission;
        }
    }
    public interface IFilterService
    {
        public UserSession? GetSession(string token);
        public PermissionView? GetPermission(int roleid, string controller, string action);
    }
}
