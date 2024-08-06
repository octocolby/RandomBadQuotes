using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;


namespace RandomQuotes.Controllers
{
    public class SQLIController : Controller
    {
        // testing normal: /sqli?name=Andrew
        // testing exploit: /sqli?name=Octopus%27%20or%20%271%27==%271
        [HttpGet("sqli")]
        public IActionResult Get(string name)
        {
            string clause = "";
            List<string> list = new List<string>();
            if (name.StartsWith("Octopus"))
            {
                list.Add("FirstName == '" + name + "';");
            }
            else
            {
                list.Add("FirstName == 'Andrew';");
            }
            
            SQLiteConnection conn2 = new SQLiteConnection("Data Source=Chinook_Sqlite.sqlite");  
            conn2.Open();  
  
            SQLiteCommand cmd2 = new SQLiteCommand(conn2);  
            string whereClause = "where " + string.Join(" OR ", list);

            cmd2.CommandText = "select * from Employee " + whereClause;
            Console.WriteLine(cmd2.CommandText);
            SQLiteDataReader reader = cmd2.ExecuteReader();  
  
            
            List<string> res = new List<string>();
            while (reader.Read())
            {
                Console.WriteLine(reader["FirstName"].ToString());
                res.Add(reader["FirstName"].ToString());
            }  
  
            reader.Close();  
  
            //cmd.CommandText = "delete from Customer where CustomerID = 33";  
            //cmd.ExecuteScalar(); 

                
            return Ok(res);
        }
    }
}
