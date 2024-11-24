using System.Windows.Controls;

namespace SportApp.Interfaces
{
    public interface IEntryPoint
    {
        public string Name { get; init; }

        public UserControl UI { get; init; }
    }
}
