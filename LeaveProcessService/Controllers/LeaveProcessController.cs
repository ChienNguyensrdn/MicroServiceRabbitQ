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
using System.Net;

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
        #region Leave process
        [Route("validate-register-leave")]
        [HttpPost]
        public async Task<ActionResult<ResponseResult>> ValidateRgisterLeave(ValidateLeavesheet leavesheet)
        {
            String message = string.Empty;
            int reponseStatus = 200;
            //string clientContentType; string clientAccept;
            try
            {
                //clientContentType = Request.Headers.ContentType.ToString();
                //clientAccept = Request.Headers.ToString();
               
                OracleCommand command = new OracleCommand();
                command.CommandText = "PKG_ATTENDANCE_BUSINESS.VALIDATE_LEAVE";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new OracleParameter("P_EMP_ID", OracleDbType.Decimal, leavesheet.employeeId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_FROMDATE", OracleDbType.NVarchar2, 8, leavesheet.leaveFrom, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_TODATE", OracleDbType.NVarchar2, 8, leavesheet.leaveTo, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVETYPE", OracleDbType.Decimal, 10, leavesheet.leaveType, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVETIME", OracleDbType.Decimal, 10, leavesheet.leaveTime, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_ID", OracleDbType.Decimal, 10, leavesheet.Id, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, 500, message, ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter("P_RESPONSESTATUS", OracleDbType.Int16, 10, reponseStatus, ParameterDirection.Output));
                _oracleDBManager = new OracleDBManager(command, _configuration.GetConnectionString("DbConnect").ToString());
                await _oracleDBManager.ExecuteNonQueryAsync();
                ResponseResult responseResult = new ResponseResult();
                responseResult.Responsestatus = int.Parse(command.Parameters["P_RESPONSESTATUS"].Value?.ToString());
                if (responseResult.Responsestatus > 0)
                {
                    responseResult.Responsestatus = (int)HttpStatusCode.OK;
                }
                else
                {
                    responseResult.Responsestatus = (int)HttpStatusCode.NotFound;
                }
                responseResult.Message = command.Parameters["P_MESSAGE"].Value.ToString(); ;
                return responseResult;
            }
            catch (Exception ex)
            {
                ResponseResult responseResult = new ResponseResult();
                responseResult.Message = ex.ToString();
                responseResult.Responsestatus = (int)HttpStatusCode.BadRequest;
                return responseResult;
            }
           
        }
      
        #endregion Leave process

        #region List 
        [Route("leave-types")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveType>>> GetLeaveTypes()
        {
            var query =  (from p in _context.LeaveTypes where p.IS_LEAVE == true select p);
            if (query == null)
            {
                return NotFound();
            }

            return await query.ToListAsync();
        }

        [Route("leave-types/{typeId}")]
        [HttpGet]
        public async Task<ActionResult<LeaveType>> GetLeaveTypes(int typeId )
        {
            var objData = await _context.LeaveTypes.FindAsync(typeId);
            if (objData == null)
            {
                return NotFound();
            }
            return objData;
        }

        [Route("leave-symbols")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveSymbol>>> GetLeaveSymbols()
        {
            return await _context.LeaveSymbols.ToListAsync();
        }

        [Route("leave-symbols/{symbolId}")]
        [HttpGet]
        public async Task<ActionResult<LeaveSymbol>> GetLeaveSymbols(int symbolId)
        {
            var objData = await  _context.LeaveSymbols.FindAsync(symbolId);
            if(objData==null)
            {
                return NotFound();
            }
            return objData;
        }

        [Route("leave-shifts/{shiftId}")]
        [HttpGet]
        public async Task<ActionResult<LeaveShift>> GetLeaveShifts(int shiftId)
        {
            var query = (from p in _context.LeaveShifts
                         where   p.ID== shiftId
                         select new LeaveShift { ID = p.ID, CODE = p.CODE, NAME_VN = p.NAME_VN }
                       );
            if (query ==null)
            {
                return NotFound();
            }
            return await query.FirstOrDefaultAsync();
        }

        [Route("leave-shifts")]
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
    #endregion
}
