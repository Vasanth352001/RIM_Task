
namespace RIM_Task.Repository
{
    public interface IEmployeeRepository
    {
        ReturnModel filterEmployee(Func<Employee,bool> predicate );
        ReturnModel readEmployeeData();
    }

}