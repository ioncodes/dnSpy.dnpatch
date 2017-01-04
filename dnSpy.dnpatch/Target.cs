using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnSpy.dnpatch
{
    class Target
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string Instructions { get; set; }
        public string Indices { get; set; }
    }
}
