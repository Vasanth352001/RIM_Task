
public class Employee
{
    public int EmployeeID { get; set; }
    public string EmployeeCode { get; set; }
    public string Name { get; set; } 
    public string Company { get; set; } 
    public string Department { get; set; } 
    public string Designation { get; set; } 
    public int ReportId { get; set; } 
    public DateTime DOB { get; set; } 
    public string MobileNo { get; set; } 
    public string EmailId { get; set; } 
    public string ActiveStatus { get; set; }
}

public class EmployeeFilter
{
    public string Company { get; set; } 
    public string Department { get; set; } 
    public string Designation { get; set; }
    public string ActiveStatus { get; set; }
}