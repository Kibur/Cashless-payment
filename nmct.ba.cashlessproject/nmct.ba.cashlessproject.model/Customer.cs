using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.model
{
    public class Customer : IDataErrorInfo
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _customername;

        [Required(ErrorMessage="Naam is verplicht")]
        [StringLength(50, MinimumLength=3, ErrorMessage="De naam moet tussen de 3 en 50 karakters bevatten")]
        public string CustomerName
        {
            get { return _customername; }
            set { _customername = value; }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private byte[] _picture;

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private double _balance;

        public double Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        public Customer()
        {

        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }

        public string Error
        {
            get { return "Het object is niet valid"; }
        }

        public string this[string columnName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columnName
                    });
                }
                catch (ValidationException ex)
                {
                    return ex.Message;
                }

                return String.Empty;
            }
        }
    }
}
