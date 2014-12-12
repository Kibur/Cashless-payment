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

        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }
        

        private Register _selected;

        public Register SelectedRegister
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedRegister"); }
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
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/employee/" + SelectedRegister.ID);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
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
    }
}
