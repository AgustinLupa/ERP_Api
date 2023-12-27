namespace ERP.Api.Entity;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public int State { get; set; }
    public int Id_Role { get; set; }
    public Role? Role { get; set; }= new Role();
}
