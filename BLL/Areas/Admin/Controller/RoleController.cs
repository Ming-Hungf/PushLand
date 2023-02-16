
using BLL.Areas.Admin.Object;
using BLL.Component.Object;
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
    [EnableCors("AllowSpecificOrigin")]
    [BeforeAction]
    public class RoleController : OverallController
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context):base(context)
        {
            _context = context;
        }
        [HttpGet("role/details")]
        public IActionResult Details(int id = 0)
        {
            var role = _context.Role.FirstOrDefault(x => x.ID == id);
            if(role == null)
            {
                role = new Role { ID = 0 };
            }
            var result = new RoleObject(role);
            return Ok(new APIResult
            {
                Status = 1,
                Data = result
            });
        }
        [HttpGet("role/list")]
        public IActionResult List(string keyword = "", int pageNumber = 1, int lineAmount = 10)
        {
            var data = _context.Role.Where(x => x.Name.Contains(keyword)&&x.Status == EnumDefaultStatus.ACTIVE).ToList();
            var count = data.Count;
            var result = new ListObject
            {
                TotalData = count,
                TotalPage = count % lineAmount == 0 ? count / lineAmount : count / lineAmount + 1,
                Data = data == null ? null : data.Skip((pageNumber - 1) * lineAmount).Take(lineAmount).ToList()
            };
            return Ok(new APIResult
            {
                Status = 1,
                Data = result
            });
        }
        [HttpPost("role/update")]
        public IActionResult Update([FromForm] Role model)
        {
            if (model.Name == null||model.Name.Length==0||_context.Role.Any(x=>x.Name.Equals(model.Name)&&x.Status==EnumDefaultStatus.ACTIVE))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Tên không hợp lệ hoặc đã tồn tại"
                });
            }
            string message;
            var role = _context.Role.FirstOrDefault(x => x.ID == model.ID);
            if (role == null)
            {
                role = new Role
                {
                    Name = model.Name,
                    Status = EnumDefaultStatus.ACTIVE
                };
                _context.Add(role);
                message = "Tạo vai trò thành công";
            }
            else
            {
                role.Name = model.Name;
                message = "Cập nhật vai trò thành công";
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = message
            });
        }
        [HttpGet("role/delete")]
        public IActionResult Delete([FromQuery]int id)
        {
            var role = _context.Role.FirstOrDefault(x => x.ID == id);
            if (role == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Không tìm thấy vai trò cần xóa"
                });
            }
            role.Status = EnumDefaultStatus.DELETED;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xóa thành công"
            });
        }
    }
}
