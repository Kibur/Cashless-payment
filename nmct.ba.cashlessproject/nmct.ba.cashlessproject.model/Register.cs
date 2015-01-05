using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Register : IDataErrorInfo
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _registername;

        [Required(ErrorMessage = "Kassanaam is verplicht")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Kassanaam moet tussen de 2 en 30 karakters lang zijn")]
        public string RegisterName
        {
            get { return _registername; }
            set { _registername = value; }
        }

        private string _device;

        [Required(ErrorMessage = "Toestel is verplicht")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Toestel moet tussen de 3 en 30 karakters lang zijn")]
        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        public Register()
        {

        }

        public override string ToString()
        {
            return RegisterName + " (" + Device + ")";
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
