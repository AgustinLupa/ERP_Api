using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Api.Entity
{
    public class RoleXPermissions
    {
        public int Id { get; set; }
        public int Id_Role { get; set; }
        public int Id_Permission { get; set; }
        public int Add { get; set; }
        public int Remove { get; set; }
        public int Edit { get; set; }
        public Permissions Permission { get; set; }
    }
}
