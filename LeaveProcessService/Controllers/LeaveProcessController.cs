using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveProcessService.Data;
using LeaveProcessService.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaveProcessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveProcessController : ControllerBase
    {
        private readonly LeaveProcessServiceContext _context;
        public LeaveProcessController(LeaveProcessServiceContext context)
        {
            _context = context;
        }
        [Route("Get/leave-types")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveType>>> GetLeaveTypes()
        {
            return await _context.LeaveTypes.ToListAsync();
        }
        [Route("Get/leave-symbols")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveSymbol>>> GetLeaveSymbols()
        {
            return await _context.LeaveSymbols.ToListAsync();
        }
        [Route("Get/leave-shifts")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveShift>>> GetLeaveShifts()
        {
            var query = (from p in _context.LeaveShifts
                         join t in _context.OtherListTypes on p.TYPE_ID equals t.ID
                         where t.CODE == "LEAVE_SHIFT"
                         select new LeaveShift { ID = p.ID, CODE = p.CODE, NAME_VN = p.NAME_VN }
                       );
         
            return await query.ToListAsync();
        }
    }
}
