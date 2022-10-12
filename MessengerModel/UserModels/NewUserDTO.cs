using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.UserModels
{
    public class NewUserDTO : UpdateUserDTO
    {
        public string Password { get; set; } = string.Empty;
    }
}
