using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveProcessService.Data;
using LeaveProcessService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using LeaveProcessService.DataAcessHelper;
namespace LeaveProcessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveProcessController : ControllerBase
    {
        private readonly LeaveProcessServiceContext _context;
        private IConfiguration _configuration;
        private IOracleDBManager _oracleDBManager;
        public LeaveProcessController(LeaveProcessServiceContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        [Route("Get/leave-symbols/{id}")]
        [HttpGet]
        public async Task<ActionResult<LeaveSymbol>> GetLeaveSymbols(int id)
        {
            var objData = await  _context.LeaveSymbols.FindAsync(id);
            if(objData==null)
            {
                return NotFound();
            }
            return objData;
        }

        [Route("Get/leave-shifts/{id}")]
        [HttpGet]
        public async Task<ActionResult<LeaveShift>> GetLeaveShifts(int id)
        {
            var query = (from p in _context.LeaveShifts
                         where   p.ID==id
                         select new LeaveShift { ID = p.ID, CODE = p.CODE, NAME_VN = p.NAME_VN }
                       );
            if (query ==null)
            {
                return NotFound();
            }
            return await query.FirstOrDefaultAsync();
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
        /*
        [Route("Get/title-jobs")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataTable>>> GetTitleJobs()
        {
            _oracleDBManager = new OracleDBManager("PKG_WS_HRM_INTEGRATED.HRM01", CommandType.StoredProcedure, _configuration.GetConnectionString("DbConnect").ToString());
            return await _oracleDBManager.GetDataTableAsync();
        }
        */
    }
}
