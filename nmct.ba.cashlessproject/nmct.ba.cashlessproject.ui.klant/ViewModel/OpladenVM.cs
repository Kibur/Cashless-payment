using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.klant.ViewModel
{
    class OpladenVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Opladen"; }
        }

        public OpladenVM(Customer customer)
        {
            if (customer != null)
            {
                Customer = customer;
            }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private ObservableCollection<int> _biljetten;

        public ObservableCollection<int> Biljetten
        {
            get
            {
                _biljetten = new ObservableCollection<int>();

                _biljetten.Add(5);
                _biljetten.Add(10);
                _biljetten.Add(20);

                return _biljetten;
            }
            set { _biljetten = value; }
        }

        private int _selectedBiljet;

        public int SelectedBiljet
        {
            get { return _selectedBiljet; }
            set { _selectedBiljet = value; OnPropertyChanged("SelectedBiljet"); }
        }

        private async void UpdateCustomer()
        {
            string input = JsonConvert.SerializeObject(Customer);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PutAsync("http://localhost:23339/api/customer",
                    new StringContent(input, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Save Customer Error");
                }
            }
        }

        public ICommand AddBalanceCommand
        {
            get { return new RelayCommand(AddBalance); }
        }

        public void AddBalance()
        {
            double preview = Customer.Balance + SelectedBiljet;

            if (preview <= 100)
            {
                Customer.Balance = preview;
                OnPropertyChanged("Customer");
                UpdateCustomer();
            }
        }

        public ICommand FinishCommand
        {
            get { return new RelayCommand(Finish); }
        }

        public void Finish()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new IndexVM());
        }
    }
}
