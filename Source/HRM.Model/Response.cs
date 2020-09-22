using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Model
{
    public class Response
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public object Data { get; set; }
    }
}
