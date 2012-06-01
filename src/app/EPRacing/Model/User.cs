using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPRacing.Model
{
   public class User : BaseEntity
    {
       public User()
       {
           
       }

       public User(string username,string password,string email)
       {
           Username = username;
           Password = password;
           Email = email;
       }

       public string Email { get; set; }
       public string Username { get; set; }
       [DataType(DataType.Password)]
       public string Password { get; set; }
    }
}
