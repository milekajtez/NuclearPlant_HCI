using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PZ3_NetworkService.ViewModel
{
    //imam nasledjivanje da bi mogao da dodeljujem trenutnom viewModelu onaj koji treba da bude trnutni
    public class ReportViewModel : BindableBase             
    {
        #region Polja,konstruktor i properties
        private MyICommand showCommand;          //komadna koja ce izvrsavati metodu pokazivanja podataka u texboxu
        private string big_string;               //string koji ce se prikazivati u textboxu

        public ReportViewModel() {
            Big_string = "";
            ShowCommand = new MyICommand(Show_method);
            
        }
        
        public MyICommand ShowCommand { get => showCommand; set => showCommand = value; }
        public string Big_string
        {
            get { return big_string; }
            set
            {
                if (big_string != value)
                {
                    big_string = value;
                    OnPropertyChanged("Big_string");
                }
            }
        }
        #endregion
        #region Metoda za popunjavanje TextBoxa
        public void Show_method()
        {
            string s = "";

            int brojac = 0;
            Big_string = "";

            for (int i = 0; i < NetworkDataViewModel.Lista_elektarana.Count; i++)
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(@"log.txt"))
                {
                    char separators = ',';
                    string[] rez;


                    Big_string += String.Format("Object {0}", i);
                    Big_string += "\n";
                    s = "";
                    s = file.ReadLine();
                    while ((s = file.ReadLine()) != null)
                    {
                        rez = s.Split(separators);
                        if (Int32.Parse(rez[1]) == i)
                        {
                            Big_string += "\t";
                            Big_string += rez[0] + rez[1] + " CHANGED STATE: " + rez[2];
                            Big_string += "\n";
                        }
                        brojac++;
                    }
                    Big_string += "\n";
                }
            }
        }
        #endregion
    }
}
