using Dapper;
using ERP.Api.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Api.Service
{
    public class RoleService
    {
        private readonly Context _context;

        public RoleService(Context context)
        {
            _context = context;
        }
        public  async Task<IEnumerable<Role>> GetAll()
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {                    
                    var query = @"
                    SELECT r.id, r.name, r.state,
                    rp.id, rp.id_role, rp.id_permissions, rp.add, rp.remove, rp.edit,
                    p.id, p.description, p.state
                    FROM roles r
                    JOIN rolesxpermissions rp ON r.id = rp.id_role
                    JOIN permissions p ON rp.id_permissions = p.id";
                    var roleDictionary =  new Dictionary<int, Role>();
                    await connection.QueryAsync<Role, RoleXPermissions, Permissions, Role>(
                        query,
                        (role, rolePermissions, permissions) =>
                        {
                            if (!roleDictionary.TryGetValue(role.Id, out var roleEntry))
                            {
                                roleEntry = role;
                                roleEntry.RolePermissions = new List<RoleXPermissions>();
                                roleDictionary.Add(roleEntry.Id, roleEntry);
                            }
                            rolePermissions.Permission = permissions;
                            roleEntry.RolePermissions.Add(rolePermissions);
                            return roleEntry;
                        },
                        splitOn: "Id, Id, Id"
                    );                    
                    return roleDictionary.Values;
                }
                catch (Exception)
                {                   
                    List<Role> roles = new List<Role>();
                    return roles;
                }
            }
        }

        public async Task<int> AddRole(Role role)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var insertQuery = @"
                        INSERT INTO roles (name)
                        VALUES (@Name);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

                    int roleId = await connection.QueryFirstOrDefaultAsync<int>(insertQuery, role);
                    return roleId;
                }
                catch (Exception)
                {                   
                    return 0;
                }
            }
        }

        public async Task<bool> DeleteRole(Role role)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = @"UPDATE roles SET state=0 where (name = @Name)";
                    var result = await connection.ExecuteAsync(mysql, role);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<IEnumerable<Role>> GetActiveRole()
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {         
                    var query = @"
                    SELECT r.id, r.name, r.state,
                    rp.id, rp.id_role, rp.id_permissions, rp.add, rp.remove, rp.edit,
                    p.id, p.description, p.state
                    FROM roles r
                    JOIN rolesxpermissions rp ON r.id = rp.id_role
                    JOIN permissions p ON rp.id_permissions = p.id
                    WHERE ((r.state = 1) AND (p.state = 1))";
                    var roleDictionary = new Dictionary<int, Role>();
                    await connection.QueryAsync<Role, RoleXPermissions, Permissions, Role>(
                        query,
                        (role, rolePermissions, permissions) =>
                        {
                            if (!roleDictionary.TryGetValue(role.Id, out var roleEntry))
                            {
                                roleEntry = role;
                                roleEntry.RolePermissions = new List<RoleXPermissions>();
                                roleDictionary.Add(roleEntry.Id, roleEntry);
                            }

                            rolePermissions.Permission = permissions;
                            roleEntry.RolePermissions.Add(rolePermissions);
                            return roleEntry;
                        },
                        splitOn: "Id, Id, Id"
                    );                    
                    return roleDictionary.Values;
                }
                catch (Exception)
                {                    
                    List<Role> roles = new List<Role>();
                    return roles;
                }

            }
        }

        //public async Task<bool> ReRegister(Role role)
        //{
        //    using (var connection = _context.CreateConnection())
        //    {
        //        try
        //        {                    
        //            var mysql = @"UPDATE roles SET state=1 where (id = @Id)";
        //            var result = connection.Execute(mysql, role);
        //            if (result > 0)
        //            {                        
        //                return true;
        //            }                    
        //            return false;

        //        }
        //        catch (Exception)
        //        {
        //            connection.Close();
        //            return false;
        //        }
        //    }
        //}

        public async Task<Role> GetById(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var query = @"
                    SELECT r.id, r.name, r.state,
                    rp.id, rp.id_role, rp.id_permissions, rp.add, rp.remove, rp.edit,
                    p.id, p.description, p.state
                    FROM roles r
                    JOIN rolesxpermissions rp ON r.id = rp.id_role
                    JOIN permissions p ON rp.id_permissions = p.id
                    WHERE (r.id = @Id)";
                    var roleDictionary = new Dictionary<int, Role>();
                    await connection.QueryAsync<Role, RoleXPermissions, Permissions, Role>(
                        query,
                        (role, rolePermissions, permissions) =>
                        {
                            if (!roleDictionary.TryGetValue(role.Id, out var roleEntry))
                            {
                                roleEntry = role;
                                roleEntry.RolePermissions = new List<RoleXPermissions>();
                                roleDictionary.Add(roleEntry.Id, roleEntry);
                            }

                            rolePermissions.Permission = permissions;
                            roleEntry.RolePermissions.Add(rolePermissions);
                            return roleEntry;
                        },
                        new { Id = id },
                        splitOn: "Id, Id, Id"
                    );
                    return roleDictionary.Values.FirstOrDefault();
                }
                catch (Exception)
                {
                    Role role = new Role();
                    return role;
                }
            }
        }

        public async Task<bool> UpdateRole(Role role)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = "UPDATE roles SET name=@Name,state=@State WHERE id = @Id";
                    var result = await connection.ExecuteAsync(mysql, role);
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
