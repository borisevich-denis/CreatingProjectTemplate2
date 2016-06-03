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



        private void button1_Click()
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

        private void button2_Click()
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
            {
                Elem.Check = false;
                CheckTree(Elem.ChildNodes, false);
            }
        }

        private void button3_Click()//false
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
            {                
                if (Elem.TypeObj == "File")
                {
                    Elem.Check = false;
                }
                CheckTreeFile(Elem.ChildNodes, false, "File");
            }
        }

        private void button4_Click()//true
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var Elem in viewModel.TreeStorage)
            {
                if (Elem.TypeObj == "Project_folder")
                {
                    Elem.Check = true;
                }
                CheckTreeFile(Elem.ChildNodes, true, "Project_folder");
            }
        }

        private void CheckTreeFile(ObservableCollection<ElementNodeViewModel> Tree, bool b, string s)
        {
            foreach (var Elem in Tree)
            {
                if (Elem.TypeObj == s)
                {
                    Elem.Check = b;
                }
                CheckTreeFile(Elem.ChildNodes, b,s);
            }
        }

       

   

        private void All_Click(object sender, RoutedEventArgs e)
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
            foreach (var tProj in viewModel.TreeStorage)
            {
                tProj.IsExpanded = false;
            }
        }

        private void expand_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var tProj in viewModel.TreeStorage)
            {
                tProj.IsExpanded = true;
            }
        }

        private void folder_Click(object sender, RoutedEventArgs e)
        {
            button3_Click();
            button4_Click();
        }

        
        
    }
}
