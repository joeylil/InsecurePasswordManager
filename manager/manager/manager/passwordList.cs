using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace manager
{
    public sealed class  passwordList : INotifyPropertyChanged
    {
        private static passwordList _instance = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public static passwordList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new passwordList();
                }
                return _instance;
            }
        }

        private ObservableCollection<passwordManager> _passwords;

        public ObservableCollection<passwordManager> passwords
        {
            get
            {
                return _passwords;
            }
            set
            {
                _passwords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("passwords"));
            }
        }

        public int lastChangedIndex { get; set; }


        public async Task SaveDataAsync()
        {
            string json = JsonConvert.SerializeObject(passwordList.Instance.passwords);
            string folderName = "saveData";
            string filename = "saveData.txt";
            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName,
            CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(filename,
            CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(json);
        }
    }
}
