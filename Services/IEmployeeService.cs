

namespace RIM_Task.Services
{
    public interface IEmployeeService
    {
        List<Employee> filterEmployee(EmployeeFilter filter, string resultFileType);
    }
}