using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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

namespace nmct.ba.cashlessproject.ui.verenigingmanagment.ViewModel
{
    class StatistiekbeheerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Statistiekbeheer"; }
        }

        public string Username
        {
            get { return "Ingelogd als " + ApplicationVM.username; }
        }

        private ObservableCollection<Sale> _bestellingen;

        public ObservableCollection<Sale> Bestellingen
        {
            get { return _bestellingen; }
            set { _bestellingen = value; OnPropertyChanged("Bestellingen"); }
        }

        private ObservableCollection<Sale> _bestellingenFilter;

        public ObservableCollection<Sale> BestellingenFilter
        {
            get { return _bestellingenFilter; }
            set { _bestellingenFilter = value; OnPropertyChanged("BestellingenFilter"); }
        }
        

        public StatistiekbeheerVM(ObservableCollection<Sale> bestellingen)
        {
            if (bestellingen.Count > 0)
            {
                Bestellingen = bestellingen;
                BestellingenFilter = Bestellingen;
            }

            GetProducts();
            GetRegisters();

            Van = DateTime.Now;
            Tot = DateTime.Now;
        }

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:23339/api/product");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);

                    Products.Add(new Product() { ID = -1, ProductName = "--Product--" });
                }
            }
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

                    Registers.Add(new Register() { ID = -1, RegisterName = "--Kassa--" });
                }
            }
        }

        private bool _status;

        public bool Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        private ObservableCollection<Register> _registers;

        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private Register _selectedRegister;

        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); }
        }

        private DateTime _van;

        public DateTime Van
        {
            get { return _van; }
            set { _van = value; OnPropertyChanged("Van"); }
        }

        private DateTime _tot;

        public DateTime Tot
        {
            get { return _tot; }
            set { _tot = value; OnPropertyChanged("Tot"); }
        }

        private string _exportMessage;

        public string ExportMessage
        {
            get { return _exportMessage; }
            set { _exportMessage = value; OnPropertyChanged("ExportMessage"); }
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

        public ICommand EnableDisableControlsCommand
        {
            get { return new RelayCommand<RadioButton>(EnableDisableControls); }
        }

        private void EnableDisableControls(RadioButton rb)
        {
            if (ExportMessage != null && !ExportMessage.Equals(string.Empty)) ExportMessage = string.Empty;

            if (rb.Name.Equals("rbTotaleVerkoop"))
            {
                Status = false;
                BestellingenFilter = Bestellingen;
            }
            else
            {
                Status = true;
            }
        }

        public ICommand ApplyFilterCommand
        {
            get { return new RelayCommand(ApplyFilter); }
        }

        private DateTime UnixToDateTime(int unixTimestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimestamp).ToLocalTime();

            return dtDateTime;
        }

        private ObservableCollection<Sale> GetSaleProducts()
        {
            ObservableCollection<Sale> list = new ObservableCollection<Sale>();

            foreach (Sale s in Bestellingen)
            {
                if (s.Product.ID == SelectedProduct.ID)
                {
                    list.Add(s);
                }
            }

            return list;
        }

        private ObservableCollection<Sale> GetSaleProductsById(ObservableCollection<Sale> list)
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.ID != -1)
                {
                    if (list.Count > 0)
                    {
                        foreach (Sale s in list)
                        {
                            if (s.Product.ID != SelectedProduct.ID)
                            {
                                list.Remove(s);
                            }
                        }
                    }
                    else
                    {
                        list = GetSaleProducts();
                    }
                }
                else if (SelectedProduct.ID == -1)
                {
                    list = GetSaleProducts();
                }
            }

            return list;
        }

        private ObservableCollection<Sale> GetSaleRegisters()
        {
            ObservableCollection<Sale> list = new ObservableCollection<Sale>();

            foreach (Sale s in Bestellingen)
            {
                if (s.Register.ID == SelectedRegister.ID)
                {
                    list.Add(s);
                }
            }

            return list;
        }

        private ObservableCollection<Sale> GetSaleRegistersById(ObservableCollection<Sale> list)
        {
            if (SelectedRegister != null)
            {
                if (SelectedRegister.ID != -1)
                {
                    if (list.Count > 0)
                    {
                        foreach (Sale s in list)
                        {
                            if (s.Register.ID != SelectedRegister.ID)
                            {
                                list.Remove(s);
                            }
                        }
                    }
                    else
                    {
                        list = GetSaleRegisters();
                    }
                }
                else if (SelectedRegister.ID == -1)
                {
                    list = GetSaleRegisters();
                }
            }

            return list;
        }

        private void ApplyFilter()
        {
            if (ExportMessage != null && !ExportMessage.Equals(string.Empty)) ExportMessage = string.Empty;

            ObservableCollection<Sale> list = new ObservableCollection<Sale>();

            try
            {
                if (Van != DateTime.Now && Tot != DateTime.Now)
                {
                    foreach (Sale s in Bestellingen)
                    {
                        DateTime datum = UnixToDateTime(s.Timestamp);

                        if (datum >= Van && datum <= Tot)
                        {
                            list.Add(s);
                        }
                    }
                }

                list = GetSaleProductsById(list);
                list = GetSaleRegistersById(list);
            }
            catch (InvalidOperationException)
            {
                // Dit gebeurt wanneer men de SelectedItem veranderen van een combobox.
                // Kan de collection van Bestellingen niet overlopen omdat er iets gewijzigd is tijdens de foreach.
            }

            BestellingenFilter = list;
        }

        public ICommand ExporteerCommand
        {
            get { return new RelayCommand(Exporteer); }
        }

        private void Exporteer()
        {
            string today = DateTime.Now.ToString("dd-MM-yyyy.hhum");
            string strDateTimeFormat = "dd/mm/yyyy";
            string vanFormat = Van.ToString(strDateTimeFormat);
            string totFormat = Tot.ToString(strDateTimeFormat);
            string strProductName = string.Empty;
            string strRegisterName = string.Empty;

            if (SelectedProduct != null && SelectedProduct.ID != -1)
            {
                strProductName = SelectedProduct.ProductName;
            }

            if (SelectedRegister != null && SelectedRegister.ID != -1)
            {
                strRegisterName = SelectedRegister.RegisterName;
            }
            
            //Document aanmaken
            SpreadsheetDocument doc = SpreadsheetDocument.Create("Statistiek_" + today + ".xlsx", SpreadsheetDocumentType.Workbook);
            WorkbookPart wbp = doc.AddWorkbookPart();
            wbp.Workbook = new Workbook();
            WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
            SheetData data = new SheetData();
            wsp.Worksheet = new Worksheet(data);
            Sheets sheets = doc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = doc.WorkbookPart.GetIdOfPart(wsp),
                SheetId = 1,
                Name = "Statistieken"
            };

            sheets.Append(sheet);

            //Gezochte data toevoegen
            Row zoekdata = new Row() { RowIndex = 1 };

            Cell vanLabel = new Cell() { CellReference = "A1", DataType = CellValues.String, CellValue = new CellValue("Van:") };
            Cell vanData = new Cell() { CellReference = "B1", DataType = CellValues.String, CellValue = new CellValue(vanFormat) };
            Cell totLabel = new Cell() { CellReference = "C1", DataType = CellValues.String, CellValue = new CellValue("Tot:") };
            Cell totData = new Cell() { CellReference = "D1", DataType = CellValues.String, CellValue = new CellValue(totFormat) };
            Cell productLabel = new Cell() { CellReference = "E1", DataType = CellValues.String, CellValue = new CellValue("Product:") };
            Cell productData = new Cell() { CellReference = "F1", DataType = CellValues.String, CellValue = new CellValue(strProductName) };
            Cell kassaLabel = new Cell() { CellReference = "G1", DataType = CellValues.String, CellValue = new CellValue("Kassa:") };
            Cell kassaData = new Cell() { CellReference = "H1", DataType = CellValues.String, CellValue = new CellValue(strRegisterName) };

            zoekdata.Append(vanLabel, vanData, totLabel, totData, productLabel, productData, kassaLabel, kassaData);
            data.Append(zoekdata);

            //Kolommen toevoegen
            Row header = new Row() { RowIndex = 3 };

            Cell tijdstipHeader = new Cell() { CellReference = "A3", DataType = CellValues.String, CellValue = new CellValue("Tijdstip") };
            Cell kassaHeader = new Cell() { CellReference = "B3", DataType = CellValues.String, CellValue = new CellValue("Kassa") };
            Cell productHeader = new Cell() { CellReference = "C3", DataType = CellValues.String, CellValue = new CellValue("Product") };
            Cell aantalHeader = new Cell() { CellReference = "D3", DataType = CellValues.String, CellValue = new CellValue("Aantal") };
            Cell totalePrijsHeader = new Cell() { CellReference = "E3", DataType = CellValues.String, CellValue = new CellValue("Totale prijs") };

            header.Append(tijdstipHeader, kassaHeader, productHeader, aantalHeader, totalePrijsHeader);
            data.Append(header);

            //Data invullen
            int i = 0;

            foreach (Sale s in BestellingenFilter)
            {
                Row sale = new Row() { RowIndex = Convert.ToUInt32(i) };

                Cell tijdstip = new Cell() { CellReference = "A" + i, DataType = CellValues.String, CellValue = new CellValue(UnixToDateTime(s.Timestamp).ToString("dd/MM/yyyy HH:mm")) };
                Cell registerName = new Cell() { CellReference = "B" + i, DataType = CellValues.String, CellValue = new CellValue(s.Register.RegisterName) };
                Cell productName = new Cell() { CellReference = "C" + i, DataType = CellValues.String, CellValue = new CellValue(s.Product.ProductName) };
                Cell aantal = new Cell() { CellReference = "D" + i, DataType = CellValues.String, CellValue = new CellValue(s.Amount.ToString()) };
                Cell totalePrijs = new Cell() { CellReference = "E" + i, DataType = CellValues.String, CellValue = new CellValue(s.TotalPrice.ToString()) };

                sale.Append(tijdstip, registerName, productName, aantal, totalePrijs);
                data.Append(sale);

                i += 4;
            }

            //Bestand opslaan
            wbp.Workbook.Save();
            doc.Close();

            ExportMessage = "Statistieken geëxporteert!";
        }
    }
}
