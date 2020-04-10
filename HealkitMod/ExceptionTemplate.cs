using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealkitMod
{
    public class ExceptionTemplate
    {
        private static Dictionary<int,ExceptionTemplate> templates = new Dictionary<int,ExceptionTemplate>();

        public Exception Exception { get; private set; }
        public int RaisedCount { get; set; } = 0;
        public bool Suppressed { get; set; }

        private ExceptionTemplate(Exception e)
        {
            Exception = e;
        }

        public static void ResetSuppressing()
        {
            foreach(var templatePair in templates)
            {
                templatePair.Value.Suppressed = false;
            }
        }

        public static ExceptionTemplate RegisterException(Exception e)
        {
            ExceptionTemplate val;
            int hash = GetExceptionHash(e);
            if(!templates.TryGetValue(hash,out val))
            {
                val = new ExceptionTemplate(e);
                templates.Add(hash,val);
            }
            return val;
        }

        private static int GetExceptionHash(Exception e)
        {
            HealkitException e2 = e as HealkitException;
            return e2 != null ? ((e2.m_uniqueData ?? "") + e2.InnerException.ToString()).GetHashCode() : e.ToString().GetHashCode();
        }
    }
}
