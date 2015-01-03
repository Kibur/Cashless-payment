using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.klant.ViewModel
{
    class IndexRegisterVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Index Register"; }
        }

        public IndexRegisterVM()
        {

        }

        public ICommand RegisterCommand
        {
            get { return new RelayCommand(Register); }
        }

        public void Register()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new RegisterFormVM());
        }

        public ICommand TerugCommand
        {
            get { return new RelayCommand(Terug); }
        }

        public void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            appvm.ChangePage(new IndexVM());
        }
    }
}
