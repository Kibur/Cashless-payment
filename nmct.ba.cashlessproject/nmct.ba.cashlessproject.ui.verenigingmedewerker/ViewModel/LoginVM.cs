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

        private void Login()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            GetEmployee(ID);

            if (SelectedEmployee != null)
            {
                appvm.ChangePage(new IndexVM());
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
    }
}
