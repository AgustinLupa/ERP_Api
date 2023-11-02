using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Api.Entity.Contracts
{
    public interface IRoleService
    {
        public Task<IEnumerable<Role>> GetAll();
        public Task<int> AddRole(Role role);
        public Task<bool> DeleteRole(Role role);
        public Task<IEnumerable<Role>> GetActiveRole();
        public Task<Role> GetById(int id);
        public Task<bool> UpdateRole(Role role);
    }
}
