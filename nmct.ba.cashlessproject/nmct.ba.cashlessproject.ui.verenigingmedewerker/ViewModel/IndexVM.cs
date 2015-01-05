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

        private Sale _selectedBestelling;

        public Sale SelectedBestelling
        {
            get { return _selectedBestelling; }
            set { _selectedBestelling = value; OnPropertyChanged("SelectedBestelling"); }
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

        private async void SaveSales()
        {
            try
            {
                foreach (Sale s in Bestelling)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string input = JsonConvert.SerializeObject(s);
                        HttpResponseMessage response = await client.PostAsync("http://localhost:23339/api/sale",
                            new StringContent(input, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            string output = await response.Content.ReadAsStringAsync();

                            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

                            appvm.ChangePage(new IndexVM(SelectedEmployee, Register));
                        }
                        else
                        {
                            CreateErrorlog("Save Sale failed", "IndexVM", "SaveSales");
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // Kan Bestelling collection niet afgaan wanneer de collection een verandering heeft.
                // Hier versta ik niet echt goed waarom.
            }
        }

        private async void InsertErrorlog(Errorlog errlog)
        {
            using (HttpClient client = new HttpClient())
            {
                string input = JsonConvert.SerializeObject(errlog);
                HttpResponseMessage response = await client.PostAsync("http://localhost:23339/api/errorlog",
                    new StringContent(input, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Errorlog added");
                }
                else
                {
                    CreateErrorlog("Errorlog insert failed", "IndexVM", "InsertErrorlog");
                }
            }
        }

        private async void UpdateCustomer()
        {
            Customer.Balance -= totaal;

            string input = JsonConvert.SerializeObject(Customer);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PutAsync("http://localhost:23339/api/customer",
                    new StringContent(input, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    CreateErrorlog("Update customer failed", "IndexVM", "UpdateCustomer");
                }
            }
        }

        private void CreateErrorlog(string message, string classStacktrace, string methodStacktrace)
        {
            Errorlog errlog = new Errorlog();
            errlog.RegisterID = Register.ID;
            errlog.Timestamp = DateTimeToUnix(DateTime.Now).ToString();
            errlog.Message = message;
            errlog.Stacktrace = "Class: " + classStacktrace + ", Method: " + methodStacktrace;

            InsertErrorlog(errlog);
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

        private int DateTimeToUnix(DateTime dt)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, dt.Kind);
            int unixTimestamp = Convert.ToInt32((dt - date).TotalSeconds);

            return unixTimestamp;
        }

        private double totaal = 0;

        public void AddProduct()
        {
            if (SelectedProduct != null && Customer != null)
            {
                Sale newS = new Sale();

                newS.Product = SelectedProduct;
                newS.Register = Register;
                newS.Customer = Customer;
                newS.Amount = 1;
                newS.Timestamp = DateTimeToUnix(DateTime.Now);
                newS.TotalPrice = newS.Product.Price * newS.Amount;

                if (Bestelling.Count > 0)
                {
                    bool test = false;

                    try
                    {
                        foreach (Sale s in Bestelling)
                        {
                            if (s.Product.ID == newS.Product.ID)
                            {
                                newS.Amount = s.Amount + 1;
                                newS.TotalPrice = newS.Product.Price * newS.Amount;

                                if (!((newS.TotalPrice + totaal) > Customer.Balance))
                                {
                                    Bestelling.Remove(s);
                                    Bestelling.Add(newS);
                                }

                                test = true;
                            }
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // Kan Bestelling collection niet afgaan wanneer de collection een verandering heeft.
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

                totaal += newS.TotalPrice;
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
                if (SelectedBestelling != null)
                {
                    try
                    {
                        Sale newS = SelectedBestelling;

                        foreach (Sale s in Bestelling)
                        {
                            if (s.Product.ID == SelectedBestelling.Product.ID)
                            {
                                newS.Amount -= 1;
                                newS.TotalPrice = newS.Product.Price * newS.Amount;

                                Bestelling.Remove(s);

                                if (s.Amount > 0)
                                {
                                    Bestelling.Add(newS);
                                }
                            }
                        }

                        totaal -= newS.TotalPrice;
                    }
                    catch (InvalidOperationException)
                    {
                        // Kan Bestelling collection niet afgaan wanneer de collection een verandering heeft.
                    }
                }
            }
        }

        public ICommand LogOffCommand
        {
            get { return new RelayCommand(LogOff); }
        }

        private void LogOff()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new LoginVM());
        }

        public ICommand SaveOrderCommand
        {
            get { return new RelayCommand(SaveOrder); }
        }

        private void SaveOrder()
        {
            SaveSales();
            UpdateCustomer();
        }
    }
}
