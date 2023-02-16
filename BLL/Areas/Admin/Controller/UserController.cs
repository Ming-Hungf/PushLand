using BLL.Areas.Admin.Object;
using BLL.Component.Object;
using BLL.Controller;
using BLL.Filter;
using DAL.Component.Enum;
using DAL.Model;
using DAL.Plugins;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
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
    public class UserController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(AppDbContext context,IWebHostEnvironment env) : base(context)
        {
            _context = context;
            _webHostEnvironment = env;
        }
        [HttpGet("user/details")]
        public IActionResult Details(int id =0)
        {
            var user = _context.User.FirstOrDefault(x => x.ID == id);
            if (user == null)
            {
                user = new User
                {
                    ID = 0
                };
            }
            var result = new UserObject(user);
            return Ok(new APIResult
            {
                Status = 1,
                Data = result
            });
        }
        [HttpGet("user/list")]
        public IActionResult List(string keyword="", int pageNumber=1,int lineAmount = 10)
        {
            var users = _context.User.Where(x => x.Status == EnumDefaultStatus.ACTIVE && (x.Email.Contains(keyword) || x.Fullname.Contains(keyword))).ToList();
            var data = new List<UserViewModel>();
            foreach(var item in users)
            {
                var obj = new UserViewModel(item);
                data.Add(obj);
            }
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
                Data = result,
            });
        }
        [HttpGet("user/delete")]
        public IActionResult Delete(int id = 0)
        {
            var user = _context.User.FirstOrDefault(x => x.ID == id && x.Status == EnumDefaultStatus.ACTIVE);
            if (user == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Không thấy user cần tìm"
                });
            }
            user.Status = EnumDefaultStatus.DELETED;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xóa thành công"
            });
        }
        [HttpPost("user/update")]
        public IActionResult Update([FromForm]UserObject obj)
        {
            User user = _context.User.FirstOrDefault(x => x.ID == obj.ID);
            var uid = GetCurrentSession().UserID;
            if (user == null)
            {
                user = new User
                {
                    IsAdmin = 0,
                    Created = DateTime.UtcNow,
                    UserCreated = uid,
                    Status = EnumDefaultStatus.ACTIVE,
                    RoleID = 2
                };
            }
            if (_context.User.Any(x => (x.Phone.Equals(obj.Phone)  || x.Email.Equals(obj.Email))&&x.ID!=obj.ID))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Số điện thoại hoặc Email đã tồn tại"
                });
            }
            user.Fullname = obj.Fullname;
            user.Phone = obj.Phone;
            user.Sex = obj.Sex;
            user.Email = obj.Email;
            user.Birth = obj.Birth;
            user.RoleID = obj.RoleID;
            if (obj.File != null&&obj.Image.Equals(user.Image))
            {
                user.Image = FileExtension.Upload(obj.File, _webHostEnvironment.WebRootPath, "Image");
            }
            user.Updated = DateTime.UtcNow;
            user.UserUpdated = uid;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = " Cập nhật thành công",
            });

        }
    }
}
