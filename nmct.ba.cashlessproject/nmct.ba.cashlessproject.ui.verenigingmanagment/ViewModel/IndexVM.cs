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
    class IndexVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Index page"; }
        }

        public string Username
        {
            get { return "Ingelogd als " + ApplicationVM.username; }
        }

        public IndexVM()
        {
            if (ApplicationVM.token != null)
            {
                GetRegisters();
                GetSales();
            }
        }

        private ObservableCollection<Register> _registers;

        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private ObservableCollection<Sale> _bestellingen;

        public ObservableCollection<Sale> Bestellingen
        {
            get { return _bestellingen; }
            set { _bestellingen = value; OnPropertyChanged("Bestellingen"); }
        }

        private ObservableCollection<Sale> _bestellingenLaatsteMaand;

        public ObservableCollection<Sale> BestellingenLaatsteMaand
        {
            get { return _bestellingenLaatsteMaand; }
            set { _bestellingenLaatsteMaand = value; OnPropertyChanged("BestellingenLaatsteMaand"); }
        }

        private DateTime UnixToDateTime(int unixTimestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimestamp).ToLocalTime();

            return dtDateTime;
        }

        private void FilterLaatsteMaand()
        {
            ObservableCollection<Sale> list = new ObservableCollection<Sale>();

            foreach (Sale s in Bestellingen)
            {
                int laatsteMaand = DateTime.Now.Month - 1;

                if (laatsteMaand == 0) laatsteMaand = 12;

                if (laatsteMaand == UnixToDateTime(s.Timestamp).Month)
                {
                    list.Add(s);
                }
            }

            BestellingenLaatsteMaand = list;
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

        private async void GetSales()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/sale");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Bestellingen = JsonConvert.DeserializeObject<ObservableCollection<Sale>>(json);

                    FilterLaatsteMaand();
                }
            }
        }

        public ICommand AccountbeheerCommand
        {
            get {
                return new RelayCommand(Accountbeheer);
            }
        }

        public void Accountbeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new AccountbeheerVM());
        }

        public ICommand ProductbeheerCommand
        {
            get
            {
                return new RelayCommand(Productbeheer);
            }
        }

        public void Productbeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new ProductbeheerVM());
        }

        public ICommand MedewerkersbeheerCommand
        {
            get
            {
                return new RelayCommand(Mederwerkersbeheer);
            }
        }

        public void Mederwerkersbeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new MedewerkersbeheerVM());
        }

        public ICommand KassabeheerCommand
        {
            get
            {
                return new RelayCommand(Kassabeheer);
            }
        }

        public void Kassabeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new KassabeheerVM());
        }

        public ICommand StatsbeheerCommand
        {
            get
            {
                return new RelayCommand(Statistiekbeheer);
            }
        }

        public void Statistiekbeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new StatistiekbeheerVM(Bestellingen));
        }

        public ICommand KlantenbeheerCommand
        {
            get
            {
                return new RelayCommand(Klantenbeheer);
            }
        }

        public void Klantenbeheer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new KlantenbeheerVM());
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
