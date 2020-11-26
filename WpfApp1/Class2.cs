using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp1 
{
    class WPFBinder : INotifyPropertyChanged
    {
        private Guid idValue = Guid.NewGuid();
        private string customerNameValue = String.Empty;
        private string phoneNumberValue = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name2;

        public string Name2
        {
            get { return _name2; }
            set
            {
                if (value != _name2)
                {
                    _name2 = value;
                    OnPropertyChanged("Name2");
                }
            }
        }
    }
}
