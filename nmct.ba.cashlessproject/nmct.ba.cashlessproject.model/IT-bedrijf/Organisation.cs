using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.IT_bedrijf
{
    public class Organisation
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _login;

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _dbname;

        public string DbName
        {
            get { return _dbname; }
            set { _dbname = value; }
        }

        private string _dblogin;

        public string DbLogin
        {
            get { return _dblogin; }
            set { _dblogin = value; }
        }

        private string _dbpassword;

        public string DbPassword
        {
            get { return _dbpassword; }
            set { _dbpassword = value; }
        }

        private string _organisationname;

        public string OrganisationName
        {
            get { return _organisationname; }
            set { _organisationname = value; }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public Organisation()
        {

        }
    }
}
