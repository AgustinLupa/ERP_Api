using ERP.Api.Entity;

namespace ERP.Api.Repository.Contracts;

public interface IEmployeeRepository
{
    public Task<Employee> GetByCode(int code_Employee);
    public Task<IEnumerable<Employee>> GetAll();
    public Task<IEnumerable<Employee>> GetActiveEmployee();
    public Task<int> CreateEmployee(Employee employee);
    public Task<int> DeleteEmployee(int code_Employee);
    public Task<int> UpdateEmployee(Employee employee);
    public Task<IEnumerable<Employee>> GetByName(string name);
}
