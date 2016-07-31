using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для StageOne.xaml
    /// </summary>
    public partial class StageOne : UserControl
    {

        public StageOne()
        {
            InitializeComponent();
        }

        private void button1_Click()
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeObj)
            {
                Elem.Check = true;
                CheckTree(Elem.ChildNodes, true);
            }
        }
        private void CheckTree(ObservableCollection<ElementNodeViewModel> Tree, bool b)
        {
            foreach (var Elem in Tree)
            {
                Elem.Check = b;
                CheckTree(Elem.ChildNodes, b);
            }
        }

        private void button2_Click()
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeObj)
            {
                Elem.Check = false;
                CheckTree(Elem.ChildNodes, false);
            }
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            button1_Click();
        }

        private void nothing_Click(object sender, RoutedEventArgs e)
        {
            button2_Click();
        }

        private void collapse_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var tProj in viewModel.TreeObj)
            {
                tProj.IsExpanded = false;
            }
        }

        private void expand_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var tProj in viewModel.TreeObj)
            {
                tProj.IsExpanded = true;
            }
        }
    }
}
