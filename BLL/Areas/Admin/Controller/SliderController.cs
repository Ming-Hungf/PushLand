using BLL.Areas.Admin.Object;
using BLL.Controller;
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
    public class SliderController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment):base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("slider/list")]
        public IActionResult GetSlider()
        {
            var images = _context.Slider.Where(x => x.Status == EnumDefaultStatus.ACTIVE).OrderBy(x => x.Sort).ToList();
            return Ok(new APIResult
            {
                Status = 1,
                Data = images
            });
        }
        [HttpGet("slider/delete")]
        public IActionResult Delete(int id = 0)
        {
            var image = _context.Slider.FirstOrDefault(x => x.ID == id);
            if (image == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Ảnh cần xóa không tồn tại"
                });
            }
            image.Status = EnumDefaultStatus.DELETED;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xóa thành công"
            });
        }
        [HttpPost("slider/update")]
        public IActionResult Update([FromForm]List<SliderObject> objs)
        {
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE [Slider]");
            
            var uid = GetCurrentSession().UserID;
            int i = 1;
            foreach(var obj in objs)
            {
                var image = new Slider
                {
                    Created = DateTime.Now,
                    UserCreated = uid,
                    Sort = i,
                    Status = EnumDefaultStatus.ACTIVE,
                    Image = FileExtension.Upload(obj.File,_webHostEnvironment.WebRootPath,"Slider"),
                    Ticks = obj.Ticks
                };
                _context.Add(image);
                i++;
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Cập nhật thành công"
            });
        }
    }
}
