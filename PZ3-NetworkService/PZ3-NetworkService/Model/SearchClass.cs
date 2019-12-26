using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class SearchClass : INotifyPropertyChanged
    {
        private string search_string;
        private bool isCheckedName;             //da li je radio button pritisnut
        private bool isCheckedType;

        public string Search_string
        {
            get { return search_string; }
            set
            {
                if (search_string != value)
                {
                    search_string = value;
                    RaisePropertyChanged("Search_string");
                }
            }
        }

        public bool IsCheckedName
        {
            get { return isCheckedName; }
            set
            {
                if (isCheckedName != value)
                {
                    isCheckedName = value;
                    RaisePropertyChanged("IsCheckedName");
                }
            }
        }

        public bool IsCheckedType
        {
            get { return isCheckedType; }
            set
            {
                if (isCheckedType != value)
                {
                    isCheckedType = value;
                    RaisePropertyChanged("IsCheckedType");
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
