using System;
using System.Windows;

namespace VSRPP4
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly string DatabaseName = "sqlite.db";
        static readonly string FolderPath = Environment.CurrentDirectory;
        public static readonly string DatabasePath = System.IO.Path.Combine(FolderPath, DatabaseName);
    }
}
