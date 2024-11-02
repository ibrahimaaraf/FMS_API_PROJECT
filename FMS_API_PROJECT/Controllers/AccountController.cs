using FMS_API_PROJECT.Models;
using FMS_API_PROJECT.Repositories;
using FMS_API_PROJECT.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FMS_API_PROJECT.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IRepository<ChartOfAccount> _accRepository;
        UserController _UserController = new UserController();

        public AccountController()
        {
            _accRepository = new Repository<ChartOfAccount>(new FMS_DB_Context());
        }

        #region [//////// chats of account /////////]

        [HttpGet]
        [Route("api/get-all-coa")]
        public async Task<IHttpActionResult> GET_ALL_COA_()
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var ch_list = await _accRepository.ExecuteStoredProcedureAsync<Coa_List>("EXEC USP_GET_ALL_COA");

                bool hasData = ch_list != null && ch_list.Any();
                return Ok(new { has_data = hasData, message = hasData ? "Data retrieved successfully." : "No data found.",
                    list = ch_list ?? new List<Coa_List>()
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/coa-posting")]
        public async Task<IHttpActionResult> COA_POSTING(int ACC_NO, int ACC_TYPE, string ACC_DESCRIPTION,int USER_ID)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ACC_NO", ACC_NO),
                    new SqlParameter("@ACC_TYPE", ACC_TYPE),
                    new SqlParameter("@ACC_DESCRIPTION", ACC_DESCRIPTION),
                    new SqlParameter("@USER_ID", USER_ID),
                    new SqlParameter
                    {
                        ParameterName = "@RESULTS",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                await _accRepository.ExecuteStoredProcedureWithOutputAsync("EXEC USP_COA_POSTING @ACC_NO, @ACC_TYPE, @ACC_DESCRIPTION, @USER_ID, @RESULTS OUT", parameters);

                // Retrieve the output value from @RESULTS parameter
                int result = (int)parameters[4].Value;

                string message;
                switch (result)
                {
                    case 1:
                        message = "Inserted successfully.";
                        break;
                    case 2:
                        message = "Already exists.";
                        break;
                    default:
                        message = "Insertion failed.";
                        break;
                }

                return Ok(new
                {
                    result = result,
                    message = message
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }
        
        [HttpPut]
        [Route("api/coa-update")]
        public async Task<IHttpActionResult> UPDATE_COA(int COA_ID, int ACC_NO, int ACC_TYPE, string ACC_DESCRIPTION, int USER_ID)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@COA_ID", COA_ID),
                    new SqlParameter("@ACC_NO", ACC_NO),
                    new SqlParameter("@ACC_TYPE", ACC_TYPE),
                    new SqlParameter("@ACC_DESCRIPTION", ACC_DESCRIPTION),
                    new SqlParameter("@USER_ID", USER_ID),
                    new SqlParameter
                    {
                        ParameterName = "@RESULTS",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                await _accRepository.ExecuteStoredProcedureWithOutputAsync("EXEC USP_UPDATE_COA @COA_ID, @ACC_NO, @ACC_TYPE, @ACC_DESCRIPTION, @USER_ID, @RESULTS OUT", parameters);

                int result = (int)parameters[5].Value;

                string message;
                switch (result)
                {
                    case 1:
                        message = "Updated successfully.";
                        break;
                    default:
                        message = "Update failed.";
                        break;
                }

                return Ok(new { has_data = result, message = message });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("api/coa-delete")]
        public async Task<IHttpActionResult> DELETE_COA(int COA_ID)
        {
            int result = 0;
            string msg = "delete failed";
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var coa = await _accRepository.GetByIdAsync(COA_ID);
                if (coa != null)
                {
                    await _accRepository.DeleteAsync(coa);
                    result = 1;
                    msg = "Deleted successfully.";
                }

                return Ok(new { has_data=result, message = msg });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        #endregion

        #region [///////////// Journal ////////////]

        [HttpGet]
        [Route("api/get-all-journal")]
        public async Task<IHttpActionResult> GET_ALL_JOURNALS()
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var ch_list = await _accRepository.ExecuteStoredProcedureAsync<Journal_list>("EXEC USP_GET_ALL_JOURNALS");
                var coa_list = await _accRepository.ExecuteStoredProcedureAsync<COA_D>("EXEC USP_GET_COA_DROP");

                bool hasData = coa_list != null && coa_list.Any();
                return Ok(new
                {
                    has_data = hasData,
                    message = hasData ? "Data retrieved successfully." : "No data found.",
                    list = ch_list ?? new List<Journal_list>(),
                    coa_list= coa_list ?? new List<COA_D>()
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/journal-posting")]
        public async Task<IHttpActionResult> JOURNAL_POSTING(int ACC_NO, int ACC_TYPE, int COA_ID, DateTime DATE, float AMOUNT, int USER_ID)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ACC_NO", ACC_NO),
                    new SqlParameter("@ACC_TYPE", ACC_TYPE),
                    new SqlParameter("@COA_ID", COA_ID),
                    new SqlParameter("@DATE", DATE),
                    new SqlParameter("@AMOUNT", AMOUNT),
                    new SqlParameter("@USER_ID", USER_ID),
                    new SqlParameter
                    {
                        ParameterName = "@RESULTS",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                await _accRepository.ExecuteStoredProcedureWithOutputAsync("EXEC USP_JOURNAL_POSTING @ACC_NO, @ACC_TYPE, @COA_ID, @DATE, @AMOUNT, @USER_ID, @RESULTS OUT", parameters);

                int result = (int)parameters[6].Value;

                string message;
                switch (result)
                {
                    case 1:
                        message = "Inserted successfully.";
                        break;
                    case 2:
                        message = "Already exists.";
                        break;
                    default:
                        message = "Insertion failed.";
                        break;
                }

                return Ok(new
                {
                    result = result,
                    message = message
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/journal-update")]
        public async Task<IHttpActionResult> UPDATE_JOURNAL(int JOURNAL_ID, int ACC_TYPE, int COA_ID, DateTime DATE, float AMOUNT, int USER_ID)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@JOURNAL_ID", JOURNAL_ID),
                    new SqlParameter("@COA_ID", COA_ID),
                    new SqlParameter("@DATE", DATE),
                    new SqlParameter("@ACC_TYPE", ACC_TYPE),
                    new SqlParameter("@AMOUNT", AMOUNT),
                    new SqlParameter("@USER_ID", USER_ID),
                    new SqlParameter
                    {
                        ParameterName = "@RESULTS",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                await _accRepository.ExecuteStoredProcedureWithOutputAsync("EXEC USP_UPDATE_JOURNAL @JOURNAL_ID, @ACC_TYPE, @COA_ID, @DATE, @AMOUNT, @USER_ID, @RESULTS OUT", parameters);

                int result = (int)parameters[6].Value;

                string message;
                switch (result)
                {
                    case 1:
                        message = "Updated successfully.";
                        break;
                    default:
                        message = "Update failed.";
                        break;
                }

                return Ok(new { has_data = result, message = message });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/journal-delete")]
        public async Task<IHttpActionResult> DELETE_JOURNAL(int JOURNAL_ID, int USER_ID)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@JOURNAL_ID", JOURNAL_ID),
                    new SqlParameter("@USER_ID", USER_ID),
                    new SqlParameter
                    {
                        ParameterName = "@RESULTS",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                await _accRepository.ExecuteStoredProcedureWithOutputAsync("EXEC USP_DELETE_JOURNAL @JOURNAL_ID, @USER_ID, @RESULTS OUT", parameters);

                int result = (int)parameters[2].Value;

                string message;
                switch (result)
                {
                    case 1:
                        message = "Deleted successfully.";
                        break;
                    default:
                        message = "Delete failed.";
                        break;
                }

                return Ok(new { has_data = result, message = message });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        #endregion

        #region [//////////// Report /////////////]

        [HttpGet]
        [Route("api/report")]
        public async Task<IHttpActionResult> GET_REPORT_DATA(DateTime ST_DATE, DateTime END_DATE)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!_UserController.ValidateJwtToken(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Invalid Token." });
                }

            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ST_DATE", ST_DATE),
                    new SqlParameter("@END_DATE", END_DATE),
                    
                };
                var ch_list = await _accRepository.ExecuteStoredProcedureAsync<FMS_Report>("EXEC RPT_GET_REPORT_DATA @ST_DATE, @END_DATE", parameters);

                bool hasData = ch_list != null && ch_list.Any();
                return Ok(new
                {
                    has_data = hasData,
                    message = hasData ? "Data retrieved successfully." : "No data found.",
                    list = ch_list ?? new List<FMS_Report>(),
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving data: {ex.Message}");
            }
        }

        #endregion
    }
}
