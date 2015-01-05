using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Errorlog : IDataErrorInfo
    {
        private int _registerid;

        public int RegisterID
        {
            get { return _registerid; }
            set { _registerid = value; }
        }

        private string _timestamp;

        [Required(ErrorMessage = "Timestamp is verplicht")]
        public string Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private string _message;

        [Required(ErrorMessage = "Message is verplicht")]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _stacktrace;

        [Required(ErrorMessage = "Stacktrace is verplicht")]
        public string Stacktrace
        {
            get { return _stacktrace; }
            set { _stacktrace = value; }
        }

        public Errorlog()
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
