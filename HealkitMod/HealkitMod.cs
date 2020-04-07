using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealkitMod
{
    public class HealkitMod
    {
        private static HealkitMod _instance;
        public static HealkitMod instance { get
            {
                if(_instance == null)
                {
                    _instance = new HealkitMod();
                }
                return _instance;
            }
        }
    }
}
