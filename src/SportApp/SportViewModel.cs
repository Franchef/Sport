using CommunityToolkit.Mvvm.ComponentModel;
using DatiPedanaUI;
using SportApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportApp
{
    public partial class SportViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "Sport app";

        public SportViewModel()
        {
            EntryPoints = Enumerable.Empty<IEntryPoint>();
            EntryPoints = new List<IEntryPoint>()
            {
                EntryPoint<DatiPedanaUserControl>.Create("Dati pedana", new DatiPedanaUserControl())
            };
        }

        public SportViewModel(IEnumerable<IEntryPoint> entryPoints)
        {
            EntryPoints=entryPoints;
        }

        public IEnumerable<IEntryPoint> EntryPoints { get; }
    }
}
