using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.UserModels
{
    public class CreateUserDTO //: UpdateUserDTO
    {
        [MinLength(3)]
        [MaxLength(36)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "only alphabet and numbers")]
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
