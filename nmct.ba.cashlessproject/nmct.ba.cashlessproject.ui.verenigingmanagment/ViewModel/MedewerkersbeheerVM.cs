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
    class MedewerkersbeheerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Medewerkersbeheer"; }
        }

        public string Username
        {
            get { return "Ingelogd als " + ApplicationVM.username; }
        }

        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }

        private Employee _selected;

        public Employee SelectedEmployee
        {
            get { if (_selected == null) _selected = new Employee(); return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedEmployee"); }
        }

        public MedewerkersbeheerVM()
        {
            if (ApplicationVM.token != null)
            {
                GetEmployees();
            }
        }

        private async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/employee/");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        private async void SaveEmployee()
        {
            string input = JsonConvert.SerializeObject(SelectedEmployee);

            if (SelectedEmployee.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:23339/api/employee",
                        new StringContent(input, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedEmployee.ID = Int32.Parse(output);
                    }
                    else
                    {
                        Console.WriteLine("Save Employee Error");
                    }
                }
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PutAsync("http://localhost:23339/api/employee",
                        new StringContent(input, Encoding.UTF8, "application/json"));

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Save Employee Error");
                    }
                }
            }
        }

        private void NewEmployee()
        {
            Employee e = new Employee();
            Employees.Add(e);
            SelectedEmployee = e;
        }

        private async void DeleteEmployee()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:23339/api/employee/" + SelectedEmployee.ID);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Delete Employee Error");
                }
                else
                {
                    Employees.Remove(SelectedEmployee);
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

        public ICommand NewEmployeeCommand
        {
            get { return new RelayCommand(NewEmployee); }
        }

        public ICommand DeleteEmployeeCommand
        {
            get { return new RelayCommand(DeleteEmployee); }
        }

        public ICommand SaveEmployeeCommand
        {
            get { return new RelayCommand(SaveEmployee, SelectedEmployee.IsValid); }
        }

        public ICommand LogOutCommand
        {
            get { return new RelayCommand(LogOut); }
        }

        public void LogOut()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            ApplicationVM.token = null;

            appvm.ChangePage(new LoginVM());
        }
    }
}
