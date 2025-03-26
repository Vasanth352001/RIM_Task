 using OfficeOpenXml;
using RIM_Task.Repository;
using RIM_Task.Utility;

namespace RIM_Task.Services
 {
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private IConfiguration _config;
        EmployeeReport employeeReport;

        public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration config)
        {
            _config = config;
            _employeeRepository = employeeRepository;
            employeeReport = new EmployeeReport(_config);
        }
    
        public List<Employee> filterEmployee(EmployeeFilter filter, string resultFileType)
        {
            try
            {
                List<Employee> filteredEmployees = _employeeRepository.filterEmployee(data => (data.Company == filter.Company) && (data.Department == filter.Department) &&
                (data.Designation == filter.Designation) && (data.ActiveStatus == filter.ActiveStatus));

                if(resultFileType.ToLower().Equals("csv"))
                {
                    employeeReport.GenerateCSVReport(filteredEmployees);
                    return filteredEmployees;
                }
                else if(resultFileType.ToLower().Equals("xlsx"))
                {
                    employeeReport.GenerateExcelReport(filteredEmployees);
                    return filteredEmployees;
                }
                else
                {
                    throw new Exception("Enter correct result file format");
                }
            }
            catch(Exception exception)
            {
                throw new ApplicationException("Error filtering employees from repositories", exception);
            }
        }

    }
 }