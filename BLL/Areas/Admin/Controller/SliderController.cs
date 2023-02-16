using BLL.Areas.Admin.Object;
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
            var images = _context.Slider.AsQueryable().Where(x => x.Status == EnumDefaultStatus.ACTIVE).OrderBy(x => x.Sort).ToList();
            return Ok(new APIResult
            {
                Status = 1,
                Data = images
            });
        }
        [BeforeAction]
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
        [BeforeAction]
        [HttpPost("slider/update")]
        public IActionResult Update([FromForm]List<SliderObject> objs)
        {
            var uid = GetCurrentSession().UserID;
            var images = _context.Slider.AsQueryable().Where(x => x.Status == EnumDefaultStatus.ACTIVE).ToList();
            var newids = objs.Where(x => x.ID != 0).Select(x => x.ID).ToList();
            var oldids = images.Select(x => x.ID).ToList();
            if (oldids.Count > newids.Count)
            {
                var deleteids = oldids.Except(newids);
                foreach(var id in deleteids)
                {
                    var image = images.FirstOrDefault(x => x.ID == id);
                    image.Status = EnumDefaultStatus.DELETED;
                }
            }
            
            foreach (var obj in objs)
            {
                var image = images.FirstOrDefault(x => x.ID == obj.ID);
                if (image == null)
                {
                    image = new Slider
                    {
                        Created = DateTime.UtcNow,
                        UserCreated = uid,
                        Status = EnumDefaultStatus.ACTIVE,
                        Image = "",
                    };
                }
                if (obj.Image.Equals(image.Image) && obj.File != null)
                {
                    image.Image = FileExtension.Upload(obj.File, _webHostEnvironment.WebRootPath, "Slider");
                }
                image.Ticks = obj.Ticks;
                image.Updated = DateTime.UtcNow;
                image.UserUpdated = uid;
                image.Sort = obj.Sort;
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
