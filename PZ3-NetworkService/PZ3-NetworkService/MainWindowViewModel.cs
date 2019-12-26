using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PZ3_NetworkService
{
    public class MainWindowViewModel : BindableBase
    {
        //inicijalizujem komandu za prelazak sa jednog na drugi pogled+njegov property
        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand<string> Command1 { get; private set; }

        //definisem viewmodele
        private NetworkViewModel networkViewModel = new NetworkViewModel();    //ovaj ce mi pogled biti kao home tj kad pokrenem programon ce biti prikazan
        private NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        private DataChartViewModel dataChartViewModel = new DataChartViewModel();
        private ReportViewModel reportViewModel = new ReportViewModel();
        
        //ovo ce mibiti trenutni view model koji koristim.kadda menjam poglede treba i view modele da menjam
        private BindableBase currentViewModel;

        public MainWindowViewModel()                            //konstruktor se poziva kad pokrenem program tj MainWindow
        {
            
            if (Keyboard.IsKeyDown(Key.F5))
            {
                Command1 = new MyICommand<string>(OnNav);
            }
            else {
                NavCommand = new MyICommand<string>(OnNav);
                CurrentViewModel = networkViewModel;
            }
                
        }

        public BindableBase CurrentViewModel                    //property za trenutni model
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);  
            }
        }

        private void OnNav(string destination)                  //metoda koja nam govori koji string je vezan tj pravilo za koji viewModel
        {
            switch (destination)
            {
                case "network":
                    CurrentViewModel = networkViewModel;
                    break;
                case "data":
                    CurrentViewModel = networkDataViewModel;
                    break;
                case "chart":
                    CurrentViewModel = dataChartViewModel;
                    break;
                case "report":
                    CurrentViewModel = reportViewModel;
                    break;
            }
        }
    }
}
