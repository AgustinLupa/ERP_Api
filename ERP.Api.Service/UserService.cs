using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ERP.Api.Entity;
using ERP.Api.Entity.Contracts;
using MySql.Data.MySqlClient;

namespace ERP.Api.Service
{
    public class UserService : IUserService
    {
        private readonly Context _context;

        public UserService(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using(var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = @"SELECT id, name, id_role FROM users order by name ASC";
                    var result = await connection.QueryAsync<User>(mysql);                    
                    return result.ToList();
                }
                catch (Exception)
                {                   
                    return new List<User>();
                }
            }
        }

        public async Task<IEnumerable<User>> GetActive()
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = @"SELECT id, name, id_role FROM users Where(state = 1)";
                    var result = await connection.QueryAsync<User>(mysql);
                    return result.ToList();
                }
                catch (Exception)
                {
                    return new List<User>();
                }
            }
        }

        public async Task<User> Login(User user)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = @"SELECT name, id, state, id_role  FROM users WHERE((state = 1) and ((name = @Name) and (password = @Password))) LIMIT 1";
                    var result = await connection.QueryFirstOrDefaultAsync<User>(mysql, user);
                    return result;
                }
                catch (Exception)
                {
                    return new User();
                }
            }
        }

        public async Task<int> Create(User user)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = @"INSERT INTO users(name, password, id_role) Values (@Name, @Password, @Id_Role);
                                  SELECT LAST_INSERT_ID() as id";
                    var result = await connection.ExecuteAsync(mysql, user);                    
                   return result;                   
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public async Task<int> Update(User user)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                { 
                    var mysql = @"UPDATE users SET name = @Name, password=@Password, id_role=@Id_Role, state=@State where (id = @Id) LIMIT 1";
                    var result = await connection.ExecuteAsync(mysql, user);                    
                    return result;                   
                }
                catch (Exception)
                {
                    connection.Close();
                    return 0;
                }
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var mysql = @"UPDATE users SET state=0 where (id = @Id) LIMIT 1";
                    var result = await connection.ExecuteAsync(mysql, new {Id = id});
                    return result;
                }
                catch (Exception)
                {
                    connection.Close();
                    return 0;
                }
            }
        }

    }
}
