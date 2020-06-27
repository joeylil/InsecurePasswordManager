using Newtonsoft.Json;
using PCLStorage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace manager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp
    {
        bool edit { get; set; }
        public PopUp(bool edit)
        {
            InitializeComponent();
            this.edit = edit;
            if (edit)
            {
                title.Text = passwordList.Instance.passwords[passwordList.Instance.lastChangedIndex].title;
                website.Text = passwordList.Instance.passwords[passwordList.Instance.lastChangedIndex].website;
                username.Text = passwordList.Instance.passwords[passwordList.Instance.lastChangedIndex].username;
                password.Text = passwordList.Instance.passwords[passwordList.Instance.lastChangedIndex].password;
            }
        }

        private async void confirm(object sender, EventArgs e)
        {
            bool missingEntry = false;
            titleError.IsVisible = false;
            UsernameError.IsVisible = false;
            PasswordError.IsVisible = false;

            if (title.Text == "" || title.Text == null)
            {
                titleError.IsVisible = true;
                missingEntry = true;
            }
            if (username.Text == "" || username.Text == null)
            {
                UsernameError.IsVisible = true;
                missingEntry = true;
            }
            if (password.Text == "" || password.Text == null)
            {
                PasswordError.IsVisible = true;
                missingEntry = true;
            }
            if (missingEntry)
            {
                return;
            }

            if (edit)
            {
                int indexOfAdd = calculateInsertion();
                if(indexOfAdd == passwordList.Instance.lastChangedIndex)
                {
                    passwordManager editedPassword = passwordList.Instance.passwords[passwordList.Instance.lastChangedIndex];
                    editedPassword.password = password.Text;
                    editedPassword.title = title.Text;
                    editedPassword.website = website.Text;
                    editedPassword.username = username.Text;
                }
                else
                {
                    passwordList.Instance.passwords.RemoveAt(passwordList.Instance.lastChangedIndex);
                    AddPassword(indexOfAdd);
                }
                   
            }
            else
            {
                int indexOfAdd = calculateInsertion();
                AddPassword(indexOfAdd);
            }
            await passwordList.Instance.SaveDataAsync();
            await PopupNavigation.Instance.PopAsync();
        }

        private void AddPassword(int indexOfAdd)
        {
            if (indexOfAdd < passwordList.Instance.passwords.Count)
            {
                passwordList.Instance.passwords.Insert(indexOfAdd, new passwordManager
                {
                    title = title.Text,
                    website = website.Text,
                    username = username.Text,
                    password = password.Text
                });
            }
            else
            {
                passwordList.Instance.passwords.Add(new passwordManager
                {
                    title = title.Text,
                    website = website.Text,
                    username = username.Text,
                    password = password.Text
                });
            }
        }

        private int calculateInsertion()
        {
            int indexOfAdd = 0;
            if(passwordList.Instance.passwords == null)
            {
                passwordList.Instance.passwords = new ObservableCollection<passwordManager>();
            }
            for (int i = 0; i < passwordList.Instance.passwords.Count; i++)
            {
                int compare = passwordList.Instance.passwords[i].title.CompareTo(title.Text);
                if (compare < 0)
                {
                    indexOfAdd++;                    
                }
                else if (compare > 0)
                {
                    break;
                }
            }
            return indexOfAdd;
        }
    }
}