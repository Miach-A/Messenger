using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerData.Repository
{
    public class SaveResult
    {
        public bool Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
