using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.klant.ViewModel
{
    class RegisterFormVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Registratieformulier"; }
        }

        public RegisterFormVM()
        {

        }

        private string _picture;

        public string Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged("Picture"); }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; OnPropertyChanged("CustomerName"); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("Address"); }
        }

        private byte[] GetPhoto()
        {
            if (Picture == null)
            {
                return new byte[] { 1 };
            }

            FileStream fs = new FileStream(Picture, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];

            fs.Read(data, 0, (int)fs.Length);
            fs.Close();

            return data;
        }

        private async void SaveCustomer()
        {
            Customer c = new Customer();
            c.CustomerName = CustomerName;
            c.Address = Address;
            c.Picture = GetPhoto();
            c.Balance = 0;

            string input = JsonConvert.SerializeObject(c);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:23339/api/customer",
                    new StringContent(input, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();
                    c.ID = Convert.ToInt32(output);

                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

                    appvm.ChangePage(new IndexVM(c));
                }
                else
                {
                    Console.WriteLine("Save Customer Error");
                }
            }
        }

        public ICommand SaveCustomerCommand
        {
            get { return new RelayCommand(SaveCustomer); }
        }

        public ICommand TerugCommand
        {
            get { return new RelayCommand(Terug); }
        }

        public void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new IndexVM());
        }
    }
}
