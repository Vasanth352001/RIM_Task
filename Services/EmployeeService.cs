using System.Data;
using System.Net;
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
    
        public ReturnModel filterEmployee(EmployeeFilter filter, string resultFileType)
        {
            try
            {
                var filteredEmployeesResponse = _employeeRepository.filterEmployee(data => (data.Company == filter.Company) && (data.Department == filter.Department) &&
                (data.Designation == filter.Designation) && (data.ActiveStatus == filter.ActiveStatus));
                
                if(filteredEmployeesResponse.IsSuccess == true)
                {
                    List<Employee> filteredEmployees = (List<Employee>) filteredEmployeesResponse.Response; 
                    if(filteredEmployees.Count() > 0)
                    {

                        if(resultFileType.ToLower().Equals("csv"))
                        {
                            var result = employeeReport.GenerateCSVReport(filteredEmployees);
                            if(result.IsSuccess == true)
                            {
                                return new ReturnModel
                                {
                                    IsSuccess = true,
                                    Reason = "",
                                    Response = filteredEmployees,
                                    httpStatusCode = HttpStatusCode.OK,
                                    fileDetail = result.fileDetail
                                };
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else if(resultFileType.ToLower().Equals("xlsx"))
                        {
                            var result = employeeReport.GenerateExcelReport(filteredEmployees);
                            if(result.IsSuccess == true)
                            {
                                return new ReturnModel
                                {
                                    IsSuccess = true,
                                    Reason = "",
                                    Response = filteredEmployees,
                                    httpStatusCode = HttpStatusCode.OK,
                                    fileDetail = result.fileDetail
                                };
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return new ReturnModel
                            {
                                IsSuccess = false,
                                Reason = "Enter correct result file format",
                                Response = null,
                                httpStatusCode = HttpStatusCode.BadRequest
                            };
                        }
                    }
                    else
                    {
                        return new ReturnModel
                        {
                            IsSuccess = false,
                            Reason = "No Data",
                            Response = null,
                            httpStatusCode = HttpStatusCode.NotFound
                        };
                    }
                }
                else
                {
                    return filteredEmployeesResponse;
                }
                
            }
            catch(Exception exception)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Error filtering employees from repositories {exception}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

    }
 }