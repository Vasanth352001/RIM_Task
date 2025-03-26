
namespace RIM_Task.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> filterEmployee(Func<Employee,bool> predicate );
        List<Employee> readEmployeeData();
    }

}