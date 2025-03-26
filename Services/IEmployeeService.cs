

namespace RIM_Task.Services
{
    public interface IEmployeeService
    {
        ReturnModel filterEmployee(EmployeeFilter filter, string resultFileType);
    }
}