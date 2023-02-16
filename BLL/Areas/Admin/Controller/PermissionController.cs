using BLL.Areas.Admin.Object;
using BLL.Controller;
using BLL.Filter;
using DAL.Component.Enum;
using DAL.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Controller
{
    [Route("api/")]
    [ApiController]
    [BeforeAction]
    [EnableCors("AllowSpecificOrigin")]
    public class PermissionController : OverallController
    {
        private readonly AppDbContext _context;
        public PermissionController(AppDbContext context) : base(context)
        {
            _context = context;
        }
        [HttpGet("permission/list")]
        public IActionResult List(int? id = null)
        {
            var data = new UserPermissionObject();
            data.SystemFunctions = _context.Function.ToList().GroupBy(x => new { x.Controller, x.Action, x.Name, x.Description })
                .Select(g => new PermissionObject { Controller = g.Key.Controller,Actions= g.ToList() }).ToList();
            data.UserPermissions = _context.PermissionView.AsQueryable().Where(x => x.RoleID == id).ToList();
            return Ok(new APIResult
            {
                Status = 1,
                Data = data
            });
        }
        [HttpGet("permission/changestatus")]
        public IActionResult ChangeStatus(int roleID = 0, int funcID = 0)
        {
            if (!_context.Role.Any(x => x.ID == roleID) || roleID == 0)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Vai trò không xác định"
                });
            }
            if(!_context.Function.Any(x => x.ID == funcID) || funcID == 0)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Chức năng không xác định"
                });
            }
            Role_Function permission = _context.Role_Function.FirstOrDefault(x => x.RoleID == roleID && funcID == x.FunctionID);
            if (permission == null)
            {
                permission = new Role_Function { RoleID = roleID, FunctionID = funcID, Status = EnumDefaultStatus.ACTIVE };
                _context.Role_Function.Add(permission);
            }
            else
            {
                if (permission.Status == EnumDefaultStatus.ACTIVE)
                {
                    permission.Status = EnumDefaultStatus.DELETED;
                }
                else
                {
                    permission.Status = EnumDefaultStatus.ACTIVE;
                }
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Phân quyền thành công"
            });
        }
    }
}
