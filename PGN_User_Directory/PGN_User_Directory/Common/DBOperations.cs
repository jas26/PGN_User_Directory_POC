using System;
//using System.Data.SQLite;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using pgn_userprofile.Model;
using MySql.Data.MySqlClient;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace pgn_userprofile.Common
{
	public class DBOperations
	{
        //string connection_string;
        //SQLiteConnection sqlite3con;
        MySqlConnection conn;

        public DBOperations()
		{
            string mysqlcon_string = "server=127.0.0.1;uid=root;pwd=#welcome123;database=pgnmysql";
            //var proj_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location+ "pgn.db");
            /////Users/jaswanth/Education/PGN/POC/PGN
            //connection_string = $"Data Source={proj_path};Version=3;";
            //sqlite3con = new SQLiteConnection(connection_string);
            conn = new MySqlConnection();
            conn.ConnectionString = mysqlcon_string;


        }
        public async Task<UserModel> Create(UserModel usermodel)
        {

            conn.Open();
            var cmdString = "INSERT INTO user values(" + usermodel.userid + ",'" + usermodel.username + "','" + usermodel.location + "')";

            MySqlCommand cmd = new MySqlCommand(cmdString, conn);
            //cmd.ExecuteNonQuery();
            //SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO user (userid, username, location) VALUES (?,?,?)", sqlite3con);
            //insertSQL.Parameters.Add(usermodel.userid);
            //insertSQL.Parameters.Add(usermodel.username);
            //insertSQL.Parameters.Add(usermodel.location);
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

            var cmdString = "select * from user where userid =" + userid ;

            MySqlCommand cmd = new MySqlCommand(cmdString, conn);

            //cmd.ExecuteNonQuery();
            //SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO user (userid, username, location) VALUES (?,?,?)", sqlite3con);
            //insertSQL.Parameters.Add(usermodel.userid);
            //insertSQL.Parameters.Add(usermodel.username);
            //insertSQL.Parameters.Add(usermodel.location);
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

