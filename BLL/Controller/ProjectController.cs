using BLL.Component.Enum;
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
    public class ProjectController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProjectController(AppDbContext context,IWebHostEnvironment env) : base(context)
        {
            _context = context;
            _webHostEnvironment = env;
        }
        //private string GetAddress(int wardid)
        //{
        //    var ward = _context.Ward.FirstOrDefault(x => x.ID == wardid);
        //    var district = _context.District.FirstOrDefault(x => x.ID == ward.ParentID);
        //    var city = _context.City.FirstOrDefault(x => x.ID == district.ParentID);
        //    return ward.Name + ", " + district.Name + ", " + city.Name;
        //}
        [HttpGet("project/details")]
        public IActionResult Details(int id=0)
        {
            if (id == 0)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Dự án không tồn tại"
                });
            }
            var project = _context.Project.Include(x=>x.Documents).FirstOrDefault(x => x.ID == id && x.Status == EnumDefaultStatus.ACTIVE);
            var data = new DetailsViewModel(project);
            data.Images = project.Documents.Select(x => x.Link).ToList();
            var detail = _context.ProjectDetails.FirstOrDefault(x => x.Status == EnumDefaultStatus.ACTIVE && x.ProjectID == id);
            if (detail != null)
            {
                data.ProjectName = detail.ProjectName;
                data.TotalFloor = detail.TotalFloor;
                data.Floor = detail.Floor;
                data.TransferDate = detail.TransferDate;
                data.Elevator = detail.Elevator;
            }
            return Ok(new APIResult
            {
                Status = 1,
                Data = data
            });
        }
        [HttpGet("project/list")]
        public IActionResult List(int category=0,int wardid = 0,float from =0,float to =0,int Fromprice=0,int Toprice=0,int bedroom=0,int bathroom=0,string maindirect="",string balcony = "", int pageNumber = 1, int lineAmount = 10)
        {
            var data = _context.Project.Where(x => x.Status == EnumDefaultStatus.ACTIVE&&x.Validate==EnumValidate.VALIDATED)
                .Select(x => new ProjectViewModel 
                {
                    ID = x.ID, 
                    Address = x.Address,
                    WardID=x.WardID,
                    Title = x.Title,
                    Bathroom = x.Bathroom,
                    Bedroom = x.Bedroom,
                    TotalPrice = x.TotalPrice,
                    Square = x.Square,
                    MainDirect=x.MainDirection,
                    Balcony=x.BalconyDirection,
                    CategoryID=x.CategoryID })
                .ToList();
            if (category != 0)
            {
                data = data.Where(x => x.CategoryID == category).ToList();
            }
            if (wardid != 0)
            {
                data = data.Where(x => x.WardID == wardid).ToList();
            }
            if (from != 0 && to != 0)
            {
                data = data.Where(x => x.Square >= from && x.Square <= to).ToList();
            }
            if (Fromprice != 0 && Toprice != 0)
            {
                data = data.Where(x => x.TotalPrice >= Fromprice && x.TotalPrice <= Toprice).ToList();
            }
            if(bedroom != 0)
            {
                data = data.Where(x => x.Bedroom==bedroom).ToList();
            }
            if (bathroom != 0)
            {
                data = data.Where(x => x.Bathroom == bathroom).ToList();
            }
            if (!maindirect.Equals(""))
            {
                data = data.Where(x => x.MainDirect.Equals(maindirect)).ToList();

            }
            if (!balcony.Equals(""))
            {
                data = data.Where(x => x.Balcony.Equals(balcony)).ToList();
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
                Data = result
            });
        }
        [BeforeAction]
        [HttpGet("project/delete")]
        public IActionResult Delete(int id)
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
            var project = _context.Project.FirstOrDefault(x => x.ID == id);
            if (project == null||project.UserCreated!=session.UserID)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Dự án cần xóa không tồn tại hoặc người dùng không có quyền xóa"
                });
            }
            project.Status = EnumDefaultStatus.DELETED;
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Xóa thành công"
            });
        }
        [BeforeAction]
        [HttpPost("project/update")]
        public IActionResult Update([FromForm]ProjectObject obj)
        {
            var session = GetCurrentSession();
            var userid = session.UserID;
            var user = _context.User.FirstOrDefault(x => x.ID == userid && x.Status == EnumDefaultStatus.ACTIVE);
            string message="Cập nhật thành công. ";
            if (_context.News.Any(x => x.Status == EnumDefaultStatus.ACTIVE && x.Title.Equals(obj.Title)))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Tiêu đề đã tồn tại"
                });
            }
            
            var project = _context.Project.FirstOrDefault(x=>x.ID ==obj.ID);
            if (project == null)
            {
                project = new Project
                {
                    Created = DateTime.UtcNow,
                    UserCreated = userid,
                    Status = EnumDefaultStatus.ACTIVE,
                    ContactPhone = session.Phone,
                    ContactName = session.FullName,
                };
                _context.Add(project);
                message = "Tạo dự án thành công. ";
            }
            project.Updated = DateTime.UtcNow;
            project.UserUpdated = userid;
            project.Title = obj.Title;
            project.Square = obj.Square;
            project.Price = obj.Price;
            project.WardID = obj.WardID;
            project.Address = obj.Address;
            project.Description = obj.Description;
            project.Bathroom = obj.Bathroom;
            project.Bedroom = obj.Bedroom;
            project.MainDirection = obj.MainDirect;
            project.BalconyDirection = obj.Balcony;
            project.TransactionType = obj.TransactionType;
            project.CategoryID = obj.CategoryID;
            project.Validate = 0;
            project.EstateType = obj.EstateType;
            _context.SaveChanges();
            if (obj.ProjectName!=null ||obj.Investor!=null ||obj.Floor != null ||obj.TotalFloor != null || obj.Elevator != null)
            {
                var details = new ProjectDetails
                {
                    ProjectID = project.ID,
                    Status = EnumDefaultStatus.ACTIVE,
                    Investor = obj.Investor,
                    Floor = obj.Floor,
                    TotalFloor = obj.TotalFloor,
                    Elevator = obj.Elevator,
                    TransferDate = obj.TransferDate
                };
            }
            if (obj.ContactName != null || obj.ContactPhone != null  || obj.ContactEmail != null)
            {
                project.ContactEmail = obj.ContactEmail;
                project.ContactName = obj.ContactName;
                project.ContactPhone = obj.ContactPhone;
            }
            
            if (obj.Files != null)
            {
                var old_documents = _context.Project_Document.Where(x => x.Status == EnumDefaultStatus.ACTIVE && x.ProjectID == project.ID).ToList();
                foreach(var document in old_documents)
                {
                    document.Status = EnumDefaultStatus.DELETED;
                }
                List<Project_Document> new_documents = new List<Project_Document>();
                foreach(var file in obj.Files)
                {
                    var document = new Project_Document
                    {
                        Status = EnumDefaultStatus.ACTIVE,
                        ProjectID = project.ID,
                        Link = FileExtension.Upload(file, _webHostEnvironment.WebRootPath, "ProjectImage")
                    };
                    new_documents.Add(document);
                }
                _context.AddRange(new_documents);
            }
            
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = message+"Chúng tôi sẽ xem xét tin của bạn trong thời gian sớm nhất trước khi đưa lên website"
            });

        }
    }
}
