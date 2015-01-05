using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.IT_bedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.verenigingmanagment.ViewModel
{
    class AccountbeheerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Accountbeheer"; }
        }

        public string Username
        {
            get { return "Ingelogd als " + ApplicationVM.username; }
        }

        private string _oud;

        public string OudWachtwoord
        {
            get { return _oud; }
            set { _oud = value; }
        }

        private string _nieuw;

        public string NieuwWachtwoord
        {
            get { return _nieuw; }
            set { _nieuw = value; }
        }

        private string _bevestig;

        public string BevestigWachtwoord
        {
            get { return _bevestig; }
            set { _bevestig = value; }
        }
        
        public AccountbeheerVM()
        {

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

        public ICommand SaveAccountCommand
        {
            get
            {
                return new RelayCommand(SaveAccount);
            }
        }

        private async void ChangePassword()
        {
            Organisation o = new Organisation();
            o.Login = Cryptography.Encrypt(Username);
            o.Password = Cryptography.Encrypt(NieuwWachtwoord);
            o.DbLogin = o.Login;
            o.DbPassword = o.Password;

            string input = JsonConvert.SerializeObject(o);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PutAsync("http://localhost:23339/api/organisation",
                    new StringContent(input, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Save Organisation Error");
                }
            }
        }

        private async void GetCheckAccount()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/organisation?username=" + Cryptography.Encrypt(Username) + "&password=" + Cryptography.Encrypt(OudWachtwoord));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    bool IsAccount = JsonConvert.DeserializeObject<bool>(json);

                    if (IsAccount)
                    {
                        if (NieuwWachtwoord.Equals(BevestigWachtwoord))
                        {
                            ChangePassword();
                        }
                    }
                }
            }
        }

        public void SaveAccount()
        {
            GetCheckAccount();
        }
    }
}
