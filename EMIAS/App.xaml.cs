using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EMIAS
{

    public partial class App : Application
    {
        private static string theme;

        public static string Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                var dict = new ResourceDictionary { Source = new Uri($"/Resources/{value}.xaml", UriKind.Relative) };

                Current.Resources.MergedDictionaries.RemoveAt(0);
                Current.Resources.MergedDictionaries.Insert(0, dict);

                var resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources");
                File.WriteAllText($"{resourcesPath}\\theme.txt", value);
            }
        }

        public App()
        {
            InitializeComponent();
            var resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources");
            if (File.Exists($"{resourcesPath}\\theme.txt"))
            {
                Theme = File.ReadAllText($"{resourcesPath}\\theme.txt");
            }
        }
    }
}
