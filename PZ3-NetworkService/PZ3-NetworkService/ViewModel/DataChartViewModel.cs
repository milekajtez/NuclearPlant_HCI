using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PZ3_NetworkService.ViewModel
{
    public class DataChartViewModel : BindableBase
    {
        public static ObservableCollection<Model.WaterMachine> Grafik_lista { get; set; }

        private DataIO serializer = new DataIO();
        public MyICommand<ComboBox> GraphCommand { get; set; }
        
        private WaterMachine selectedObject = null;         //selektovan objekat iz comboboxa
        private WaterMachine obj_for_graphic = null;

        private double variableX1;          //linija 1
        private double variableY1;
        private double variableX2;
        private double variableY2;

        private double variableX3;          //linija 2
        private double variableY3;
        private double variableX4;
        private double variableY4;
        
        
        public DataChartViewModel()
        {
            Grafik_lista = NetworkDataViewModel.Lista_elektarana;
            GraphCommand = new MyICommand<ComboBox>(Graph_method);
        }

        #region properties
        public WaterMachine SelectedObject { get => selectedObject; set => selectedObject = value; }

        public double VariableX1
        {
            get { return variableX1; }
            set
            {
                if (variableX1 != value)
                {
                    variableX1 = value;
                    OnPropertyChanged("VariableX1");
                }
            }
        }

        public double VariableX2
        {
            get { return variableX2; }
            set
            {
                if (variableX2 != value)
                {
                    variableX2 = value;
                    OnPropertyChanged("variableX2");
                }
            }
        }

        public double VariableX3
        {
            get { return variableX3; }
            set
            {
                if (variableX3 != value)
                {
                    variableX3 = value;
                    OnPropertyChanged("VariableX3");
                }
            }
        }

        public double VariableX4
        {
            get { return variableX4; }
            set
            {
                if (variableX4 != value)
                {
                    variableX4 = value;
                    OnPropertyChanged("VariableX4");
                }
            }
        }

        



        public double VariableY1
        {
            get { return variableY1; }
            set
            {
                if (variableY1 != value)
                {
                    variableY1 = value;
                    OnPropertyChanged("VariableY1");
                }
            }
        }

        public double VariableY2
        {
            get { return variableY2; }
            set
            {
                if (variableY2 != value)
                {
                    variableY2 = value;
                    OnPropertyChanged("VariableY2");
                }
            }
        }

        public double VariableY3
        {
            get { return variableY3; }
            set
            {
                if (variableY3 != value)
                {
                    variableY3 = value;
                    OnPropertyChanged("VariableY3");
                }
            }
        }

        public double VariableY4
        {
            get { return variableY4; }
            set
            {
                if (variableY4 != value)
                {
                    variableY4 = value;
                    OnPropertyChanged("VariableY4");
                }
            }
        }

        

        #endregion

        public void Graph_method(ComboBox cb)
        {
            for (int i = 0; i < Grafik_lista.Count; i++)
            {
                if (Grafik_lista[i] == SelectedObject)
                {
                    obj_for_graphic = SelectedObject;
                }
            }

            if (obj_for_graphic != null)
            {
                //MessageBox.Show("Aasas!", "Greska!", MessageBoxButton.OK);

                // linija 1
                VariableX1 = 150;                          
                VariableY1 = 287;
                VariableX2 = 150;
                VariableY2 = 287 - (obj_for_graphic.Vrednost) / 3;
                VariableX3 = 350;
                VariableY3 = 287 - (obj_for_graphic.Vrednost) / 3;
                VariableX4 = 350;
                VariableY4 = 287;
                

            }
            else
            {
                MessageBox.Show("Objekat nije izabran iz combo boxa!", "Error!", MessageBoxButton.OK,MessageBoxImage.Error);
            } 
            
        }
    }
}
