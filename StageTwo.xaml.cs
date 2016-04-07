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
    /// Логика взаимодействия для StageTwo.xaml
    /// </summary>
    public partial class StageTwo : UserControl
    {
        public StageTwo()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
            {
                Elem.Check = false;
                CheckTree(Elem.ChildNodes, false);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)//false
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
            {                
                if (Elem.TypeObj == "File")
                {
                    Elem.Check = false;
                }
                CheckTreeFile(Elem.ChildNodes, false);
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)//true
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
            {
                if (Elem.TypeObj == "File")
                {
                    Elem.Check = true;
                }
                CheckTreeFile(Elem.ChildNodes, true);
            }
        }

        private void CheckTreeFile(ObservableCollection<ElementNodeViewModel> Tree, bool b)
        {
            foreach (var Elem in Tree)
            {
                if (Elem.TypeObj == "File")
                {
                    Elem.Check = b;
                }
                CheckTreeFile(Elem.ChildNodes, b);
            }
        }

        
    }
}
