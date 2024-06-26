﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DepartmentApp.Models;
namespace DepartmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DepartmentController (IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult GetDepartment()
        {
            string query = @"
            select DepartmentId, DepartmentName from
            dbo.Department
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
        public JsonResult AddDepartment(Department dep)
        {
            string query = @"
            Insert into dbo.Department values (@DepartmentName)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, mySqlCon))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult("Department added successfully.");
        }

        [HttpPut]
        public JsonResult UpdateDepartment(Department dep)
        {
            string query = @"
            Update dbo.Department 
            set DepartmentName = @DepartmentName
            where DepartmentId=@DepartmentId
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, mySqlCon))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult("Department updated successfully.");
        }

        [HttpDelete]
        public JsonResult RemoveDepartment(int departmentId)
        {
            string query = @"
            Delete from dbo.Department 
            where DepartmentId=@DepartmentId
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DepartmentAppCon");
            SqlDataReader sqlReader;
            using (SqlConnection mySqlCon = new SqlConnection(sqlDataSource))
            {
                mySqlCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, mySqlCon))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    sqlReader = sqlCommand.ExecuteReader();
                    table.Load(sqlReader);
                    sqlReader.Close();
                    mySqlCon.Close();
                }
            }
            return new JsonResult("Department removed successfully.");
        }
    }
}
