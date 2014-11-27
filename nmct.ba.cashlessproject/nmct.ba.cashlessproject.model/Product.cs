using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Product
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _productname;

        public string ProductName
        {
            get { return _productname; }
            set { _productname = value; }
        }

        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public Product()
        {

        }
    }
}
