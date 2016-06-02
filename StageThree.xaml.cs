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
    /// Логика взаимодействия для StageThree.xaml
    /// </summary>
    public partial class StageThree : UserControl
    {
        //private IDictionary<string, object> attributes;
        private ObservableCollection<TextBox> TextBoxItems = new ObservableCollection<TextBox>();
        private ObservableCollection<TextBlock> TextBlockItems = new ObservableCollection<TextBlock>();
        //private ReadOnlyCollection<IAttribute> AttributeProject;
        private IEnumerable<IAttribute> DiplayAttributeProject;
        private int top = 45;
        public StageThree()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedValue != null)
            {
                var s = ((TreeViewModel)((CreateProject)((PureWindow)((UserControl)((Grid)((TabControl)((TabItem)((UserControl)((Grid)((Button)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Content).DataContext);
                if (s.CreateUpProject(TreeView.SelectedValue as ElementNodeViewModel, getAttributes()))
                ((PureWindow)((UserControl)((Grid)((TabControl)((TabItem)((UserControl)((Grid)((Button)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Close();

            }
        }

        private Dictionary<string,object> getAttributes()
        {
            Dictionary<string, object> attr = new Dictionary<string, object>();
             for (var i = 0; i < TextBlockItems.Count; i++)
             {
                 var _attr = DiplayAttributeProject.First(b => b.Title == TextBlockItems[i].Text);
                 attr.Add(_attr.Name, TextBoxItems[i].Text);                 
             }

             return attr;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedObject = TreeView.SelectedValue as ElementNodeViewModel;

            /* if (AttributeProject == null)
                 AttributeProject = selectedObject.repository.GetType("project").Attributes;*/
            if (DiplayAttributeProject == null)
                DiplayAttributeProject = selectedObject.repository.GetType("project").DisplayAttributes;

            if (selectedObject.TypeObj == "project")
            {
                button1.Content = "Обновить структуру проекта";
                button1.IsEnabled = true;
                CreateAttr(selectedObject, true);
            }
            else if (selectedObject.TypeObj == "projectfolder")
            {
                button1.Content = "Создать проект";
                button1.IsEnabled = true;
                CreateAttr(selectedObject, false);
            }
            AddItemsGrid();
        }

        private void AddItemsGrid()
        {
            grid.Children.Clear();
            for (var i = 0; i < TextBlockItems.Count; i++)
            {
                grid.Children.Add(TextBlockItems[i]);
                grid.Children.Add(TextBoxItems[i]);
            }        
        }

        private void CreateAttr(ElementNodeViewModel selected, bool project)
        {
            if (project)
            {
                var attrItems = selected.Attributes;
                foreach (var attr in attrItems)
                {
                    var _attr = DiplayAttributeProject.First(b => b.Name == attr.Key);
                    // Grid.
                    if (!TextBlockItems.ToList().Exists(n => n.Text == _attr.Title))
                    {
                        var i = (TextBlockItems.Count * 2) / 2;
                        TextBlockItems.Add(new TextBlock() { Margin = new Thickness(10, (top * i) + tb.ActualHeight + 20, 10, 0), Text = _attr.Title, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left });
                        TextBoxItems.Add(new TextBox() { Margin = new Thickness(10, (top * i) + tb.ActualHeight + 20 + 16, 10, 0), Text = attr.Value.ToString(), TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top });
                    }
                    else
                    {
                        var _textBlock = TextBlockItems.First(n => n.Text == _attr.Title);
                        //var _text = TextBoxItems.First(n => n.Text == attr.Value.ToString());
                        var i = TextBlockItems.IndexOf(_textBlock);
                        TextBoxItems[i].Text = attr.Value.ToString();
                    }
                }

                foreach (var _textbox in TextBoxItems)
                {
                    _textbox.IsEnabled = false;
                }
            }
            else if (!project)
            {
                if (TextBoxItems.Count == 0)
                {
                    foreach (var _attr in DiplayAttributeProject)
                    {
                        var i = (TextBlockItems.Count * 2) / 2;
                        TextBlockItems.Add(new TextBlock() { Margin = new Thickness(10, (top * i) + tb.ActualHeight + 20, 0, 0), Text = _attr.Title, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left });
                        TextBoxItems.Add(new TextBox() { Margin = new Thickness(10, (top * i) + tb.ActualHeight + 20 + 16, 0, 0), Text = "", TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top });
                    }
                }

                for (var i = 0; i < TextBoxItems.Count; i++)
                {
                    TextBoxItems[i].Text = "";
                    TextBoxItems[i].IsEnabled = true;
                }
            }            
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            TreeView.Margin = new Thickness(5, tb.ActualHeight + 20, 5, 58);
            
        }
    }
}
