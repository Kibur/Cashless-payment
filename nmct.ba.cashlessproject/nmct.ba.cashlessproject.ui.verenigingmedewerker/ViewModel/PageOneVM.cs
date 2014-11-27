using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
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
    class PageOneVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "First page"; }
        }

        private string _demo = "Demo";

        public string Demo
        {
            get { return _demo; }
            set
            {
                _demo = value;
                OnPropertyChanged("Demo");
            }
        }
    }
}
