using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.IT_bedrijf
{
    public class Errorlog
    {
        private int _registerid;

        public int RegisterID
        {
            get { return _registerid; }
            set { _registerid = value; }
        }

        private DateTime _timestamp;

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _stacktrace;

        public string Stacktrace
        {
            get { return _stacktrace; }
            set { _stacktrace = value; }
        }

        public Errorlog()
        {

        }
    }
}
