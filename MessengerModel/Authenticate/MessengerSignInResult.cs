using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel.Authenticate
{
    public class MessengerSignInResult
    {
        public bool Result { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
