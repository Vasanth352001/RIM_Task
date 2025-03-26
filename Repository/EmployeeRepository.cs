using System.Data;
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
        public List<Employee> readEmployeeData()
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
                            ActiveStatus = worksheet.Cells[row, 11].Text.ToLower() == "active"
                        };
                        employees.Add(emp);
                    }
                }

                return employees;
            }
            catch (FileNotFoundException ex)
            {
                throw new ApplicationException("Excel file is missing. Please upload a valid file.", ex);
            }
            catch (InvalidDataException ex)
            {
                throw new ApplicationException("The Excel worksheet is empty or missing.", ex);
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("Excel file format is incorrect. Please check the headers and data.", ex);
            }
            catch (IOException ex)
            {
                throw new ApplicationException("Error reading the Excel file. The file might be open in another application.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while reading the Excel file.", ex);
            }
        }
    
        public List<Employee> filterEmployee(Func<Employee,bool> predicate )
        {
            try
            {
                List<Employee> employees = readEmployeeData();
                List<Employee> filteredEmployees = employees.Where(predicate).ToList();
                foreach(var employee in filteredEmployees)
                {
                    Console.WriteLine(employee.Name);
                }
                return filteredEmployees;
            }
            catch(ApplicationException applicationException)
            {
                throw new ApplicationException("Error occured while filtering the employee from list of employees data", applicationException);
            }
        }
    }
}