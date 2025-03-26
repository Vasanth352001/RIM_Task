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
        public IActionResult GetEmployees([FromBody] EmployeeFilter employeeFilter, string resultFileType)
        {
            try
            {
                return Ok(_employeeService.filterEmployee(employeeFilter, resultFileType));
            } 
            catch(ArgumentException argumentException)
            {
                return BadRequest(new { message = argumentException.Message });
            }
            catch(Exception exception)
            {
                return StatusCode(500, new { message = "Internal Server Errorr", error = exception.Message });
            }

        }
    }
}