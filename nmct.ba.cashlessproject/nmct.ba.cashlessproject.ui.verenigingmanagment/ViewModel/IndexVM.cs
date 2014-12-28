﻿using GalaSoft.MvvmLight.CommandWpf;
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
            get { return ApplicationVM.username; }
        }

        public IndexVM()
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

            appvm.ChangePage(new StatistiekbeheerVM());
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
