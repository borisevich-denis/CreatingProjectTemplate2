using Ascon.Pilot.Theme.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    /// <summary>
    /// Логика взаимодействия для StageCreate.xaml
    /// </summary>
    public partial class StageCreate : UserControl
    {
        public StageCreate()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((PureWindow)Parent).Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var path = System.IO.Path.GetTempPath() + "Отчет.txt";
            var stWr = File.CreateText(path);
           foreach (var elem in ((TreeViewModel)DataContext).resultCreation )
           {
               stWr.WriteLine(elem.Text);
           }
           stWr.Close();
           stWr.Dispose();
           Process.Start("notepad.exe", path);
        }
    }
}
