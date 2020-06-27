using Newtonsoft.Json;
using PCLStorage;
using Plugin.Clipboard;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace manager
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            setup();
            
        }

        private async void setup()
        {
            await LoadPasswordList();            
        }


        private void Edit(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var clickedPassword = menuItem.CommandParameter as passwordManager;
            int index = 0;
            for(int i = 0; i< passwordList.Instance.passwords.Count; i++)
            {
                if (passwordList.Instance.passwords[i] == clickedPassword)
                {
                    passwordList.Instance.lastChangedIndex = i;
                }
            }
            PopUpEditAsync();
            
        }

        private async void PopUpEditAsync()
        {
            bool edit = true;
            await PopupNavigation.Instance.PushAsync(new PopUp(edit));
            
        }

        private async void DeleteAsync(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var clickedPassword = menuItem.CommandParameter as passwordManager;
            passwordList.Instance.passwords.Remove(clickedPassword);
            await passwordList.Instance.SaveDataAsync();
        }

        private void showPassword(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var passwordField = button.CommandParameter as Entry;
            passwordField.IsPassword = !passwordField.IsPassword;

        }
       
        public async Task LoadPasswordList()
        {
            passwordList.Instance.passwords = await LoadDataAsync();
        }

        public async Task<ObservableCollection<passwordManager>> LoadDataAsync()
        {
            string folderName = "saveData";
            string filename = "saveData.txt";
            IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName,
            PCLStorage.CreationCollisionOption.OpenIfExists);
            IFile file = await folder.GetFileAsync(filename);
            string loadedContent = await file.ReadAllTextAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<passwordManager>>(loadedContent);
        }

        private async void AddAsync(object sender, EventArgs e)
        {
            bool edit = false;
            await PopupNavigation.Instance.PushAsync(new PopUp(edit));
        }        

        private void CopyToClipboard(object sender, SelectedItemChangedEventArgs e)
        {
            
            string password = ((passwordManager)e.SelectedItem).password;
            CrossClipboard.Current.SetText(password);
        }

        private bool isRowEven;
        private void ViewCell_Appearing(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;

            if (this.isRowEven)
            {
                if (viewCell.View != null)
                {
                    viewCell.View.BackgroundColor = Color.BlanchedAlmond;
                }
            }
            else
            {
                
                if (viewCell.View != null)
                {
                    viewCell.View.BackgroundColor = Color.Beige;
                }
            }

            this.isRowEven = !this.isRowEven;
        }

        private async void SwipedLeft(object sender, SwipedEventArgs e)
        {
            var clickedPassword = e.Parameter as passwordManager;
            passwordList.Instance.passwords.Remove(clickedPassword);
            await passwordList.Instance.SaveDataAsync();
        }        
    }
}
