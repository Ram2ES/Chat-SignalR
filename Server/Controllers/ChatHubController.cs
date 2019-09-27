using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatHubController : ControllerBase
    {
        // GET: api/ChatHub
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        private IHubContext<ChatHub> _hubContext;

        public ChatHubController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            _hubContext.Clients.All.SendAsync("Posted", value);
        }
    }
}
