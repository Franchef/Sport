using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace SportApp.Interfaces
{
    internal sealed class EntryPoint<T> : IEntryPoint where T : UserControl
    {
        private EntryPoint(string name, UserControl ui)
        {
            Name=name;
            UI = ui;
        }
        public EntryPoint(IServiceProvider serviceProvider) 
        {
            Name = typeof(T).Name;
            UI = serviceProvider.GetRequiredService<T>();
        }

        public string Name { get; init; }
        public UserControl UI { get; init; }

        public static IEntryPoint Create(string name, T ui)
        {
            return new EntryPoint<T>(name, ui);
        }
    }
}
