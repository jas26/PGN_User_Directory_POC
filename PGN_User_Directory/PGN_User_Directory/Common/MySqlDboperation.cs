using System;
using MySql.Data.MySqlClient;
using pgn_userprofile.Model;

namespace PGN_User_Directory.Common
{
	public class MySqlDboperation
	{
        IConfiguration _configuration;
        MySqlConnection conn;

        public MySqlDboperation(IConfiguration configuration)
		{
            _configuration = configuration;
            conn = new MySqlConnection();
            conn.ConnectionString = _configuration.GetSection("MySql").GetSection("Connection_String").Value;
        }

        public async Task<UserModel> Create(UserModel usermodel)
        {
            conn.Open();
            var cmdString = "INSERT INTO user values(" + usermodel.userid + ",'" + usermodel.username + "','" + usermodel.location + "')";
            MySqlCommand cmd = new MySqlCommand(cmdString, conn);
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
                return usermodel;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }
        public async Task<Dictionary<string, object>> getuserdetailsfromdb(int userid)
        {
            MySqlDataReader reader;
            conn.Open();
            var cmdString = "select * from user where userid =" + userid;
            MySqlCommand cmd = new MySqlCommand(cmdString, conn);
            try
            {
                reader = cmd.ExecuteReader();

                var userdata = dbvaluestojsonconvertor(reader);
                conn.Close();
                return userdata;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<string, object> dbvaluestojsonconvertor(MySqlDataReader reader)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    data.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            return data;
        }
    }
}

