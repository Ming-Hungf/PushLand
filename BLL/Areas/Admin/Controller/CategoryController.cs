using BLL.Areas.Admin.Object;
using BLL.Controller;
using BLL.Filter;
using DAL.Component.Enum;
using DAL.Model;
using DAL.Plugins;
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
    [EnableCors("AllowSpecificOrigin")]
    public class CategoryController : OverallController
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context) : base(context)
        {
            _context = context;
        }
        [BeforeAction]
        [HttpGet("category/details")]
        public IActionResult Details(int id = 0)
        {
            var cate = _context.Category.FirstOrDefault(x => x.ID == id);
            if (cate == null)
            {
                cate = new Category { ID = 0 };
            }
            return Ok(new APIResult
            {
                Status = 1,
                Data = new CategoryObject(cate)
            });
        }
        [HttpGet("category/list")]
        public IActionResult List(string keyword = "", int pageNumber = 1, int lineAmount = 10)
        {
            var data = _context.Category.Where(x => x.Status == EnumDefaultStatus.ACTIVE&&x.Name.Contains(keyword)).OrderBy(x => x.Sort)
                .Include(x=>x.Children.Where(y=>y.Status==EnumDefaultStatus.ACTIVE)).ToList();
            return Ok(new APIResult
            {
                Status = 1,
                Data = data == null ? null : data.Skip((pageNumber - 1) * lineAmount).Take(lineAmount).ToList()
            });
        }
        private bool Check(List<Category> categories,string name)
        {
            foreach (var cate in categories)
            {
                if (cate.Name.Equals(name.Trim()))
                {
                    return true;
                }
                if (cate.Children != null)
                    return Check(cate.Children.ToList(),name);
            }
            return false;
        }
        [BeforeAction]
        [HttpPost("category/update")]
        public IActionResult Update([FromForm] CategoryObject request)
        {
            if (_context.Category.Any(x => x.Name == request.Name)||Security.isEmpty(request.Name))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Tên không hợp lệ hoặc đã tồn tại"
                });
            }
            var category = _context.Category.Include(x=>x.Children).FirstOrDefault(x => x.ID == request.ID);
            var userid = GetCurrentSession().UserID;
            string message;
            if (category == null)
            {
                category = new Category
                {
                    Name = request.Name,
                    Updated = DateTime.UtcNow,
                    Created = DateTime.UtcNow,
                    UserCreated = userid,
                    UserUpdated = userid,
                    Status = EnumDefaultStatus.ACTIVE
                };
                _context.Add(category);
                message = " Thêm danh mục thành công";
            }
            else
            {
                category.Name = request.Name;
                category.Updated = DateTime.UtcNow;
                category.UserUpdated = userid;
                message = "Sửa thành công";
            }
            if (request.ParentID !=null)
            {
                var parent = _context.Category.FirstOrDefault(x => x.ID == request.ParentID);
                if (parent == null || Check(category.Children.ToList(), parent.Name))
                {
                    return Ok(new APIResult
                    {
                        Status = -1,
                        Message = "Danh mục cha không hợp lệ"
                    });
                }
                category.ParentID = parent.ID;
                category.Parent = parent;
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = message
            });
        }
        [BeforeAction]
        [HttpGet("category/delete")]
        public IActionResult Delete(int id = 0)
        {
            var category = _context.Category.Include(x => x.Children).FirstOrDefault(x => x.Status == EnumDefaultStatus.ACTIVE && x.ID == id);
            if (category == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Danh mục cần xóa không tồn tại"
                });
            }
            category.Status = EnumDefaultStatus.DELETED;
            foreach(var cate in category.Children)
            {
                cate.Status = EnumDefaultStatus.DELETED;
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xóa thành công"
            });
        }
    }
}
