using BLL.Areas.Admin.Object;
using BLL.Areas.Admin.ViewModel;
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
    public class NewsController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsController(AppDbContext context,IWebHostEnvironment env) : base(context)
        {
            _context = context;
            _webHostEnvironment = env;
        }
        [HttpGet("news/list")]
        public IActionResult List(string keyword = "", int pageNumber = 1, int lineAmount = 10)
        {
            var data = _context.News.Where(x => x.Status == EnumDefaultStatus.ACTIVE && (x.Title.Contains(keyword) || x.Content.Contains(keyword)))
                .Include(x=>x.Images.Where(x=>x.Status==EnumDefaultStatus.ACTIVE))
                .Select(x=> new NewsViewModel {ID = x.ID,Title=x.Title,Content=x.Content,Created=x.Created,Images=x.Images.Select(g=>g.Image).ToList() }).ToList();
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
        [HttpGet("news/details")]
        public IActionResult Details(int id = 0)
        {
            var news = _context.News.Where(x=>x.ID == id&&x.Status==EnumDefaultStatus.ACTIVE).Include(x => x.Images.Where(x => x.Status == EnumDefaultStatus.ACTIVE))
                .Select(x => new NewsViewModel { ID = x.ID, Title = x.Title, Content = x.Content, Created = x.Created, Images = x.Images.Select(g => g.Image).ToList() }).FirstOrDefault();
            if(news== null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Tin không tồn tại"
                });
            }
            return Ok(new APIResult
            {
                Status = 1,
                Data = news
            });
        }
        [BeforeAction]
        [HttpPost("news/update")]
        public IActionResult Update([FromForm]NewsObject obj)
        {
            var userid = GetCurrentSession().UserID;
            string message = "Cập nhật thành công";
            if (_context.News.Any(x => x.Status == EnumDefaultStatus.ACTIVE && x.Title.Equals(obj.Title)))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Tiêu đề đã tồn tại"
                });
            }
            var news = _context.News.FirstOrDefault(x => x.ID == obj.ID);
            if (news == null)
            {
                news = new News
                {
                    Created = DateTime.UtcNow,
                    UserCreated = userid,
                    Status = EnumDefaultStatus.ACTIVE
                };
                message = "Tạo tin thành công";
                _context.Add(news);
                
            }
            news.Content = obj.Content;
            news.Title = obj.Title;
            news.UserUpdated = userid;
            news.Updated = DateTime.UtcNow;
            _context.SaveChanges();
            var images = _context.News_Image.Where(x => x.Status == EnumDefaultStatus.ACTIVE && x.NewsID == news.ID).ToList();
            if (images.Count > 0)
            {
                foreach(var image in images)
                {
                    image.Status = EnumDefaultStatus.DELETED;
                }
            }
            if (obj.Images.ToList().Count > 0)
            {
                foreach (var image in obj.Images)
                {
                    _context.News_Image.Add(new News_Image
                    {
                        NewsID = news.ID,
                        Status = EnumDefaultStatus.ACTIVE,
                        Image = FileExtension.Upload(image, _webHostEnvironment.WebRootPath, "NewsImage")
                    });
                }
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = message
            });
        }
        [BeforeAction]
        [HttpGet("news/delete")]
        public IActionResult Delete(int id = 0)
        {
            var news = _context.News.FirstOrDefault(x => x.ID == id && x.Status == EnumDefaultStatus.ACTIVE);
            if (news == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Tin cần xóa không tồn tại"
                });
            }
            news.Status = EnumDefaultStatus.DELETED;
            var images = _context.News_Image.Where(x => x.Status == EnumDefaultStatus.ACTIVE && x.NewsID == news.ID).ToList();
            foreach(var image in images)
            {
                image.Status = EnumDefaultStatus.DELETED;
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
