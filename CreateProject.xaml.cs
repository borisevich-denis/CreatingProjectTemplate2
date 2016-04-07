using Ascon.Pilot.Theme.Controls;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class CreateProject : UserControl
    {
        //private IEnumerable<IDataObject> _selection;
        public PureWindow _win;
        public CreateProject(PureWindow win)
        {
            //_selection = selection;
            InitializeComponent();
            _win = win;
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (TC1.Items.Count - 1 > TC1.SelectedIndex)
            {
                TC1.SelectedIndex += 1;
                
            }
            if (TC1.Items.Count - 1 == TC1.SelectedIndex) { next.IsEnabled = false; }
            if (TC1.SelectedIndex > 0) 
            { 
                back.IsEnabled = true;
                tb1.Visibility = Visibility.Visible;
                tb2.Visibility = Visibility.Visible;
                tb3.Visibility = Visibility.Visible;
            }
            
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (TC1.SelectedIndex > 0)
            {
                TC1.SelectedIndex -= 1;
            }
            if (TC1.Items.Count - 1 > TC1.SelectedIndex) { next.IsEnabled = true; }
            if (TC1.SelectedIndex == 0) 
            { 
                back.IsEnabled = false;
                tb1.Visibility = Visibility.Collapsed;
                tb2.Visibility = Visibility.Collapsed;
                tb3.Visibility = Visibility.Collapsed;
                tblock.Text = "Добро пожаловать в мастер создания проекта по шаблону";
            }

        }

        private void TC1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TC1.SelectedIndex == 1)
            { 
                tb1.IsChecked = true;
                tb2.IsChecked = false;
                tb3.IsChecked = false;
                tblock.Text = "Структура состава проекта в Обозревателе документов";
            }
            if (TC1.SelectedIndex == 2)
            {
                tb1.IsChecked = false;
                tb2.IsChecked = true;
                tb3.IsChecked = false;
                tblock.Text = "Структура папок проекта в Pilot-Storage и шаблоны файлов";
            }
            if (TC1.SelectedIndex == 3)
            {
                tb1.IsChecked = false;
                tb2.IsChecked = false;
                tb3.IsChecked = true;
                tblock.Text = "Выбор папки для нового проекта";
            }
        }
    }
}
