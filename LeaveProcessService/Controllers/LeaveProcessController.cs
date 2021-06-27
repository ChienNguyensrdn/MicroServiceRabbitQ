using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using LeaveProcessService.Data;
using LeaveProcessService.Entities;
using LeaveProcessService.Request;
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
    public partial class LeaveProcessController : ControllerBase
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
        [Route("leave-entitlement")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entitlement>>> GetEntitlements(string employeeId,int year=2021)
        {
            DataTable dataTable = new DataTable();
            List<Entitlement> lsResult = new List<Entitlement>();
            OracleCommand command = new OracleCommand();
            try
            {
               
                command.CommandText = "PKG_ATTENDANCE_BUSINESS.GET_REMAIN_LEAVE_SHEET_API";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new OracleParameter("P_EMP_IDS", OracleDbType.Clob, 1000, employeeId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_YEAR", OracleDbType.Decimal, 4, year, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_CUR", OracleDbType.RefCursor, ParameterDirection.Output));
                _oracleDBManager = new OracleDBManager(command, _configuration.GetConnectionString("DbConnect").ToString());
                dataTable=await _oracleDBManager.GetDataTableAsync();
                if(dataTable ==null || dataTable?.Rows.Count<1)
                {
                    return NotFound();
                }
                foreach(DataRow rows in dataTable.Rows)
                {
                    Entitlement entitlement = new Entitlement();
                    entitlement.employeeId =int.Parse( rows["ID"]?.ToString());
                    entitlement.employeeCode = rows["EMPLOYEE_CODE"]?.ToString();
                    entitlement.employeeName = rows["FULLNAME_VN"]?.ToString();
                    entitlement.orgName = rows["ORG_NAME"]?.ToString();
                    entitlement.titleName = rows["TITLE_NAME"]?.ToString();
                    entitlement.curHave =decimal.Parse( rows["CUR_HAVE"]?.ToString());
                    lsResult.Add(entitlement);
                    entitlement = null;
                }
                return lsResult;
            }
            catch(Exception ex)
            {
                return NotFound();
            }
            finally
            {
                dataTable.Dispose();
                lsResult = null;
                command.Dispose();
            }
        }
        [Route ("register-leave")]
        [HttpPost]
        public async Task<ActionResult<ResponseResult>> RegisterLeave(EntitlementRequest entitlementRequest)
        {
            ResponseResult responseResult;
            OracleCommand command;
            try
            {
                 command = new OracleCommand();
                //validate data fields ....
                //validate .....

                command.CommandText = "PKG_ATTENDANCE_BUSINESS.INSERT_AT_LEAVESHEET_API";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new OracleParameter("P_ID_EOFFICE", OracleDbType.NVarchar2, 20, entitlementRequest.leaveEOfficeId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_EMOLOYEE_ID", OracleDbType.Int16, 10, entitlementRequest.employeeId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_FROM", OracleDbType.NVarchar2, 8, entitlementRequest.leaveFrom, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_TO", OracleDbType.NVarchar2, 8, entitlementRequest.leaveTo, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_MANUAL_ID", OracleDbType.Int16, 10, entitlementRequest.manualId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_TIME_ID", OracleDbType.Int16, 10, entitlementRequest.leaveTimeId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_PLACE", OracleDbType.NVarchar2, 100, entitlementRequest.leavePlace, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_REASON", OracleDbType.NVarchar2, 100, entitlementRequest.leaveReason, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_IS_OFFSHORE", OracleDbType.Boolean, 1, entitlementRequest.isOffshore, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_NOTE", OracleDbType.NVarchar2, 100, entitlementRequest.note, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LEAVE_REPLACE_ID", OracleDbType.Int16, 10, entitlementRequest.leaveReplaceId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_BALANCE_NOW", OracleDbType.Int16, 10, entitlementRequest.balanceNow, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_USER", OracleDbType.NVarchar2, 20, entitlementRequest.userLog, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_LOG", OracleDbType.NVarchar2, 10, HttpRequestHeader.Host.ToString(), ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, 500, "", ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter("P_RESPONSESTATUS", OracleDbType.Int16, 10, -1, ParameterDirection.Output));
                _oracleDBManager = new OracleDBManager(command, _configuration.GetConnectionString("DbConnect").ToString());
                await _oracleDBManager.ExecuteNonQueryAsync();
                responseResult = new ResponseResult();
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
            catch(Exception ex)
            {
                responseResult = new ResponseResult();
                responseResult.Responsestatus = (int)HttpStatusCode.NoContent;
                responseResult.Message = ex.ToString();
                return responseResult;
            }
        }
        [Route("leave-sheets/employeeId/{employeeId}/page/{page}/pageSize/{pageSize}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leavesheet>>> GetLeaveSheets(int employeeId = -1, int page=1 , int pageSize =50)
        {
            try
            {
                if (employeeId == -1)
                {
                    return await _context.LeaveSheets.Skip(page * pageSize).Take(pageSize).ToListAsync();
                }
                return await _context.LeaveSheets.Where(x => x.EMPLOYEE_ID == employeeId).Skip(page * pageSize).Take(pageSize).ToListAsync();
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("leave-replaces/page/{page}/pageSize/{pageSize}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetLeaveReplaces(int page = 1, int pageSize = 50)
        {
            try
            {
                return await _context.Employees.Skip(page * pageSize).Take(pageSize).ToListAsync();
            }
            catch
            {
                return NotFound();
            }
        }


        [Route("leave-replaces/employeeId/{employeeId}")]
        [HttpGet]
        public async Task<ActionResult<Employee>> GetLeaveReplaces(int employeeId)
        {
            try
            {
                return await _context.Employees.Where(x => x.Id == employeeId).FirstAsync();
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("leave-sheet")]
        [HttpDelete]
        public async Task<ActionResult<ResponseResult>> DeleteLeaveSheet (int leavesheetId)
        {
            ResponseResult responseResult;
            try
            {
                //validate trươc khi xoa...

                Leavesheet leavesheet =await _context.LeaveSheets.Where(x => x.ID == leavesheetId).SingleAsync<Leavesheet>();
                if (leavesheet == null)
                {
                    return NotFound();
                }
                foreach (var item in _context.LeavesheetDetails.Where(x => x.LEAVE_ID_PH == leavesheet.ID).ToList())
                {
                    _context.Remove(item);
                }
                _context.Remove(leavesheet);
                _context.SaveChanges();
                responseResult = new ResponseResult();
                responseResult.Message = "Successs";
                responseResult.Responsestatus = (int)HttpStatusCode.OK;
                return responseResult;
            }
            catch(Exception ex)
            {
                responseResult = new ResponseResult();
                responseResult.Message = ex.ToString();
                responseResult.Responsestatus = (int)HttpStatusCode.BadRequest;
                return responseResult;
            }
            finally
            {
                responseResult = null;
            }
        }

        [Route("approve-leave-sheets")]
        [HttpPut]
        public async Task<ActionResult<ResponseResult>> ApproveLeaveSheets(ApproveRequest approveRequest)
        {
            int statusId = 0;
            if (approveRequest == null)
            {
                return NotFound();
            }
            switch (approveRequest.statusCode)
            {
                case "APPROVE1":
                    statusId = 3;
                    break;
                case "APPROVE2":
                    statusId = 1;
                    break;
                case "RESET":
                    statusId = 0;
                    break;
                case "REJECT":
                    statusId = 2;//ko duet
                    break;
                case "CANCEL":
                    statusId=-1;
                    break;
                default:
                    statusId = -99;
                    break;
            }
            if(statusId==-99)
            {
                ResponseResult responseResult = new ResponseResult();
                responseResult.Message = "Approve status is incorrect";
                responseResult.Responsestatus = (int)HttpStatusCode.NotFound ;
                return responseResult;
            }
            try
            {
                OracleCommand command = new OracleCommand();
                command.CommandText = "PKG_ATTENDANCE_BUSINESS.APPROVELEAVESHEETAPI";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new OracleParameter("P_IDS", OracleDbType.Clob, 1000,approveRequest.leavesheetIds, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_ACTIVE", OracleDbType.Decimal, 2, statusId, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_USER", OracleDbType.NVarchar2, 50, approveRequest.userName, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, 500, "", ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter("P_RESPONSESTATUS", OracleDbType.Int16, 10, -1, ParameterDirection.Output));
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
            catch(Exception ex )
            {
                ResponseResult responseResult = new ResponseResult();
                responseResult.Message = ex.ToString();
                responseResult.Responsestatus = (int)HttpStatusCode.BadRequest;
                return responseResult;
            }
        }


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
               //validate ContentType ....
                OracleCommand command = new OracleCommand();
                command.CommandText = "PKG_ATTENDANCE_BUSINESS.VALIDATE_LEAVE";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new OracleParameter("P_EMP_ID", OracleDbType.Decimal, 10,leavesheet.employeeId, ParameterDirection.Input));
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
        [Route("approve-status")]
        [HttpGet]
        public  ActionResult<List<DictionaryEntry>> GetApproveStatus()
        {
            List<DictionaryEntry> ApproveStatus = new List<DictionaryEntry>();
            ApproveStatus.Add(new DictionaryEntry("Phê duyệt 1", "APPROVE1"));
            ApproveStatus.Add(new DictionaryEntry("Phê duyệt 2", "APPROVE2"));
            ApproveStatus.Add(new DictionaryEntry("Hoàn duyệt", "RESET"));
            ApproveStatus.Add(new DictionaryEntry("Không duyệt", "REJECT"));
             ApproveStatus.Add(new DictionaryEntry("Hủy đơn", "CANCEL"));
            return  ApproveStatus;
        }

        [Route("leave-manuals")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveManual>>> GetLeaveManuals()
        {
            var query =  (from p in _context.LeaveManuals where p.IS_LEAVE == true select p);
            if (query == null)
            {
                return NotFound();
            }

            return await query.ToListAsync();
        }

        [Route("leave-manuals/{manualId}")]
        [HttpGet]
        public async Task<ActionResult<LeaveManual>> GetLeaveManuals(int manualId )
        {
            var objData = await _context.LeaveManuals.FindAsync(manualId);
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

        [Route("leave-times/{timeId}")]
        [HttpGet]
        public async Task<ActionResult<LeaveTime>> GetLeaveTime(int timeId)
        {
            var query = (from p in _context.LeaveTimes
                         where   p.ID== timeId
                         select new LeaveTime { ID = p.ID, CODE = p.CODE, NAME_VN = p.NAME_VN }
                       );
            if (query ==null)
            {
                return NotFound();
            }
            return await query.FirstOrDefaultAsync();
        }

        [Route("leave-times")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveTime>>> GetLeaveTimes()
        {
            var query = (from p in _context.LeaveTimes
                         join t in _context.OtherListTypes on p.TYPE_ID equals t.ID
                         where t.CODE == "LEAVE_SHIFT"
                         select new LeaveTime { ID = p.ID, CODE = p.CODE, NAME_VN = p.NAME_VN }
                       );
         
            return await query.ToListAsync();
        }
      
    }
    #endregion
}
