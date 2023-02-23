using BLL.Service;
using DAL.Model;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Controller
{
    [Route("api/")]
    [ApiController]
    public class ChatController : OverallController
    {
        private readonly AppDbContext _context;
        private readonly ChatService _service;

        public ChatController(AppDbContext context, ChatService service):base(context)
        {
            _context = context;
            _service = service;
        }
        [HttpPost("chat/send")]
        public async Task<IActionResult> Send([FromForm] ChatMessage chatMessage)
        {
            try
            {
                await _service.SendMessage(chatMessage);
                var device = _context.User.FirstOrDefault(x => x.ID == chatMessage.To).DeviceToken;
                var message = new Message()
                {
                    Token = device,
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = "Tin nhắn mới",
                        Body = "Bạn có tin nhắn mới"
                    }
                };
                // Send the message using the Firebase Cloud Messaging service
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
            catch
            {
                return Ok(new APIResult
                {
                    Status = -1,
                    Message = "Có lỗi xảy ra. Vui lòng thử lại sau"
                });
            }
            return Ok(new APIResult
            {
                Status = 1,
                Message = "Gửi tin nhắn thành công"
            });
        }
    }
}
