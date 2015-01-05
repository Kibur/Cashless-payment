using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Product : IDataErrorInfo
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _productname;

        [Required(ErrorMessage = "Productnaam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Productnaam moet tussen de 3 en 50 karakters lang zijn")]
        public string ProductName
        {
            get { return _productname; }
            set { _productname = value; }
        }

        private double _price;

        [Required(ErrorMessage = "Prijs is verplicht")]
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public Product()
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
