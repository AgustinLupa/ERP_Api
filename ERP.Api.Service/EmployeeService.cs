using Dapper;
using ERP.Api.Entity.Contracts;
using MySql.Data.MySqlClient;
using SystemERP.Model;

namespace ERP.Api.Service;

public class EmployeeService : IEmployeeService
{
    private readonly Context _context;

    public EmployeeService(Context context)
    {
        _context = context;
    }

    public async Task<Employee> GetByCode(int code_Employee)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                var mysql = @"SELECT id, name, lastname, state, dni, code_employee FROM employee where (code_employee = @code_Employee)";
                var result = await connection.QuerySingleOrDefaultAsync<Employee>(mysql, new { code_Employee });               
                return result;
            }
            catch (Exception)
            {
                return new Employee();
            }
        }
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                var mysql = @"SELECT id, name, lastname, state, dni, code_employee FROM employee";
                var result = await connection.QueryAsync<Employee>(mysql);
                return result;
            }
            catch (Exception)
            {
                return new List<Employee>();
            }
        }
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployee()
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                var mysql = @"SELECT id, name, lastname, state, dni, code_employee FROM employee where (state = 1)";
                var result = await connection.QueryAsync<Employee>(mysql);
                return result;
            }
            catch (Exception)
            {
                return new List<Employee>();
            }
        }
    }

    public async Task<int> CreateEmployee(Employee employee)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                var mysql = @"INSERT INTO employee(name, lastname, dni, code_employee) 
                                  Values (@Name, @LastName, @Dni, @Code_Employee);";
                var result = await connection.ExecuteAsync(mysql, employee);
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

    public async Task<int> DeleteEmployee(int code_Employee)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                var mysql = @"Update employee Set state = 0 where(code_employee = @Code_Employee);";
                var result = await connection.ExecuteAsync(mysql, new { Code_Employee = code_Employee});
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

    public async Task<int> UpdateEmployee(Employee employee)
    {
        using (var connection = _context.CreateConnection())
        {
            try
            {
                var mysql = @"Update employee Set state = @State, name= @name,
                lastname=@LastName, code_employee = @Code_Employee, dni = @Dni
                where(id = @Id);";
                var result = await connection.ExecuteAsync(mysql, employee);
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
