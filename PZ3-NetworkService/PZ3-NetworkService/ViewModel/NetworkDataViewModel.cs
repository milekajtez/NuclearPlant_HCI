using Microsoft.Win32;
using PZ3_NetworkService.Model;
using PZ3_NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {
        #region Inicijalizacija liste,komandi i polja za propertyje,kao i postavljanje pocetnih stanja
        public static ObservableCollection<WaterMachine> Lista_elektarana { get; set; }

        private static ObservableCollection<string> waters = new ObservableCollection<string>() { "Turbo795", "Turbo796", "HidroMer71", "HidroMer81", "SuperAqua10" };
        public static ObservableCollection<string> Waters { get => waters; set => waters = value; }

        public MyICommand AddCommand { get; set; }          //komanda za dodavanje u tabelu
        public MyICommand DeleteCommand { get; set; }       //komanda za brisanje iz tebele

        private string variableID = "";
        private string variableNAME = "";
        private double variableVREDNOST;
        private Model.Type variableTIP = new Model.Type();
        private Model.Type variableSLIKA = new Model.Type();

        private DataIO serializer = new DataIO();               //serijalizacija

        //varijable vezane za statistiku
        private double po1 = 0;             //u njih ce broj objekata po vrstama
        private double po2 = 0;
        private double po3 = 0;
        private double po4 = 0;
        private double po5 = 0;
        private string procenti1 = "";      //u njih ce da ide taj rezultat u obliku texta(string)
        private string procenti2 = "";
        private string procenti3 = "";
        private string procenti4 = "";
        private string procenti5 = "";

        //komande i varialbla za search
        public MyICommand SearchCommand { get; set; }
        public MyICommand EndSearchCommand { get; set; }
        private string variableSEARCH = "";                 //varijable za search
        private bool variableRADIONAME;
        private bool variableRADIOTYPE;
        public static ObservableCollection<WaterMachine> Search_lista { get; set; }     //pomocna lista---za search i za filter
        private bool search_time = false;               //ovo ce mi govoriti da samo jednom mogu da uradim search


        //komande i varijable za filter
        public MyICommand FilterCommand { get; set; }
        public MyICommand EndFilterCommand { get; set; }
        private bool variableRADIOOK;                       //ok vrednost
        private bool variableRADIONO;                       //nije ok vrednost
        private bool filter_time = false;                   //ovo ce mi govoriti da samo jednom mogu da uradim search




        public NetworkDataViewModel()
        {
            UcitajElektrane();
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            SearchCommand = new MyICommand(OnSearch);
            EndSearchCommand = new MyICommand(OnEndSearch);
            FilterCommand = new MyICommand(OnFilter);
            EndFilterCommand = new MyICommand(OnEndFilter);
        }


        public void UcitajElektrane()
        {

            Lista_elektarana = serializer.DeSerializeObject<ObservableCollection<WaterMachine>>("fajl_sa_podacima.xml");

            //lista u kojoj su mi podaci za prikazivanje
            if (Lista_elektarana == null) //U slucaju da nista nije ucitano
            {
                Lista_elektarana = new ObservableCollection<WaterMachine>(); //nova lista pa cemo u nju dodavati
            }

            //lista za pretragu
            if (Search_lista == null)
            {
                Search_lista = new ObservableCollection<WaterMachine>();
            }

            po1 = 0;             //u njih ce broj objekata po vrstama
            po2 = 0;
            po3 = 0;
            po4 = 0;
            po5 = 0;
            //pocetna statistika
            for (int i = 0; i < Lista_elektarana.Count; i++)
            {
                if (Lista_elektarana[i].Tip_name == "Turbo795")
                {
                    po1++;
                }
                if (Lista_elektarana[i].Tip_name == "Turbo796")
                {
                    po2++;
                }
                if (Lista_elektarana[i].Tip_name == "HidroMer71")
                {
                    po3++;
                }
                if (Lista_elektarana[i].Tip_name == "HidroMer81")
                {
                    po4++;
                }
                if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                {
                    po5++;
                }
            }
            Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike

        }
        #endregion
        #region Dodavanje + validacija
        private void Get_image_path_plus_Id_validation(string image_name)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "WaterImages", image_name);
            var uri = new Uri(path);
            string stringId = string.Empty;
            stringId = variableID.ToString();
            bool isok = true;
            if (stringId.Trim().Equals(""))
            {
                MessageBox.Show("ID polje mora biti popunjeno!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isok = false;
                stringId = string.Empty;
            }
            else
            {
                try
                {
                    int broj = Int32.Parse(stringId);
                    for (int i = 0; i < Lista_elektarana.Count; i++)
                    {
                        if (stringId == Lista_elektarana[i].Id.ToString())
                        {
                            isok = false;
                            MessageBox.Show("ID mora imati jedinstvenu vrednost!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            stringId = string.Empty;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("ID mora biti ceo broj!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    isok = false;
                    stringId = string.Empty;
                }
            }
            if (isok)
            {
                if (variableNAME.Trim().Equals("") == false)
                {
                    stringId = string.Empty;
                    variableSLIKA.TypeImage = uri.ToString();
                    Lista_elektarana.Add(new WaterMachine { Id = Int32.Parse(variableID), Name = variableNAME, Tip_name = variableTIP.TypeName, Vrednost = variableVREDNOST, Tip_image = variableSLIKA.TypeImage });
                    serializer.SerializeObject<ObservableCollection<WaterMachine>>(Lista_elektarana, "fajl_sa_podacima.xml");
                    #region statistika
                    po1 = 0;             //u njih ce broj objekata po vrstama
                    po2 = 0;
                    po3 = 0;
                    po4 = 0;
                    po5 = 0;
                    //pocetna statistika
                    for (int i = 0; i < Lista_elektarana.Count; i++)
                    {
                        if (Lista_elektarana[i].Tip_name == "Turbo795")
                        {
                            po1++;
                        }
                        if (Lista_elektarana[i].Tip_name == "Turbo796")
                        {
                            po2++;
                        }
                        if (Lista_elektarana[i].Tip_name == "HidroMer71")
                        {
                            po3++;
                        }
                        if (Lista_elektarana[i].Tip_name == "HidroMer81")
                        {
                            po4++;
                        }
                        if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                        {
                            po5++;
                        }
                    }
                    Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku
                    #endregion
                }
                else
                {
                    MessageBox.Show("NAME polje mora biti popunjeno!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void OnAdd()
        {
            if (search_time == false && filter_time == false)
            {
                if (variableTIP.TypeName == "Turbo795")
                {
                    po1++;
                    Get_image_path_plus_Id_validation("slika1.jpg");
                }
                else if (variableTIP.TypeName == "Turbo796")
                {
                    po2++;
                    Get_image_path_plus_Id_validation("slika2.jpg");
                }
                else if (variableTIP.TypeName == "HidroMer71")
                {
                    po3++;
                    Get_image_path_plus_Id_validation("slika3.jpg");
                }
                else if (variableTIP.TypeName == "HidroMer81")
                {
                    po4++;
                    Get_image_path_plus_Id_validation("slika4.jpg");
                }
                else if (variableTIP.TypeName == "SuperAqua10")
                {
                    po5++;
                    Get_image_path_plus_Id_validation("slika5.jpg");
                }
                else
                {
                    MessageBox.Show("Potrebno je izabrati tip iz comboBoxa!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Dodavanje u toku pretrage i filtriranja nije dozvoljeno!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion
        #region PROPERTIES za tabelu
        public string VariableID
        {
            get { return variableID; }
            set
            {
                if (variableID != value)
                {
                    variableID = value;
                    OnPropertyChanged("VariableID");
                }
            }
        }
        public string VariableNAME
        {
            get { return variableNAME; }
            set
            {
                if (variableNAME != value)
                {
                    variableNAME = value;
                    OnPropertyChanged("VariableNAME");
                }
            }
        }
        public string VariableTIP
        {
            get { return variableTIP.TypeName; }
            set
            {
                if (variableTIP.TypeName != value)
                {
                    variableTIP.TypeName = value;
                    OnPropertyChanged("VariableTIP");

                }
            }
        }
        public string VariableSLIKA
        {
            get { return variableSLIKA.TypeName; }
            set
            {
                if (variableSLIKA.TypeName != value)
                {
                    variableSLIKA.TypeName = value;
                    OnPropertyChanged("VariableTIP");
                }
            }
        }
        public double VariableVREDNOST
        {
            get { return variableVREDNOST; }
            set
            {
                if (variableVREDNOST != value)
                {
                    variableVREDNOST = value;
                    OnPropertyChanged("VariableVREDNOST");
                }
            }
        }
        #endregion
        #region Logika za komandu brisanja iz tabele
        private WaterMachine selectedElektrana;
        //property
        public WaterMachine SelectedElektrana
        {
            get { return selectedElektrana; }
            set
            {
                selectedElektrana = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        private bool CanDelete()
        {
            return SelectedElektrana != null;               //provera da li u listi imamo nesto
        }
        private void OnDelete()
        {
            for (int i = 0; i < Lista_elektarana.Count; i++)
            {
                if (Lista_elektarana[i] == SelectedElektrana)
                {
                    if (Lista_elektarana[i].Tip_name == "Turbo795")
                    {
                        po1--;
                    }
                    if (Lista_elektarana[i].Tip_name == "Turbo796")
                    {
                        po2--;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer71")
                    {
                        po3--;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer81")
                    {
                        po4--;
                    }
                    if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                    {
                        po5--;
                    }
                }
            }
            if (search_time == false)
            {
                Lista_elektarana.Remove(SelectedElektrana);                           //obrisi iz liste onog selektovanog
                serializer.SerializeObject<ObservableCollection<WaterMachine>>(Lista_elektarana, "fajl_sa_podacima.xml");
                #region statistika
                po1 = 0;             //u njih ce broj objekata po vrstama
                po2 = 0;
                po3 = 0;
                po4 = 0;
                po5 = 0;
                //pocetna statistika
                for (int i = 0; i < Lista_elektarana.Count; i++)
                {
                    if (Lista_elektarana[i].Tip_name == "Turbo795")
                    {
                        po1++;
                    }
                    if (Lista_elektarana[i].Tip_name == "Turbo796")
                    {
                        po2++;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer71")
                    {
                        po3++;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer81")
                    {
                        po4++;
                    }
                    if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                    {
                        po5++;
                    }
                }
                Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku
                #endregion
            }
            else
            {
                MessageBox.Show("Brisanje u toku pretrage nije dozvoljeno!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
        #region Statistika
        public string Procenti1
        {
            get { return procenti1; }
            set
            {
                if (procenti1 != value)
                {
                    procenti1 = value;
                    OnPropertyChanged("Procenti1");
                }
            }
        }
        public string Procenti2
        {
            get { return procenti2; }
            set
            {
                if (procenti2 != value)
                {
                    procenti2 = value;
                    OnPropertyChanged("Procenti2");
                }
            }
        }
        public string Procenti3
        {
            get { return procenti3; }
            set
            {
                if (procenti3 != value)
                {
                    procenti3 = value;
                    OnPropertyChanged("Procenti3");
                }
            }
        }
        public string Procenti4
        {
            get { return procenti4; }
            set
            {
                if (procenti4 != value)
                {
                    procenti4 = value;
                    OnPropertyChanged("Procenti4");
                }
            }
        }
        public string Procenti5
        {
            get { return procenti5; }
            set
            {
                if (procenti5 != value)
                {
                    procenti5 = value;
                    OnPropertyChanged("Procenti5");
                }
            }
        }
        public void Statistic_method(double p1, double p2, double p3, double p4, double p5)
        {
            if (Lista_elektarana.Count > 0)
            {
                int ukupno = Lista_elektarana.Count;                              //ukupan broj objekata
                double rez1 = Math.Round(((p1 * 100) / ukupno), 3);               //procenat postojanja prvog
                double rez2 = Math.Round(((p2 * 100) / ukupno), 3);
                double rez3 = Math.Round(((p3 * 100) / ukupno), 3);
                double rez4 = Math.Round(((p4 * 100) / ukupno), 3);
                double rez5 = Math.Round(((p5 * 100) / ukupno), 3);

                Procenti1 = rez1.ToString();
                Procenti2 = rez2.ToString();
                Procenti3 = rez3.ToString();
                Procenti4 = rez4.ToString();
                Procenti5 = rez5.ToString();
            }
            else
            {
                Procenti1 = "0";
                Procenti2 = "0";
                Procenti3 = "0";
                Procenti4 = "0";
                Procenti5 = "0";
            }
        }
        #endregion
        #region SEARCH

        #region property for search
        public string VariableSEARCH
        {
            get { return variableSEARCH; }
            set
            {
                if (variableSEARCH != value)
                {
                    variableSEARCH = value;
                    OnPropertyChanged("VariableSEARCH");
                }
            }
        }

        public bool VariableRADIONAME
        {
            get { return variableRADIONAME; }
            set
            {
                if (variableRADIONAME != value)
                {
                    variableRADIONAME = value;
                    OnPropertyChanged("VariableRADIONAME");
                }
            }
        }


        public bool VariableRADIOTYPE
        {
            get { return variableRADIOTYPE; }
            set
            {
                if (variableRADIOTYPE != value)
                {
                    variableRADIOTYPE = value;
                    OnPropertyChanged("VariableRADIOTYPE");
                }
            }
        }

        #endregion

        //metoda za pocetak search moda
        private void OnSearch()
        {
            if (search_time == false && filter_time == false)
            {
                //prvo proveravam da li puponjeno polje za pretragu
                if (variableSEARCH.Trim().Equals("") == false)
                {
                    if (VariableRADIONAME == true)
                    {
                        MessageBox.Show("Mode za pretragu je poceo!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        search_time = true;
                        for (int i = 0; i < Lista_elektarana.Count; i++)
                        {
                            Search_lista.Add(Lista_elektarana[i]);
                        }

                        Lista_elektarana.Clear();               //brisem sve iz liste
                        for (int i = 0; i < Search_lista.Count; i++)
                        {
                            if (Search_lista[i].Name == variableSEARCH)
                            {
                                Lista_elektarana.Add(Search_lista[i]);
                            }
                        }
                        #region statistika
                        po1 = 0;             //u njih ce broj objekata po vrstama
                        po2 = 0;
                        po3 = 0;
                        po4 = 0;
                        po5 = 0;
                        //pocetna statistika
                        for (int i = 0; i < Lista_elektarana.Count; i++)
                        {
                            if (Lista_elektarana[i].Tip_name == "Turbo795")
                            {
                                po1++;
                            }
                            if (Lista_elektarana[i].Tip_name == "Turbo796")
                            {
                                po2++;
                            }
                            if (Lista_elektarana[i].Tip_name == "HidroMer71")
                            {
                                po3++;
                            }
                            if (Lista_elektarana[i].Tip_name == "HidroMer81")
                            {
                                po4++;
                            }
                            if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                            {
                                po5++;
                            }
                        }
                        Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike
                        #endregion
                    }
                    else if (VariableRADIOTYPE == true)
                    {
                        MessageBox.Show("Mode za pretragu je poceo!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        search_time = true;
                        for (int i = 0; i < Lista_elektarana.Count; i++)
                        {
                            Search_lista.Add(Lista_elektarana[i]);
                        }

                        Lista_elektarana.Clear();               //brisem sve iz liste
                        for (int i = 0; i < Search_lista.Count; i++)
                        {
                            if (Search_lista[i].Tip_name == variableSEARCH)
                            {
                                Lista_elektarana.Add(Search_lista[i]);
                            }
                        }

                        #region statistika
                        po1 = 0;             //u njih ce broj objekata po vrstama
                        po2 = 0;
                        po3 = 0;
                        po4 = 0;
                        po5 = 0;
                        //pocetna statistika
                        for (int i = 0; i < Lista_elektarana.Count; i++)
                        {
                            if (Lista_elektarana[i].Tip_name == "Turbo795")
                            {
                                po1++;
                            }
                            if (Lista_elektarana[i].Tip_name == "Turbo796")
                            {
                                po2++;
                            }
                            if (Lista_elektarana[i].Tip_name == "HidroMer71")
                            {
                                po3++;
                            }
                            if (Lista_elektarana[i].Tip_name == "HidroMer81")
                            {
                                po4++;
                            }
                            if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                            {
                                po5++;
                            }
                        }
                        Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Izaberite Name ili Type!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Polje za Search mora biti popunjeno!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //metoda za kraj search-a
        private void OnEndSearch()
        {
            if (search_time == true)
            {
                search_time = false;
                Lista_elektarana.Clear();
                for (int i = 0; i < Search_lista.Count; i++)
                {
                    Lista_elektarana.Add(Search_lista[i]);
                }
                MessageBox.Show("Mode za pretragu je zavrsen!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                Search_lista.Clear();
                #region statistika
                po1 = 0;             //u njih ce broj objekata po vrstama
                po2 = 0;
                po3 = 0;
                po4 = 0;
                po5 = 0;
                //pocetna statistika
                for (int i = 0; i < Lista_elektarana.Count; i++)
                {
                    if (Lista_elektarana[i].Tip_name == "Turbo795")
                    {
                        po1++;
                    }
                    if (Lista_elektarana[i].Tip_name == "Turbo796")
                    {
                        po2++;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer71")
                    {
                        po3++;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer81")
                    {
                        po4++;
                    }
                    if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                    {
                        po5++;
                    }
                }
                Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike
                #endregion
            }
        }
        #endregion
        #region FILTER
        public bool VariableRADIOOK
        {
            get { return variableRADIOOK; }
            set
            {
                if (variableRADIOOK != value)
                {
                    variableRADIOOK = value;
                    OnPropertyChanged("VariableRADIOOK");
                }
            }
        }

        public bool VariableRADIONO
        {
            get { return variableRADIONO; }
            set
            {
                if (variableRADIONO != value)
                {
                    variableRADIONO = value;
                    OnPropertyChanged("VariableRADIONO");
                }
            }
        }

        private void OnFilter()
        {
            if (filter_time == false && search_time == false)
            {
                if (variableRADIOOK == true)
                {
                    MessageBox.Show("Mode za filtriranje je poceo!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    filter_time = true;
                    for (int i = 0; i < Lista_elektarana.Count; i++)
                    {
                        Search_lista.Add(Lista_elektarana[i]);
                    }
                    Lista_elektarana.Clear();               //brisem sve iz liste
                    for (int i = 0; i < Search_lista.Count; i++)
                    {
                        if (Search_lista[i].Vrednost >= 670 && Search_lista[i].Vrednost <= 735)
                        {
                            Lista_elektarana.Add(Search_lista[i]);
                        }
                    }
                    #region statistika
                    po1 = 0;             //u njih ce broj objekata po vrstama
                    po2 = 0;
                    po3 = 0;
                    po4 = 0;
                    po5 = 0;
                    //pocetna statistika
                    for (int i = 0; i < Lista_elektarana.Count; i++)
                    {
                        if (Lista_elektarana[i].Tip_name == "Turbo795")
                        {
                            po1++;
                        }
                        if (Lista_elektarana[i].Tip_name == "Turbo796")
                        {
                            po2++;
                        }
                        if (Lista_elektarana[i].Tip_name == "HidroMer71")
                        {
                            po3++;
                        }
                        if (Lista_elektarana[i].Tip_name == "HidroMer81")
                        {
                            po4++;
                        }
                        if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                        {
                            po5++;
                        }
                    }
                    Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike
                    #endregion


                }
                else if (variableRADIONO == true)
                {
                    MessageBox.Show("Mode za filtriranje je poceo!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    filter_time = true;
                    for (int i = 0; i < Lista_elektarana.Count; i++)
                    {
                        Search_lista.Add(Lista_elektarana[i]);
                    }
                    Lista_elektarana.Clear();               //brisem sve iz liste
                    for (int i = 0; i < Search_lista.Count; i++)
                    {
                        if (Search_lista[i].Vrednost < 670 || Search_lista[i].Vrednost > 735)
                        {
                            Lista_elektarana.Add(Search_lista[i]);
                        }
                    }
                    #region statistika
                    po1 = 0;             //u njih ce broj objekata po vrstama
                    po2 = 0;
                    po3 = 0;
                    po4 = 0;
                    po5 = 0;
                    //pocetna statistika
                    for (int i = 0; i < Lista_elektarana.Count; i++)
                    {
                        if (Lista_elektarana[i].Tip_name == "Turbo795")
                        {
                            po1++;
                        }
                        if (Lista_elektarana[i].Tip_name == "Turbo796")
                        {
                            po2++;
                        }
                        if (Lista_elektarana[i].Tip_name == "HidroMer71")
                        {
                            po3++;
                        }
                        if (Lista_elektarana[i].Tip_name == "HidroMer81")
                        {
                            po4++;
                        }
                        if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                        {
                            po5++;
                        }
                    }
                    Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike
                    #endregion
                }
                else
                {
                    MessageBox.Show("Izaberite 'Out of range values' ili 'Expected values'!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnEndFilter()
        {
            if (filter_time == true)
            {
                filter_time = false;
                Lista_elektarana.Clear();
                for (int i = 0; i < Search_lista.Count; i++)
                {
                    Lista_elektarana.Add(Search_lista[i]);
                }
                MessageBox.Show("Mode za filteriranje je zavrsen!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                Search_lista.Clear();
                #region statistika
                po1 = 0;             //u njih ce broj objekata po vrstama
                po2 = 0;
                po3 = 0;
                po4 = 0;
                po5 = 0;
                //pocetna statistika
                for (int i = 0; i < Lista_elektarana.Count; i++)
                {
                    if (Lista_elektarana[i].Tip_name == "Turbo795")
                    {
                        po1++;
                    }
                    if (Lista_elektarana[i].Tip_name == "Turbo796")
                    {
                        po2++;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer71")
                    {
                        po3++;
                    }
                    if (Lista_elektarana[i].Tip_name == "HidroMer81")
                    {
                        po4++;
                    }
                    if (Lista_elektarana[i].Tip_name == "SuperAqua10")
                    {
                        po5++;
                    }
                }
                Statistic_method(po1, po2, po3, po4, po5);       //menjam statistiku na pocetku tj postavljam trenutno stanje statistike
                #endregion
            }
        }

        #endregion
    }
}
