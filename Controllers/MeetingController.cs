using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTrade.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProjectTrade.Models.CommonModel;

namespace ProjectTrade.Controllers
{
    [ApiController]
    [Route("api/meeting/")]
    public class MeetingController : ControllerBase
    {
        private readonly CustomerContext _context;

        public MeetingController(CustomerContext context)
        {
            _context = context;
        }


        [HttpGet("count")]
        public async Task<ActionResult<CountResponse>> GetMeetingCount()
        {
            var count = await _context.MeetingDetail.CountAsync();
            return Ok(new CountResponse { Count = count });
        }
        [HttpGet("meetingdetails")]
        public async Task<ActionResult<IEnumerable<MeetingDetail>>> GetMeetingDetails()
        {
            return await _context.MeetingDetail.ToListAsync();
        }
    }
}

