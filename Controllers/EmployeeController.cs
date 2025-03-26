using System.Net;
using Microsoft.AspNetCore.Mvc;
using RIM_Task.Services;


namespace RIM_Task.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    public class Employee : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public Employee(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("getEmployee")]
        public ReturnModel GetEmployees([FromBody] EmployeeFilter employeeFilter, string resultFileType)
        {
            try
            {
                return new ReturnModel
                {
                    IsSuccess = true,
                    Reason = "",
                    Response = _employeeService.filterEmployee(employeeFilter, resultFileType),
                    httpStatusCode = HttpStatusCode.OK
                };
            } 
            catch(ArgumentException argumentException)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = argumentException.Message,
                    Response = null,
                    httpStatusCode = HttpStatusCode.BadRequest
                };
            }
            catch(Exception exception)
            {
                return new ReturnModel
                {
                    IsSuccess = false,
                    Reason = $"Internal Server Error {exception.Message}",
                    Response = null,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}