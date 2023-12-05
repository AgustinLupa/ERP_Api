using SystemERP.Model;

namespace ERP.Api.Entity.Contracts;

public interface IEmployeeService
{
    public Task<Employee> GetByCode(int code_Employee);
    public Task<IEnumerable<Employee>> GetAll();
    public Task<IEnumerable<Employee>> GetActiveEmployee();
    public Task<int> CreateEmployee(Employee employee);
    public Task<int> DeleteEmployee(int code_Employee);
    public Task<int> UpdateEmployee(Employee employee);
    public Task<IEnumerable<Employee>> GetByName(string name);
}
