using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.klant.ViewModel
{
    class IndexVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Index"; }
        }

        public IndexVM()
        {

        }

        public IndexVM(Customer customer)
        {
            Customer = customer;
            eID = Customer.ID;
        }

        private int _eID;

        public int eID
        {
            get { return _eID; }
            set { _eID = value; OnPropertyChanged("eID"); }
        }
        
        
        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private async void GetCustomerById()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/customer?value=" + eID);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer = JsonConvert.DeserializeObject<Customer>(json);

                    if (Customer.CustomerName == null)
                    {
                        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

                        appvm.ChangePage(new IndexRegisterVM());
                    }
                }
            }
        }

        public ICommand GetCustomerCommand
        {
            get { return new RelayCommand(GetCustomer); }
        }

        public void GetCustomer()
        {
            GetCustomerById();
        }

        public ICommand OpladenCommand
        {
            get { return new RelayCommand(Opladen); }
        }

        public void Opladen()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new OpladenVM(Customer));
        }
    }
}
