using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Ems
{
    [ApiController]
    [Route("api/[controller]")]
    public class EMSController : ControllerBase
    {
        private string _connectionString;

        public EMSController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpPost("add")]
        public ActionResult<EMS> AddEmployee(EMS emp)
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "INSERT INTO employee(Name, Salary, Department) VALUES(@name, @salary, @dept)";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", emp.Name);
            cmd.Parameters.AddWithValue("@salary", emp.Salary);
            cmd.Parameters.AddWithValue("@dept", emp.Department);
            int res = cmd.ExecuteNonQuery();
            if(res > 0)
            {
                return Ok(emp);
            }
            else
            {
                return BadRequest("Employee not added");
            }
        }
        [HttpPut("update")]
        public ActionResult UpdateEmployee( EMS emp)
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "UPDATE employee SET Name = @name, Salary = @salary, Department = @dept WHERE Id = @id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", emp.Id);
            cmd.Parameters.AddWithValue("@name", emp.Name);
            cmd.Parameters.AddWithValue("@salary", emp.Salary);
            cmd.Parameters.AddWithValue("@dept", emp.Department);
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                return Ok("Employee updated");
            }
            else
            {
                return BadRequest("Employee not added");
            }

        }
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "DELETE FROM employee WHERE Id = @id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                return Ok("Employee deleted");
            }
            else
            {
                return BadRequest("Employee not added");
            }
        }

        [HttpGet("all")]
        public ActionResult<List<EMS>> ViewAll()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT * FROM employee";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();
            var list = new List<EMS>();
            while (reader.Read())
            {
                list.Add(new EMS
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Salary = reader.GetString("salary"),
                    Department = reader.GetString("department")
                });
            }
            return Ok(list);
        }
        [HttpGet("{id}")]
        public ActionResult<EMS> ViewOne(int id)
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT * FROM employee WHERE Id = @id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var emp = new EMS
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Salary = reader.GetString("salary"),
                    Department = reader.GetString("department")
                };
                return Ok(emp);
            }
            return NotFound("Employee not found");
        }
    }
}
