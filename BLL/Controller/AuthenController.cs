using BLL.Component.Object;
using BLL.Service;
using BLL.ViewModel;
using DAL.Component.Enum;
using DAL.Model;
using DAL.Plugins;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BLL.Controller
{
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class AuthenController : OverallController
    {
        private readonly AppDbContext _context;
        private SendMailService _mailservice;
        public AuthenController(AppDbContext context) :base(context)
        {
            _context = context;
        }

        [HttpPost("authen/login")]
        public  IActionResult Login([FromForm]LoginViewModel model)
        {
            string encoded = model.Password.Encode();
            var user = _context.User.FirstOrDefault(x => x.Phone.Equals(model.Phone) && x.Password.Equals(encoded) &&x.Status!=EnumDefaultStatus.DELETED);
            if (user == null)
            {
                return Ok(new APIResult { Status = -1, Data=null, Message = "Tài khoản không tồn tại" });
            }
            string token = Security.GenerateAuthKey();
            _context.Add(new UserSession
            {
                UserID = user.ID,
                AccessToken = token,
                LoginResult = 1,
                Phone = user.Phone,
                FullName = user.Fullname,
                Remember = model.Remember,
                Expire = DateTime.UtcNow.AddYears(1),
                RoleID = user.RoleID,
                Status = EnumDefaultStatus.ACTIVE,
            }) ;
            _context.SaveChanges();
            return Ok(new APIResult { Status = 1, Data = token, Message = "Đăng nhập thành công" });
        }
        

        [HttpGet("authen/logout")]
        public IActionResult Logout()
        {
            var session = GetCurrentSession();
            if (session == null)
            {
                return Ok(new APIResult { Status = -1, Data = null, Message = "Phiên đăng nhập hết hạn" });
            }
            _context.Remove(session);
            _context.SaveChanges();
            return Ok(new APIResult { Status = 1,Data= null, Message = "Đăng xuất thành công" });
        }
        [HttpPost("authen/register")]
        public IActionResult Register([FromForm]RegisterRequest request)
        {
            string message;
            if (_context.User.Any(x => x.Email.Equals(request.Email))|| !Security.IsValidEmail(request.Email))
            {
                message = "Email không hợp lệ hoặc đã được đăng ký";
            }
            else if (_context.User.Any(x => x.Phone.Equals(request.Phone)) || !request.Phone.isPhoneNumber(10))
            {
                message = "Số điện thoại không hợp lệ hoặc đã được đăng ký";
            }
            else if (request.Fullname.isEmpty())
            {
                message = "Họ và tên không được để trống";
            }
            else
            {
                User user = new User
                {
                    Fullname = request.Fullname,
                    Phone = request.Phone,
                    Password = request.Password.Encode(),
                    Status = EnumDefaultStatus.ACTIVE,
                    Email = request.Email,
                    RoleID = 2,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                };
                _context.Add(user);
                _context.SaveChanges();
                return Ok(new APIResult
                {
                    Status = 1,
                    Message = "Đăng ký thành công"
                });
            }
            return Ok(new APIResult
            {
                Status = -1,
                Message = message
            });
        }
        [HttpPost("authen/forgetpassword")]
        public async Task< IActionResult> ForgetPassword([FromForm]string email)
        {
            var user = _context.User.FirstOrDefault(x => x.Email.Equals(email) && x.Status == EnumDefaultStatus.ACTIVE);
            if (user == null)
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Người dùng với email này không tồn tại"
                });
            }
            if (!Security.IsValidEmail(email))
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Email không hợp lệ"
                });
            }
            var setting = new MailSettings();
            var config = _context.Config.FirstOrDefault();
            if (config == null)
            {
                 return Ok(new APIResult
                 {
                    Data = null,
                    Message = "Lỗi cài đặt hệ thống, thử lại sau",
                    Status = -1
                });
            }
            setting.DisplayName = config.Name;
            setting.Email = config.Email;
            setting.Password = Security.Decode(config.Password);
            _mailservice = new SendMailService(setting);
            var rand = new Random();
            var otp = rand.Next(10001, 99999);
            string otp_str = string.Format($"<h2>{otp}</h2>");
            var otp_key = Security.GenerateAuthKey();
            var content = new MailContent
            {
                To = email,
                Subject = "Xác nhận mail",
                Body = config.Body+ otp_str
            };
            await _mailservice.SendMail(content);
            var option = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            user.OTP = SignData(otp.ToString(),otp_key);
            user.OTPCreated = DateTime.UtcNow;
            _context.SaveChanges();
            Response.Cookies.Append("OTP_KEY", otp_key, option);
            return Ok(new APIResult
            {
                Data = null,
                Message = "Đã gửi mail xác nhận",
                Status = 1
            });
        }
        [HttpPost("authen/resetpassword")]
        public IActionResult ResetPassword([FromForm]ChangePasswordObject obj)
        {
            var key = Request.Cookies["OTP_Key"].ToString();
            if (key != null)
            {
                var hashotp = SignData(obj.OTP, key);
                var user = _context.User.FirstOrDefault(x => x.OTP.Equals(hashotp) && x.Status == EnumDefaultStatus.ACTIVE);
                var expired = (DateTime)user.OTPCreated;
                if (user != null&& expired.AddMinutes(10)!=DateTime.UtcNow)
                {
                    user.Password = Security.Encode(obj.New);
                    _context.SaveChanges();
                    return Ok(new APIResult
                    {
                        Status = 1,
                        Message = "Đặt lại mật khẩu thành công"
                    });
                }
                Response.Cookies.Delete("OTP_Key");
            }
            return Ok(new APIResult
            {
                Status = -1,
                Message="Mã xác thực không đúng hoặc đã hết hạn"
            });
        }
        private string SignData(string message, string secret)
        {
            var encoding = new System.Text.UTF8Encoding();
            var keyBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);
            using (var hmacsha1 = new HMACSHA1(keyBytes))
            {
                var hashMessage = hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        }
    }
}
