using Ascon.Pilot.Theme.Controls;
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
    /// Логика взаимодействия для StageZero.xaml
    /// </summary>
    public partial class StageZero : UserControl
    {
        public StageZero()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewModel s = null;
            s = ((TreeViewModel)DataContext);

            var selectedObject = TreeView.SelectedValue as ElementNodeViewModel;
            if (selectedObject.TypeObj == s.NameTypeProject)
            {
                s.Select = selectedObject;
                s.TitleSelect = selectedObject.DisplayName;
            }
        }

        private void collapse_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var tProj in viewModel.TreeProject)
            {
                tProj.IsExpanded = false;
            }
        }

        private void expand_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TreeViewModel)DataContext;
            foreach (var tProj in viewModel.TreeProject)
            {
                tProj.IsExpanded = true;
            }
        }
    }

}
