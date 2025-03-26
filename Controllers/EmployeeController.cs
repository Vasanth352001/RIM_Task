using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var result = _employeeService.filterEmployee(employeeFilter, resultFileType);
            if(result.IsSuccess == true)
            {
                if(resultFileType.ToLower().Equals("xlsx"))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(result.fileDetail.filePath);
                    FileContentResult employeeDetailsFile = File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.fileDetail.fileName);
                    return employeeDetailsFile;
                }
                else
                {
                    var fileBytes = System.IO.File.ReadAllBytes(result.fileDetail.filePath);
                    FileContentResult file = File(fileBytes, "text/csv", result.fileDetail.fileName);
                    return file;
                }
            }
            else if(result.httpStatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.Reason);
            }
            else if(result.httpStatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result.Reason);
            }    
            else
            {
                return StatusCode(500, new { message = result.Reason, error = result.Reason });
            }
        }

    }
}