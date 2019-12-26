using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PZ3_NetworkService.Model
{
    public class NetworkDataModel
    {  
    }

    #region Klasa Watermachine
    public class WaterMachine : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private Type tip = new Type();
        private double vrednost;

        public WaterMachine() { }

        public WaterMachine(int i, string n, string nt, string ni, double v) {
            id = i;
            name = n;
            nt = tip.TypeName;
            ni = tip.TypeImage;
            vrednost = v;
        }
        #region PROPERTES
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Tip_name
        {
            get { return tip.TypeName; }
            set
            {
                if (tip.TypeName != value)
                {
                    tip.TypeName = value;
                    RaisePropertyChanged("Tip_name");
                }
            }
        }
        
        public string Tip_image
        {
            get { return tip.TypeImage; }
            set
            {
                if (tip.TypeImage != value)
                {
                    tip.TypeImage = value;
                    RaisePropertyChanged("Tip_image");
                }
            }
        }


        public double Vrednost
        {
            get { return vrednost; }
            set
            {
                if (vrednost != value)
                {
                    vrednost = value;
                    RaisePropertyChanged("Vrednost");
                }
            }
        }

        
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #region Metoda ToString
        public override string ToString()
        {
            return "Object with id: " + Id + " (Name: " + Name + ")";
        }
        #endregion
    }
    #endregion
    #region KLASA TYPE
    public class Type : INotifyPropertyChanged
    {
        private string typeName;
        private string typeImage;


        public string TypeName
        {
            get { return typeName; }
            set
            {
                if (typeName != value)
                {
                    typeName = value;
                    RaisePropertyChanged("TypeName");
                }
            }
        }

        public string TypeImage
        {
            get { return typeImage; }
            set
            {
                if (typeImage != value)
                {
                    typeImage = value;
                    RaisePropertyChanged("TypeImage");
                }
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
    #endregion
}
