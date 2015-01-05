using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Employee : IDataErrorInfo
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _employeename;

        [Required(ErrorMessage="Naam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage="Er zijn geen speciale tekens toegelaten")]
        [StringLength(50, MinimumLength=3, ErrorMessage="De naam moet tussen de 3 en 50 karakters bevatten")]
        public string EmployeeName
        {
            get { return _employeename; }
            set { _employeename = value; }
        }

        private string _address;

        [Required(ErrorMessage="Het adres is verplicht")]
        [StringLength(50, MinimumLength=3, ErrorMessage="Het adres moet tussen de 3 en 50 karakters bevatten")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _email;

        [Required(ErrorMessage="Email adres is verplicht")]
        [EmailAddress(ErrorMessage="Geen geldig email adres")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;

        [Required(ErrorMessage="Telefoonnummer is verplicht")]
        [StringLength(12, MinimumLength = 12, ErrorMessage="Telefoonnummer moet 12 karakters zijn")]
        [RegularExpression(@"^(\d{3})(\/)(\d{2})(\.)(\d{2})(\.)(\d{2})$", ErrorMessage = "Geen geldig telefoonnummer")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public Employee()
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
