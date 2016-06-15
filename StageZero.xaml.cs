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
            var s1 = ((PureWindow)((UserControl)((Grid)((TabControl)((TabItem)((UserControl)((Grid)((TreeView)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent);
            TreeViewModel s = null;

            if (s1.Content is CreateProject) s = ((TreeViewModel)((CreateProject)s1.Content).DataContext);
            else if (s1.Content is CreateProjectStructure) s = ((TreeViewModel)((CreateProjectStructure)s1.Content).DataContext);

            var selectedObject = TreeView.SelectedValue as ElementNodeViewModel;//+++
            if (selectedObject.TypeObj == s.NameTypeProject)
            {
                

                s.Select = selectedObject;
                s.TitleSelect = selectedObject.DisplayName;
              /*  foreach (var obj in ((Grid)((TabControl)((TabItem)((UserControl)((Grid)((TreeView)sender).Parent).Parent).Parent).Parent).Parent).Children)
                {
                    if (obj.GetType().Name == "TextBlock")
                        if (((TextBlock)obj).Name == "textBlock2")
                        {
                            ((TextBlock)obj).Text = selectedObject.DisplayName;
                        }
                        else if (((TextBlock)obj).Name == "textBlock")
                        {
                            ((TextBlock)obj).Visibility = Visibility.Visible;
                        }
                }*/
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
