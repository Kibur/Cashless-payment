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
using System.Windows.Controls;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.verenigingmedewerker.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Login"; }
        }

        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }
        

        private Employee _selected;

        public Employee SelectedEmployee
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; OnPropertyChanged("Register"); }
        }

        private async void GetEmployee(int id) {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/employee/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    SelectedEmployee = JsonConvert.DeserializeObject<Employee>(json);
                }
            }
        }

        private async void GetRegisterByEmployee()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/register?eID=" + SelectedEmployee.ID);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Register = JsonConvert.DeserializeObject<Register>(json);
                }
            }
        }

        private void Login()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            GetEmployee(ID);

            if (SelectedEmployee != null)
            {
                GetRegisterByEmployee();

                if (Register != null)
                {
                    appvm.ChangePage(new IndexVM(SelectedEmployee, Register));
                }
            }
            else
            {
                Error = "Authenticatie mislukt";
            }
        }

        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }

        public ICommand AddNumberCommand
        {
            get { return new RelayCommand<Button>(AddNumber); }
        }

        private void AddNumber(Button btn)
        {
            string id = ID.ToString() + btn.Content.ToString();
            int number = 0;

            if (int.TryParse(id, out number))
            {
                ID = number;
            }
        }

        public ICommand ClearNumbersCommand
        {
            get { return new RelayCommand(ClearNumbers); }
        }

        private void ClearNumbers()
        {
            ID = 0;
        }

        public ICommand RemoveNumberCommand
        {
            get { return new RelayCommand(RemoveNumber); }
        }

        private void RemoveNumber()
        {
            string id = ID.ToString();
            string newID = "0";

            if (id.Length > 1)
            {
                newID = id.Remove(id.Length - 1);
            }

            ID = Convert.ToInt32(newID);
        }
    }
}
