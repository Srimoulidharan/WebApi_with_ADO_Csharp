using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using StudentWebApi.Models;
namespace StudentWebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentController : Controller
    {
        public String _connectionString;
        public StudentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpPost("add")]
        public ActionResult<Student> AddStudent(Student student)
        {
            var connection = new MySqlConnection(_connectionString);  
            connection.Open();
            String query = "Insert into student (Id,Name,Department,PhoneNumber) VALUES (@Id,@Name,@Department,@PhoneNumber)";
            var cmd= new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", student.Id);
            cmd.Parameters.AddWithValue("@Name", student.Name);
            cmd.Parameters.AddWithValue("@Department", student.Department);
            cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
            int res= cmd.ExecuteNonQuery();
            if(res> 0)
            {
                return Ok("Student added successfully");
            }
            else
            {
                return BadRequest("Student Not added ");
            }
            connection.Close();
        }
        [HttpGet("viewall")]
        public ActionResult<List<Student>> ViewallStudents() { 
            
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            String query = "select * from student";
            List<Student> students = new List<Student>();
            var cmd=new MySqlCommand(query, connection);
            var res= cmd.ExecuteReader();
            while (res.Read()) { 
               var student=new Student
               {
                   Id = res.GetInt32("Id"),
                   Name = res.GetString("Name"),
                   Department = res.GetString("Department"),
                   PhoneNumber = res.GetInt64("PhoneNumber")
               };
                students.Add(student);
            }
            
            return Ok(students);
        }
        [HttpGet("{id}")]

        public ActionResult<Student> ViewStudents(int  id)
        {

            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            String query = "select * from student where Id=@Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var res = cmd.ExecuteReader();
            if (res.Read()) 
            {
                var student = new Student
                {
                    Id = res.GetInt32("Id"),
                    Name = res.GetString("Name"),
                    Department = res.GetString("Department"),
                    PhoneNumber = res.GetInt64("PhoneNumber")
                };
                return Ok(student);
            }
            else
            {
                return BadRequest("student not found");
            }
        }
        [HttpPut("update")]
        public ActionResult<Student> UpdateStudent(Student student) {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            String query = "Update student SET Name=@Name,Department=@Department,PhoneNumber=@PhoneNumber where Id=@Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", student.Id);
            cmd.Parameters.AddWithValue("@Name", student.Name);
            cmd.Parameters.AddWithValue("@Department", student.Department);
            cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                return Ok("Student Updated successfully");
            }
            else
            {
                return BadRequest("Student with  given Id not Updated ");
            }
            connection.Close();
        }

        [HttpDelete("id")]
        public ActionResult DeleteStudent(int id)
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            String query = "Delete from student where Id=@Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id",id);
            int res = cmd.ExecuteNonQuery();
            if (res > 0) {
                return Ok("student Deleted");
                connection.Close();
            }
            else
            {
                return BadRequest("Student not deleted");

            }

        }

    }
}