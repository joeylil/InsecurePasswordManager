using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace manager
{
    public class passwordManager : INotifyPropertyChanged
    {

        

        private string _title;
        private string _username;
        private string _website;
        private string _password;
        private bool _isWebsite;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("title"));
            }
        }
        public string username 
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("username"));
            }
        }
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("password"));
            }
        }
        public string website
        {
            get => _website;
            set
            {
                _website = value;
                if(_website != "" && _website != null)
                {
                    isWebsite = true;
                }
                else
                {
                    isWebsite = false;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("website"));
            }
        }

        public bool isWebsite
        {
            get => _isWebsite;
            set
            {
                _isWebsite = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isWebsite"));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
