using BLL.Areas.Admin.Object;
using BLL.Component.Enum;
using BLL.Component.Object;
using BLL.Controller;
using BLL.Filter;
using DAL.Component.Enum;
using DAL.Model;
using DAL.Plugins;
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
    public class CustomerController : OverallController
    {
        public readonly AppDbContext _context;
        public CustomerController(AppDbContext context): base(context)
        {
            _context = context;
        }
        [BeforeAction]
        [HttpGet("customer/advise")]
        public IActionResult GetAdvise(string keyword="",int pageNumber = 1,int lineAmount = 10)
        {
            var data = _context.Customer.AsQueryable().Where(x => x.CustomerType == EnumCustomer.ADVISE&&(x.Fullname.Contains(keyword)||x.Email.Contains(keyword)||x.Phone.Contains(keyword)||x.Note.Contains(keyword))).OrderByDescending(x => x.Status).ToList();
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
        
        [HttpPost("customer/advise")]
        public IActionResult Advise([FromForm]AdviseObject obj)
        {
            if (obj.FullName == null || obj.Email == null || obj.Phone == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Vui lòng điền đủ thông tin"
                });
            }
            if (!Security.IsValidEmail(obj.Email) || !Security.isPhoneNumber(obj.Phone))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Email hoặc số điện thoại không hợp lệ"
                });
            }
            if (_context.Customer.Any(x => x.Phone.Equals(x.Phone) && x.Email.Equals(obj.Email) && x.Fullname.Equals(obj.FullName)&&x.Status==EnumDefaultStatus.ACTIVE))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Khách hàng đã tồn tại"
                });
            }
            var customer = new Customer
            {
                Status = EnumDefaultStatus.ACTIVE,
                Phone = obj.Phone,
                Email = obj.Email,
                Fullname = obj.FullName,
                Note = obj.Note,
                CustomerType = EnumCustomer.ADVISE
            };
            _context.Add(customer);
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Đăng kí nhận tư vấn thành công"
            });
        }
        [HttpPost("customer/register")]
        public IActionResult Register([FromForm] RegisterObject obj)
        {
            if (obj.Email == null || obj.Phone == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Vui lòng điền đủ thông tin"
                });
            }
            var customer = _context.Customer.FirstOrDefault(x => x.Phone.Equals(x.Phone) && x.Email.Equals(obj.Email) && x.Status == EnumDefaultStatus.ACTIVE);
            if (customer!=null&&_context.Customer_Information.Any(x=>x.CustomerID==customer.ID&&x.ProjectID==obj.ProjectID))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Khách hàng đã tồn tại"
                });
            }
            else if (customer == null)
            {
                customer = new Customer
                {
                    Status = EnumDefaultStatus.ACTIVE,
                    Phone = obj.Phone,
                    Email = obj.Email,
                    CustomerType = EnumCustomer.REGISTER
                };
                _context.Add(customer);
                _context.SaveChanges();
            }
            foreach(var id in obj.InformationIDs)
            {
                _context.Customer_Information.Add(new Customer_Information
                {
                    CustomerID = customer.ID,
                    ProjectID = obj.ProjectID,
                    InformationID = id
                });
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Thông tin báo giá sẽ được gửi ngay. Cám ơn bạn"
            });
        }
        [HttpGet("customer/changestatus")]
        public IActionResult ChangeStatus(int id = 0)
        {
            var customer = _context.Customer.FirstOrDefault(x => x.ID == id);
            if (customer == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Khách hàng không tồn tại"
                });
            }
            if (customer.Status == EnumDefaultStatus.ACTIVE)
            {
                customer.Status = EnumDefaultStatus.DELETED;
            }
            else
            {
                customer.Status = EnumDefaultStatus.ACTIVE;
            }
            _context.SaveChanges();
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Thay đổi trạng thái thành công"
            });
        }
    }
}
