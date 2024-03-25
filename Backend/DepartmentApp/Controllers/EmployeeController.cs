using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DepartmentApp.Models;
namespace DepartmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController (IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

        }

        [HttpGet]
        public JsonResult GetEmployee()
        {
            string query = @"
            select EmployeeId, EmployeeName,Department,
            convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFileName from
            dbo.Employee
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query,mySqlCon))
                {
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult AddEmployee(Employee emp)
        {
            string query = @"
            Insert into dbo.Employee
            (EmployeeName,Department,DateOfJoining, PhotoFileName)
            values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, mySqlCon))
                {

                    sqlCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", emp.EmployeeDepartment);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult("Employee added successfully.");
        }

        [HttpPut]
        public JsonResult UpdateEmployee(Employee emp)
        {
            string query = @"
            Update dbo.Employee 
            set EmployeeName = @EmployeeName,
            EmployeeDepartment = @EmployeeDepartment,
            DateOfJoining = @DateOfJoining,
            PhotoFileName = @PhotoFileName,
            where EmployeeId=@EmployeeId
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, mySqlCon))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", emp.EmployeeDepartment);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult("Employee updated successfully.");
        }

        [HttpDelete]
        public JsonResult RemoveEmployee(int employeeId)
        {
            string query = @"
            Delete from dbo.Employee
            where EmployeeId=@EmployeeId
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, mySqlCon))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult("Employee removed successfully.");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile() 
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "\\Photos\\" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                
                return new JsonResult("anonymous.jpg");
            }
        }
    }
}
