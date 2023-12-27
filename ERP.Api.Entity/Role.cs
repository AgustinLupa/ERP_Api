namespace ERP.Api.Entity;

public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int State { get; set; }
    public List<RoleXPermissions> RolePermissions { get; set; }
}
