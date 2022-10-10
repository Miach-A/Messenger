using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.UserModels
{
    public class NewUserDTO
    {
        [MinLength(3)]
        [MaxLength(36)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only alphabet")]
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
