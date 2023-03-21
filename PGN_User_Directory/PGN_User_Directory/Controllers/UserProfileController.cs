using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PGN_User_Directory.Common;
using pgn_userprofile.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pgn_userprofile.Controllers
{
    [ApiController]
    [Route("pgn")]
    public class UserProfileController : Controller
    {
        MySqlDboperation mySqlDboperation;
        PostgresDboperation postgresDboperation;
        private IConfiguration _configuration;
        private string currentDB;

        public UserProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
            mySqlDboperation = new MySqlDboperation(_configuration);
            postgresDboperation = new PostgresDboperation(_configuration);
            currentDB = _configuration.GetSection("PGNDB").Value;

        }
        [HttpPost("adduser")]
        public async Task<IActionResult> createuser([FromBody]UserModel usermodel)
        {
            UserModel create_db_response;
            try
            {
                if (currentDB == "MySql")
                {
                    create_db_response = await mySqlDboperation.Create(usermodel);
                }
                else
                {
                    create_db_response = await postgresDboperation.Create(usermodel);
                }
                return Ok(create_db_response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            

        }
        [HttpGet("getuserdetails")]
        public async Task<IActionResult> getuserdetails([FromQuery] int userid)
        {
            Dictionary<string, object> get_db_response;
            try
            {
                if (currentDB == "MySql")
                {
                    get_db_response = await mySqlDboperation.getuserdetailsfromdb(userid);
                }
                else
                {
                    get_db_response = await postgresDboperation.getuserdetailsfromdb(userid);
                }
                if (get_db_response.Count() != 0)
                {
                    return Ok(get_db_response);
                }
                else
                {
                    return NotFound("usernotfound");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}

