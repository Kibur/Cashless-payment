using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Sale
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _timestamp;

        public int Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; }
        }

        private Product _product;

        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        private int _amount;

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private double _totalprice;

        public double TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }

        public Sale()
        {

        }
    }
}
