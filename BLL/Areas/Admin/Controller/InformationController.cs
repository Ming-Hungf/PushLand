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
    public class InformationController : OverallController
    {
        private readonly AppDbContext _context;
        public InformationController(AppDbContext context) : base(context)
        {
            _context = context;
        }
        [BeforeAction]
        [HttpGet("information/details")]
        public IActionResult Details(int id = 0)
        {
            var infor = _context.Information.FirstOrDefault(x => x.ID == id);
            if (infor == null)
            {
                infor = new Information
                {
                    ID = 0
                };
            }
            var result = new InformationObject(infor);
            return Ok(new APIResult
            {
                Status = 1,
                Data = result
            });
        }
        [HttpGet("information/list")]
        public IActionResult List(string keyword="", int pageNumber = 1, int lineAmount = 10)
        {
            var data = _context.Information.AsQueryable().Where(x => x.Name.Contains(keyword) && x.Status == EnumDefaultStatus.ACTIVE).ToList();
            var count = data.Count;
            return Ok(new APIResult
            {
                Status = 1,
                Data = new ListObject
                {
                    TotalData = count,
                    TotalPage = count % lineAmount == 0 ? count / lineAmount : count / lineAmount + 1,
                    Data = data == null ? null : data.Skip((pageNumber - 1) * lineAmount).Take(lineAmount).ToList()
                }
            });
        }
        [BeforeAction]
        [HttpGet("information/delete")]
        public IActionResult Delete(int id = 0)
        {
            var infor = _context.Information.FirstOrDefault(x => x.ID == id);
            if(infor == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Thông tin báo giá không tồn tại"
                });
            }
            infor.Status = EnumDefaultStatus.DELETED;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xóa thông tin thành công"
            });
        }
        [BeforeAction]
        [HttpPost("information/update")]
        public IActionResult Update([FromForm]InformationObject obj)
        {
            var uid = GetCurrentSession().UserID;
            string message = "Cập nhật thành công";
            if (_context.Information.Any(x => x.Name.Equals(obj.Name)))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Thông tin báo giá đã tồn tại"
                });
            }
            var infor = _context.Information.FirstOrDefault(x => x.Status == EnumDefaultStatus.ACTIVE && x.ID == obj.ID);
            if (infor == null)
            {
                infor = new Information
                {
                    Created = DateTime.UtcNow,
                    UserCreated = uid,
                    Status = EnumDefaultStatus.ACTIVE
                };
                _context.Add(infor);
                message = "Tạo thông tin báo giá thành công";
            }
            infor.Updated = DateTime.UtcNow;
            infor.UserUpdated = uid;
            infor.Name = obj.Name;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = message
            });
        }
        
    }
}
