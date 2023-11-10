using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Api.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public int State { get; set; }
        public int Id_Role { get; set; }
        public Role? Role { get; set; }= new Role();
    }
}
