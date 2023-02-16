using BLL.Component.Object;
using BLL.Filter;
using DAL.Component.Enum;
using DAL.Model;
using DAL.Plugins;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Controller
{
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [BeforeAction]
    public class AccountController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(AppDbContext context, IWebHostEnvironment env) : base(context)
        {
            _context = context;
            _webHostEnvironment = env;
        }
        [HttpGet("account/details")]
        public IActionResult Details()
        {
            var session = GetCurrentSession();
            if (session != null)
            {
                var userid = session.UserID;
                var user = _context.User.FirstOrDefault(x => x.ID == userid && x.Status==EnumDefaultStatus.ACTIVE);
                if (user != null)
                {
                    var account = new AccountObject(user);
                    return Ok(new APIResult
                    {
                        Status = 1,
                        Data = account
                    });
                }
            }
            return Ok(new APIResult
            {
                Status = -1,
                Message = "Người dùng không tồn tại"
            });
            
        }
        [HttpPost("account/update")]
        public IActionResult Update([FromForm]AccountObject account)
        {
            var session = GetCurrentSession();
            if (session != null)
            {
                var user = _context.User.FirstOrDefault(x => x.ID == session.UserID && x.Status == EnumDefaultStatus.ACTIVE);
                if (user != null)
                {
                    user.Fullname = account.Fullname;
                    user.Phone = account.Phone;
                    user.Email = account.Email;
                    user.Birth = account.Birth;
                    user.Sex = account.Sex;
                    if (account.Image != user.Image && account.File != null)
                    {
                        user.Image = FileExtension.Upload(account.File, _webHostEnvironment.WebRootPath, "Account Image");
                    }
                    _context.SaveChanges();
                    return Ok(new APIResult
                    {
                        Status = 1,
                        Message = "Cập nhật thông tin thành công"
                    });
                }
            }
            return Ok(new APIResult
            {
                Status = -1,
                Message = "Người dùng chưa đăng nhập hoặc không tồn tại"
            });
        }
        [HttpPost("account/changepassword")]
        public IActionResult ChangePassword([FromForm]ChangePasswordRequest request)
        {
            var uid = GetCurrentSession().UserID;
            var user = _context.User.FirstOrDefault(x=>x.ID== uid);
            if (Security.Encode(request.Old).Equals(user.Password))
            {
                user.Password = Security.Encode(request.New);
                _context.SaveChanges();
                return Ok(new APIResult
                {
                    Status = 1,
                    Message = "Đổi mật khẩu thành công"
                });
            }
            return Ok(new APIResult
            {
                Status = -1,
                Message = "Mật khẩu cũ chưa chính xác"
            });
        }
        [HttpGet("account/userproject")]
        public IActionResult GetUserProject(string keyword = "", int pageNumber = 1, int lineAmount = 10)
        {
            var session = GetCurrentSession();
            if (session == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Phiên đăng nhập đã hết hạn"
                });
            }
            var data = _context.Project.AsQueryable().Where(x => x.Title.Contains(keyword) && x.UserCreated == session.UserID && x.Status == EnumDefaultStatus.ACTIVE)
                        .Select(x => new { x.Title, x.Validate }).ToList();
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
        [HttpGet("account/getcustomer")]
        public IActionResult GetCustomer(int projectid = 0, string keyword = "", int pageNumber = 1, int lineAmount = 10)
        {
            var session = GetCurrentSession();
            if (session == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Phiên đăng nhập đã hết hạn"
                });
            }
            if (projectid == 0)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Dự án cần tìm không tồn tại"
                });
            }
            var query = _context.Customer.AsQueryable().Join(_context.Customer_Information,
                c => c.ID, ci => ci.CustomerID,
                (c, ci) => new { ID = c.ID, Phone = c.Phone, Email = c.Email, InformationID = ci.InformationID, ProjectID = ci.ProjectID })
                .GroupJoin(_context.Information,
                gr => gr.InformationID,
                i => i.ID,
                (gr, i) => new { gr, i })
                .SelectMany(x => x.i.DefaultIfEmpty(), (x,i) => new { x.gr.ID, x.gr.Phone, x.gr.Email, x.gr.ProjectID, Information= i.Name })
                .Where(x => x.ProjectID == projectid).ToList();
            var data = query.GroupBy(x => new { x.ID, x.Phone, x.Email, x.ProjectID })
                .Select(g => new
                {
                    ID = g.Key.ID,
                    Phone = g.Key.Phone,
                    Email = g.Key.Email,
                    ProjectID = g.Key.ProjectID,
                    Informations = g.Select(c => c.Information).ToList()
                }).ToList();
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

    }
}
