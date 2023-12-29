using ERP.Api.Entity;
using ERP.Api.Models.Request;
using ERP.Api.Utils;

namespace ERP.Api.Models.Tools;

public static class Mapper
{
    public static Employee MapToEmployeeDTO(this SaveEmployee employee)
    {
        return new Employee
        {
            Name = employee.Name,
            Code_Employee = employee.Code_Employee,
            Dni = employee.Dni,
            LastName = employee.LastName,
            State = employee.State
        };
    }

    public static Employee MapToEmployeeDTO(this SaveEmployee employee, int id)
    {
        return new Employee
        {
            Id = id,
            Name = employee.Name,
            Code_Employee = employee.Code_Employee,
            Dni = employee.Dni,
            LastName = employee.LastName,
            State = employee.State
        };
    }

    public static Supplier MapToSupplierDTO(this SaveSupplier supplier)
    {
        return new Supplier
        {
            Name = supplier.Name,
            Adress = supplier.Address,
            Phone = supplier.Phone,
            State = supplier.State,
        };
    }

    public static Supplier MapToSupplierDTO(this SaveSupplier supplier, int id)
    {
        return new Supplier
        {
            Id = id,
            Name = supplier.Name,
            Adress = supplier.Address,
            Phone = supplier.Phone,
            State = supplier.State,
        };
    }

    public static User MapToUserDTO(this LoginCredentials credentials)
    {
        return new User { Name = credentials.username, Password = credentials.password };
    }

    public static User MapToUserDTO(this SaveUser user)
    {
        return new User
        {
            Name = user.username,
            Password = KeySha256.CalculateSHA256(user.password),
            Id_Role = (int)user.id_role
        };
    }

    public static User MapToUserDTO(this EditUser user, int id)
    {
        return new User
        {
            Id = id,
            Name = user.username,
            Id_Role = (int)user.id_role,
            State = user.state
        };
    }


}
