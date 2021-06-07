using System;

namespace Parser2._0
{
    class LocalData
    {
        internal string source;
        internal Object value;
        internal int id;
        internal LocalData(string source_, Object value_, int id_)
        {
            this.source = source_;
            this.value = value_;
            this.id = id_;
        }
        internal LocalData()
        {
            source = "";
            value = 0;
            id = 0;
        }
    }
}
