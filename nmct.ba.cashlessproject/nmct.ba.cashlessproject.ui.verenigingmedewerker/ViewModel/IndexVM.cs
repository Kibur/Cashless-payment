using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.ui.verenigingmedewerker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.verenigingmedewerker.ViewModel
{
    class IndexVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Index"; }
        }

        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private Product _selected;

        public Product SelectedProduct
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedProduct"); }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; OnPropertyChanged("CustomerName"); }
        }

        private ObservableCollection<Bestelling> _bestelling;

        public ObservableCollection<Bestelling> Bestelling
        {
            get { return _bestelling; }
            set { _bestelling = value; OnPropertyChanged("Bestelling"); }
        }

        private int _counter;

        public int Counter
        {
            get { return _counter; }
            set { _counter = value; OnPropertyChanged("Counter"); }
        }
        

        public IndexVM()
        {
            GetProducts();
        }

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/product");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                }
            }
        }

        private async void GetCustomerByName()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/customer?value=" + CustomerName.ToLower());

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer = JsonConvert.DeserializeObject<Customer>(json);
                }
            }
        }

        public ICommand GetCustomerCommand
        {
            get { return new RelayCommand(GetCustomer); }
        }

        public void GetCustomer()
        {
            GetCustomerByName();
        }

        public ICommand AddProductCommand
        {
            get { return new RelayCommand(AddProduct); }
        }

        public void AddProduct()
        {
            Counter++;

            Bestelling b = new Bestelling();
            b.Product = SelectedProduct;
            b.Counter = Counter;

            Bestelling.Add(b);
        }

        public ICommand VerlaagAantalCommand
        {
            get { return new RelayCommand(VerlaagAantal); }
        }

        public void VerlaagAantal()
        {
            Bestelling.Remove(SelectedProduct);
        }
    }
}
