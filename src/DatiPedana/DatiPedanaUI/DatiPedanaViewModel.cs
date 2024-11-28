using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataPedanaHandler;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

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
                PlatformA.Clear();
                if (File.Exists(dialog.FileName))
                {
                    FilePath = dialog.FileName;
                    foreach (var item in new DatiPedanaFileHandler().ReadPlatformA(dialog.FileName).ToArray())
                        PlatformA.Add(item);
                    foreach (var item in new DatiPedanaFileHandler().ReadPlatformB(dialog.FileName).ToArray())
                        PlatformB.Add(item);
                    EnableEdit = true;
                }
            }
        }

        private DatiPedanaRow[] RiduciAlDecimo(IEnumerable<DatiPedanaRow> data)
        {
            return new DatiPedanaDecimator().ReduceTimePrecision(data, DatiPedanaDecimator.Riduzione.Decimo).ToArray();
        }

        private DatiPedanaRow[] RiduciAlCentesimo(IEnumerable<DatiPedanaRow> data)
        {
            return new DatiPedanaDecimator().ReduceTimePrecision(data, DatiPedanaDecimator.Riduzione.Centesimo).ToArray();
        }

        [RelayCommand]
        public void DecimazioneAlDecimoDiSecondo()
        {
            var itemsA = RiduciAlDecimo(new DatiPedanaFileHandler().ReadPlatformA(FilePath));
            PlatformA.Clear();
            foreach (var item in itemsA)
                PlatformA.Add(item);
            var itemsB = RiduciAlDecimo(new DatiPedanaFileHandler().ReadPlatformB(FilePath));
            PlatformB.Clear();
            foreach (var item in itemsB)
                PlatformB.Add(item);
        }

        [RelayCommand]
        public void DecimazioneAlCentesimoDiSecondo()
        {
            var itemsA = RiduciAlCentesimo(new DatiPedanaFileHandler().ReadPlatformA(FilePath));
            PlatformA.Clear();
            foreach (var item in itemsA)
                PlatformA.Add(item);
            var itemsB = RiduciAlCentesimo(new DatiPedanaFileHandler().ReadPlatformB(FilePath));
            PlatformB.Clear();
            foreach (var item in itemsB)
                PlatformB.Add(item);
        }

        [ObservableProperty]
        private bool _enableEdit = false;

        public ObservableCollection<DatiPedanaRow> PlatformA { get; init; } = new();
        public ObservableCollection<DatiPedanaRow> PlatformB { get; init; } = new();
        [RelayCommand]
        public void SalvaCSVConHeader()
        {
            var filePath = SaveFilePath();
            if (string.IsNullOrWhiteSpace(filePath)) return;
            SalvaFileCSV(filePath, true);
        }
        [RelayCommand]
        public void SalvaSCVSenzaHeader()
        {
            var filePath = SaveFilePath();
            if (string.IsNullOrWhiteSpace(filePath)) return;
            SalvaFileCSV(filePath, true);
        }

        private string SaveFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter ="TXT|*.txt";
            saveFileDialog.InitialDirectory =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Title = "File TXT ridotto";

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null!;
        }

        const char CSVFieldseparator = ';';
        private void SalvaFileCSV(string destinationFilePath, bool conHeader)
        {
            var fileHandler = new DatiPedanaFileHandler();
            fileHandler.CreateCopy(FilePath, destinationFilePath, PlatformA.ToArray(), PlatformB.ToArray());

            //if (File.Exists(destinationFilePath))
            //    File.Delete(destinationFilePath);
            //var sb = new StringBuilder();

            //if (conHeader)
            //    sb.AppendLine(string.Join(CSVFieldseparator, "idx", "Load", "Encoder", "Force", "Speed", "Displ", "Power", "Time"));

            //foreach (var row in PlatformA)
            //    sb.AppendLine(
            //        string.Join(
            //            CSVFieldseparator, 
            //            $"{row.Index}",
            //            row.Load.ToString("F3", CultureInfo.InvariantCulture),
            //            row.Encoder.ToString("F3", CultureInfo.InvariantCulture),
            //            row.Force.ToString("F3", CultureInfo.InvariantCulture),
            //            row.Speed.ToString("F3", CultureInfo.InvariantCulture),
            //            row.Displ.ToString("F3", CultureInfo.InvariantCulture),
            //            row.Power.ToString("F3", CultureInfo.InvariantCulture),
            //            row.Time.ToString("F3", CultureInfo.InvariantCulture)
            //            )
            //        );


            //File.WriteAllText(destinationFilePath, sb.ToString());
        }

    }
}
