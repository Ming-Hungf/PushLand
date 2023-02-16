using BLL.Areas.Admin.Object;
using BLL.Areas.Admin.ViewModel;
using BLL.Component.Enum;
using BLL.Component.Object;
using BLL.Controller;
using BLL.Filter;
using DAL.Component.Enum;
using DAL.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ValidateController : OverallController
    {
        private readonly AppDbContext _context;
        public ValidateController(AppDbContext context) : base(context)
        {
            _context = context;
        }
        [HttpGet("validate/list")]
        public IActionResult List(string keyword = "", int pageNumber = 1, int lineAmount = 10)
        {
            var data = _context.Project.Where(x => x.Validate != EnumValidate.VALIDATED && x.Status == EnumDefaultStatus.ACTIVE&&x.Title.Contains(keyword))
                .Include(x=>x.Documents)
                .Join(_context.User,
                p=> p.UserCreated,
                u=>u.ID,
                (p,u)=>new ValidateViewModel {
                    ID =p.ID,
                    Title = p.Title,
                    Created = p.Created,
                    Updated=p.Updated,
                    UserCreated = u.Fullname,
                    Images = p.Documents.Select(x => x.Link).ToList()
                })
                .ToList();
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
        [HttpPost("validate/changestatus")]
        public IActionResult ChangeStatus([FromForm] ValidateObject obj)
        {
            var userid = GetCurrentSession().UserID;
            var project = _context.Project.FirstOrDefault(x => x.ID == obj.ProjectID&&x.Status ==EnumDefaultStatus.ACTIVE);
            if (obj.ValidateID == EnumValidate.REJECTED)
            {
                var history = new Project_Delete_History
                {
                    Created = DateTime.UtcNow,
                    Status = EnumDefaultStatus.ACTIVE,
                    UserCreated = userid,
                    Note = obj.Note
                };
                _context.Add(history);
            }
            project.Validate = obj.ValidateID;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xét duyệt thành công"
            });
        }

    }
}
