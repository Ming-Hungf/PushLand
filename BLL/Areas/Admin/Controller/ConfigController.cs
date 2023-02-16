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
    public class ConfigController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ConfigController(AppDbContext context,IWebHostEnvironment env):base(context)
        {
            _context = context;
            _webHostEnvironment = env;
        }
        [HttpGet("config/get")]
        public IActionResult Get()
        {
            var config = _context.Config.FirstOrDefault(x => x.Status == EnumDefaultStatus.ACTIVE);
            if (config == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Data = null,
                    Message = "Lỗi cài đặt"
                });
            }
            return Ok(new APIResult
            {
                Status = 1,
                Data = config
            });
        }
        [HttpPost("config/update")]
        public IActionResult Update([FromForm] ConfigObject obj)
        {
            var config = _context.Config.FirstOrDefault(x=>x.ID==obj.ID);
            if (config == null)
            {
                config = new Config { Status=EnumDefaultStatus.ACTIVE};
            }
            config.Name = obj.Name;
            config.Password = Security.Encode(obj.EmailPassword);
            config.Body = obj.Body;
            config.Phone = obj.Phone;
            config.Address = obj.Address;
            config.Email = obj.Email;
            config.Facebook = obj.Facebook;
            config.Youtube = obj.Youtube;
            config.Insta = obj.Insta;
            config.Description = obj.Description;
            if (obj.FaviconFile != null&&!obj.Favicon.Equals(config.Favicon))
            {
                config.Favicon = FileExtension.Upload(obj.FaviconFile, _webHostEnvironment.WebRootPath, "Favicon");
            }
            if(obj.LogoFile!=null&& !obj.Logo.Equals(config.Logo))
            {
                config.Logo = FileExtension.Upload(obj.LogoFile, _webHostEnvironment.WebRootPath, "Logo");
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Cập nhật thành công"
            });
        }
        [HttpGet("config/images")]
        public IActionResult GetImage()
        {
            var images = _context.System_Image.Where(x => x.Status == EnumDefaultStatus.ACTIVE).ToList();
            return Ok(new APIResult
            {
                Status = 1,
                Data = images
            });
        }
        
        [HttpGet("config/deleteimage")]
        public IActionResult Delete(int id = 0)
        {
            var image = _context.System_Image.FirstOrDefault(x => x.ID == id);
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
        [HttpGet("config/upload")]
        public IActionResult Upload([FromForm]ImageObject obj)
        {
            var images = _context.System_Image.Where(x => x.Status == EnumDefaultStatus.ACTIVE).ToList();
            int sort = 1;
            if (obj.Files != null)
            {
                foreach(var file in obj.Files)
                {
                    _context.Add(new System_Image
                    {
                        Status = EnumDefaultStatus.ACTIVE,
                        Sort = sort,
                        Image = FileExtension.Upload(file, _webHostEnvironment.WebRootPath, "SystemImage")
                    });
                    sort++;
                }
            }
            if (images.Count > 0)
            {
                foreach (var image in images)
                {
                    image.Status = EnumDefaultStatus.DELETED;
                }
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Tải lên hình ảnh thành công"
            });
        }
    }
}
