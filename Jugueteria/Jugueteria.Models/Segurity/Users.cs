using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jugueteria.Models.Segurity
{
    public class Users : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public DateTime RegisterDate { get; set; }


        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }

    }
}
