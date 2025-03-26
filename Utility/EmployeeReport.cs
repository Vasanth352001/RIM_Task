using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Net;
using System.Text;

namespace RIM_Task.Utility
{
    public class EmployeeReport
    {

        private IConfiguration _config;
        public EmployeeReport(IConfiguration config)
        {
            _config = config;
        }

        public ReturnModel GenerateExcelReport(List<Employee> filteredEmployees)
        {
            try
            {
                string fileName = $"EmployeeList_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
                string filePath = $@"{_config["FolderPath:ResultFileFolder"]}/{fileName}";
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Employee Report");
                    worksheet.Cells["A1"].Value = "Employee ID";
                    worksheet.Cells["B1"].Value = "Employee Code";
                    worksheet.Cells["C1"].Value = "Name";
                    worksheet.Cells["D1"].Value = "Company";
                    worksheet.Cells["E1"].Value = "Department";
                    worksheet.Cells["F1"].Value = "Designation";
                    worksheet.Cells["G1"].Value = "Report ID";
                    worksheet.Cells["H1"].Value = "Date of Birth";
                    worksheet.Cells["I1"].Value = "Mobile No";
                    worksheet.Cells["J1"].Value = "Email ID";
                    worksheet.Cells["K1"].Value = "Active Status";

                    int row = 2;
                    foreach (var employee in filteredEmployees)
                    {
                        worksheet.Cells[row, 1].Value = employee.EmployeeID;
                        worksheet.Cells[row, 2].Value = employee.EmployeeCode;
                        worksheet.Cells[row, 3].Value = employee.Name;
                        worksheet.Cells[row, 4].Value = employee.Company;
                        worksheet.Cells[row, 5].Value = employee.Department;
                        worksheet.Cells[row, 6].Value = employee.Designation;
                        worksheet.Cells[row, 7].Value = employee.ReportId;
                        worksheet.Cells[row, 8].Value = employee.DOB.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 9].Value = employee.MobileNo;
                        worksheet.Cells[row, 10].Value = employee.EmailId;
                        worksheet.Cells[row, 11].Value = employee.ActiveStatus;
                        row++;
                    }

                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                }
                
                return new ReturnModel
                {
                    IsSuccess = true,
                    Reason = "",
                    Response = $"Excel report saved to: {filePath}",
                    httpStatusCode = HttpStatusCode.OK,
                    fileDetail = new FileDetail
                    {
                        fileName = fileName,
                        filePath = filePath
                    }
                };
            }
            catch(Exception ex)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Error occured while generate Excel file {ex}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public ReturnModel GenerateCSVReport(List<Employee> filteredEmployees)
        {
            try
            {
                string fileName = $"EmployeeList_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                string filePath = $@"{_config["FolderPath:ResultFileFolder"]}/{fileName}";
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.WriteLine("Employee ID,Employee Code,Name,Company,Department,Designation,Report ID,Date of Birth,Mobile No,Email ID,Active Status");

                    foreach (var emp in filteredEmployees)
                    {
                        writer.WriteLine($"{emp.EmployeeID},{emp.EmployeeCode},{emp.Name},{emp.Company},{emp.Department},{emp.Designation},{emp.ReportId},{emp.DOB.ToString("yyyy-MM-dd")},{emp.MobileNo},{emp.EmailId},{(emp.ActiveStatus)}");
                    }
                }
                return new ReturnModel
                {
                    IsSuccess = true,
                    Reason = "",
                    Response = $"CSV report saved to: {filePath}",
                    httpStatusCode = HttpStatusCode.OK,
                    fileDetail = new FileDetail
                    {
                        fileName = fileName,
                        filePath = filePath
                    }
                };
            }
            catch(Exception exception)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Error occured while generate CSV file {exception}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    
    }
}