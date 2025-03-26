using System.Data;
using System.Net;
using OfficeOpenXml;
using RIM_Task.Services;
using RIM_Task.Utility;

namespace RIM_Task.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IConfiguration _config;
        public EmployeeRepository(IConfiguration config)
        {
            _config = config;
        }
        public ReturnModel readEmployeeData()
        {
            try
            {
                List<Employee> employees = new List<Employee>();
                FileInfo file = new FileInfo($"{_config["FolderPath:EmployeeDetailsFileFolder"]}/Employees.xlsx");

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var emp = new Employee
                        {
                            EmployeeID = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                            EmployeeCode = worksheet.Cells[row, 2].Text,
                            Name = worksheet.Cells[row, 3].Text,
                            Company = worksheet.Cells[row, 4].Text,
                            Department = worksheet.Cells[row, 5].Text,
                            Designation = worksheet.Cells[row, 6].Text,
                            ReportId = Convert.ToInt32(worksheet.Cells[row, 7].Value),
                            DOB = Convert.ToDateTime(worksheet.Cells[row, 8].Value),
                            MobileNo = worksheet.Cells[row, 9].Text,
                            EmailId = worksheet.Cells[row, 10].Text,
                            ActiveStatus = worksheet.Cells[row, 11].Text
                        };
                        employees.Add(emp);
                    }
                }

                return new ReturnModel
                {
                    IsSuccess = true,
                    Reason = "",
                    Response = employees,
                    httpStatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Error occured in read Employee data {ex.Message}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    
        public ReturnModel filterEmployee(Func<Employee,bool> predicate )
        {
            try
            {
                ReturnModel returnModel = readEmployeeData();
                if(returnModel.IsSuccess == true)
                {
                    List<Employee> employees = (List<Employee>)readEmployeeData().Response;
                    List<Employee> filteredEmployees = employees.Where(predicate).ToList();
                    foreach(var employee in filteredEmployees)
                    {
                        Console.WriteLine(employee.Name);
                    }
                    return new ReturnModel
                    {
                        IsSuccess = true,
                        Reason = "",
                        Response = filteredEmployees,
                        httpStatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return returnModel;
                }
            }
            catch(ApplicationException applicationException)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Error occured while filtering the employee from list of employees data {applicationException}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.BadRequest
                };
            }
            catch(Exception exception)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Error occured in filter Employee data {exception.Message}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}