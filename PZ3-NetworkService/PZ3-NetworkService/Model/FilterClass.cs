using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class FilterClass : INotifyPropertyChanged
    {
        private bool okValue;             //da li je radio button pritisnut
        private bool noValue;

        public bool OkValue
        {
            get { return okValue; }
            set
            {
                if (okValue != value)
                {
                    okValue = value;
                    RaisePropertyChanged("IsCheckedOkValue");
                }
            }
        }

        public bool NoValue
        {
            get { return noValue; }
            set
            {
                if (noValue != value)
                {
                    noValue = value;
                    RaisePropertyChanged("IsCheckedNoValue");
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
}
