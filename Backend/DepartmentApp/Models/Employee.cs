﻿namespace DepartmentApp.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeDepartment { get; set; }
    public DateTime DateOfJoining { get; set; }
    public string PhotoFileName { get; set; }
}
