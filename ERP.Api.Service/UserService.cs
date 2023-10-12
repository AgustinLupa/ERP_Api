using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ERP.Api.Entity;
using ERP.Api.Entity.Contracts;

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
                    var mysql = @"SELECT * FROM users order by name ASC";
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
                    var mysql = @"SELECT * FROM users Where(state = 1)";
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
                    var mysql = @"SELECT name FROM users WHERE((state = 1) and ((name = @Name) and (password = @Password)))";
                    var result = await connection.QueryFirstOrDefaultAsync<User>(mysql, user);
                    return result;
                }
                catch (Exception)
                {
                    return new User();
                }
            }
        }
    }
}
