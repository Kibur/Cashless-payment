using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterEmployee
    {
        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; }
        }

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }

        private int _from;

        public int From
        {
            get { return _from; }
            set { _from = value; }
        }

        private int _until;

        public int Until
        {
            get { return _until; }
            set { _until = value; }
        }

        public RegisterEmployee()
        {

        }
    }
}
