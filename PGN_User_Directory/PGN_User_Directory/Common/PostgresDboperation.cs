using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Npgsql;
using pgn_userprofile.Model;

namespace PGN_User_Directory.Common
{
	public class PostgresDboperation
	{
		IConfiguration _configuration;
        private string postgres_con_st;

        public PostgresDboperation(IConfiguration configuration)
		{
			_configuration = configuration;
            postgres_con_st = _configuration.GetSection("Postgres").GetSection("Connection_String").Value;
        }
        public async Task<UserModel> Create(UserModel usermodel)
        {
            try
            {
                using (var connection = new NpgsqlConnection(postgres_con_st))
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO userprofile values(" + usermodel.userid + ",'" + usermodel.username + "','" + usermodel.location + "')";
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return usermodel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Dictionary<string, object>> getuserdetailsfromdb(int userid)
        {
            Dictionary<string, object> userdata = new Dictionary<string, object>();
            try
            {
                using (var connection = new NpgsqlConnection(postgres_con_st))
                {
                    connection.Open();
                    using (var npgsqlcmd = new NpgsqlCommand())
                    {
                        npgsqlcmd.Connection = connection;
                        npgsqlcmd.CommandText = "select * from userprofile where userid =" + userid;
                        using (var da = new NpgsqlDataAdapter(npgsqlcmd))
                        {
                            var dt = new DataTable();
                            da.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn dataColumn in dt.Columns)
                                {
                                    userdata.Add(dataColumn.ColumnName, row[dataColumn.ColumnName]);
                                }
                            }
                        }
                    }
                    return userdata;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

