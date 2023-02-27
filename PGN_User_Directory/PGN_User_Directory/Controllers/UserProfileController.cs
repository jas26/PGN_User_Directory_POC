using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using pgn_userprofile.Common;
using pgn_userprofile.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pgn_userprofile.Controllers
{
    [ApiController]
    [Route("calci")]
    public class UserProfileController : Controller
    {
        DBOperations dBOperations;
        public UserProfileController()
        {
            dBOperations = new DBOperations();
        }
        [HttpPost("adduser")]
        public async Task<IActionResult> createuser([FromBody]UserModel usermodel)
        {
            try
            {
                var create_db_response = await dBOperations.Create(usermodel);
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
            try
            {
                var userdata = await dBOperations.getuserdetailsfromdb(userid);

                if (userdata.Count() != 0)
                {
                    return Ok(userdata);
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

