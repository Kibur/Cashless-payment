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

namespace nmct.ba.cashlessproject.ui.verenigingmanagment.ViewModel
{
    class KassabeheerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Kassabeheer page"; }
        }

        public string Username
        {
            get { return ApplicationVM.username; }
        }

        public KassabeheerVM()
        {
            if (ApplicationVM.token != null)
            {
                GetRegisters();
            }
        }

        private ObservableCollection<Register> _registers;

        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private ObservableCollection<RegisterEmployee> _registersEmployees;

        public ObservableCollection<RegisterEmployee> RegistersEmployees
        {
            get { return _registersEmployees; }
            set { _registersEmployees = value; OnPropertyChanged("RegistersEmployees"); }
        }
        
        private Register _selectedRegister;

        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private RegisterEmployee _selectedRegisterEmployee;

        public RegisterEmployee SelectedRegisterEmployee
        {
            get { return _selectedRegisterEmployee; }
            set { _selectedRegisterEmployee = value; OnPropertyChanged("SelectedRegisterEmployee"); }
        }

        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }
        

        private async void GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/register");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                }
            }
        }

        private async void GetEmployeesByRegister()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/employee?rID=r" + SelectedRegister.ID);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        RegistersEmployees = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json);

                        ObservableCollection<Employee> employeeList = new ObservableCollection<Employee>();

                        foreach (RegisterEmployee re in RegistersEmployees)
                        {
                            employeeList.Add(re.Employee);
                        }

                        Employees = employeeList;
                    }
                }
            }
            catch (NullReferenceException nrex)
            {
                // Gebeurt wanneer je één van de listboxes gebruikt en daarna op 'Afmelden' klikt.
                // Omdat ik de token op null zet. Maar deze exception gebeurt alleen bij Kassabeheer.
            }
        }

        public ICommand TerugCommand
        {
            get
            {
                return new RelayCommand(Terug);
            }
        }

        public void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new IndexVM());
        }

        public ICommand AccountbeheerCommand
        {
            get { return new RelayCommand(Accountbeheer); }
        }

        public void Accountbeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new AccountbeheerVM());
        }

        public ICommand GetEmployeesCommand
        {
            get { return new RelayCommand(GetEmployees); }
        }

        public void GetEmployees()
        {
            GetEmployeesByRegister();
        }

        public ICommand GetRegister_EmployeeCommand
        {
            get { return new RelayCommand(GetRegister_Employee); }
        }

        public void GetRegister_Employee()
        {
            if (SelectedEmployee != null)
            {
                foreach (RegisterEmployee re in RegistersEmployees)
                {
                    if (SelectedRegister.ID.Equals(re.Register.ID) && SelectedEmployee.ID.Equals(re.Employee.ID))
                    {
                        SelectedRegisterEmployee = re;
                    }
                }
            }
        }

        public ICommand LogOutCommand
        {
            get { return new RelayCommand(LogOut); }
        }

        public void LogOut()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new LoginVM());

            ApplicationVM.token = null;
        }
    }
}
