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

namespace nmct.ba.cashlessproject.ui.verenigingmedewerker.ViewModel
{
    class IndexVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Index"; }
        }

        public IndexVM(Employee e, Register r)
        {
            SelectedEmployee = e;
            Register = r;
            GetProducts();
        }

        public string InlogInfo
        {
            get
            {
                string info = "Kassa ..., medewerker ...";

                if (Register != null && SelectedEmployee != null)
                {
                    info = "Kassa " + Register.RegisterName + ", medewerker " + SelectedEmployee.EmployeeName;
                }

                return info;
            }
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

        private ObservableCollection<Sale> _bestelling;

        public ObservableCollection<Sale> Bestelling
        {
            get
            {
                if (_bestelling == null)
                {
                    _bestelling = new ObservableCollection<Sale>();
                }

                return _bestelling;
            }
            set { _bestelling = value; OnPropertyChanged("Bestelling"); }
        }
        
        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; OnPropertyChanged("Register"); }
        }

        private int _counter;

        public int Counter
        {
            get { return _counter; }
            set { _counter = value; OnPropertyChanged("Counter"); }
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
            if (SelectedProduct != null && Customer != null)
            {
                Sale newS = new Sale();

                newS.Product = SelectedProduct;
                newS.Register = Register;
                newS.Customer = Customer;
                newS.Amount = 1;
                newS.Timestamp = DateTime.Now;
                newS.TotalPrice = newS.Product.Price * newS.Amount;

                Counter = newS.Amount;

                if (Bestelling.Count > 0)
                {
                    bool test = false;

                    foreach (Sale s in Bestelling)
                    {
                        if (s.Product.ID == newS.Product.ID)
                        {
                            s.Amount = s.Amount + 1;
                            Counter = s.Amount;
                            test = true;
                        }
                    }

                    if (!test)
                    {
                        Bestelling.Add(newS);
                    }
                }
                else
                {
                    Bestelling.Add(newS);
                }
            }
        }

        public ICommand VerlaagAantalCommand
        {
            get { return new RelayCommand(VerlaagAantal); }
        }

        public void VerlaagAantal()
        {
            if (Bestelling.Count > 0)
            {
                foreach (Sale s in Bestelling)
                {
                    if (s.Product.ID == SelectedProduct.ID)
                    {
                        s.Amount = s.Amount - 1;

                        Counter = s.Amount;

                        if (s.Amount == 0)
                        {
                            Bestelling.Remove(s);
                        }
                    }
                }
            }
        }
    }
}
