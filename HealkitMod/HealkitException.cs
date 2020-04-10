using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealkitMod
{
    public class HealkitException : Exception
    {
        public string m_uniqueData;
        public string m_supperessMsg;

        public HealkitException(string info, Exception inner) : base(info, inner)
        { }
    }
}
