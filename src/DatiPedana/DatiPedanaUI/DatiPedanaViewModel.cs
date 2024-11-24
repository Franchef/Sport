using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace DatiPedanaUI
{
    public partial class DatiPedanaViewModel : ObservableObject
    {
        public DatiPedanaViewModel()
        {
            
        }

        [ObservableProperty]
        private string _filePath = string.Empty;

        [RelayCommand]
        public void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "Text Files|*.txt";
            dialog.InitialDirectory =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Title = "Please select data text file.";
            if (dialog.ShowDialog() == true)
            {
                if (System.IO.File.Exists(dialog.FileName))
                {
                    FilePath = dialog.FileName;
                }
            }
        }
    }
}
