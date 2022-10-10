using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerData.Repository
{
    public class SaveResult<T> where T : class
    {
        public bool Result { get; set; }
        public T? Entity { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
