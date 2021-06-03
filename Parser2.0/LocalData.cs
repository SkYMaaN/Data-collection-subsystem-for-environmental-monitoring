using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser2._0
{
    class LocalData
    {
        internal string source;
        internal Object value;
        internal LocalData(string source_, Object value_)
        {
            this.source = source_;
            this.value = value_;
        }
        internal LocalData()
        {
            source = null;
            value = null;
        }
    }
}
