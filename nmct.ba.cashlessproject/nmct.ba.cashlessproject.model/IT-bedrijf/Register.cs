using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.IT_bedrijf
{
    public class Register
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _registername;

        public string RegisterName
        {
            get { return _registername; }
            set { _registername = value; }
        }

        private string _device;

        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        private DateTime _purchasedate;

        public DateTime PurchaseDate
        {
            get { return _purchasedate; }
            set { _purchasedate = value; }
        }

        private DateTime _expiresdate;

        public DateTime ExpiresDate
        {
            get { return _expiresdate; }
            set { _expiresdate = value; }
        }

        public Register()
        {

        }
    }
}
