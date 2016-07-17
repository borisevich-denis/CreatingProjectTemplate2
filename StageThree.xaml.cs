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
    /// 

    class Content
    {
        public TextBox textBox { get; set; }
        public TextBlock Title { get; set; }
        public Button button { get; set; }
       // public TextBox TextBox { get; set; }
        public ComboBox combobox { get; set; }
        public DatePicker Date { get; set; }
        public ButtonEdit BEdit { get; set; }
        public string Key { get; set; }
        public string Attr { get; set; }
        public string name { get; set; }
        public List<string> Items { get; set; }
        public string Config { get; set; }
        public ObservableCollection<ItemCB> _ItemsCB { get; set; }
       // public bool IsEditable { get; set; }
        public Content()
        {

        }

    }

    public partial class StageThree : UserControl
    {
        private List<Content> content = new List<Content>();
        //private IDictionary<string, object> attributes;
     //   private ObservableCollection<TextBox> TextBoxItems = new ObservableCollection<TextBox>();
     //   private ObservableCollection<TextBlock> TextBlockItems = new ObservableCollection<TextBlock>();
        //private ReadOnlyCollection<IAttribute> AttributeProject;
        private IEnumerable<IAttribute> AttributeProject;       
        private TreeViewModel s;       

        public StageThree()
        {
            InitializeComponent();
            Loaded += StageThree_Loaded;
        }

        void StageThree_Loaded(object sender, RoutedEventArgs e)
        {
           /* if (DiplayAttributeProject == null) 
            DiplayAttributeProject = ((TreeViewModel)DataContext).DiplayAttributeProject;
            if (AllObligatoryAttr(((TreeViewModel)DataContext).AttributesNewProject))
            {
                ((TreeViewModel)DataContext).getAllAttributes = true;
            }
            else ((TreeViewModel)DataContext).getAllAttributes = false;*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        /*    if (TreeView.SelectedValue != null)
            {
                var s = ((TreeViewModel)((CreateProject)((PureWindow)((UserControl)((Grid)((TabControl)((TabItem)((UserControl)((Grid)((Button)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Content).DataContext);
                if (s.CreateUpProject(TreeView.SelectedValue as ElementNodeViewModel, getAttributes()))
                ((PureWindow)((UserControl)((Grid)((TabControl)((TabItem)((UserControl)((Grid)((Button)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Close();

            }*/
        }

        private Dictionary<string,object> getAttributes()
        {
            Dictionary<string, object> attr = new Dictionary<string, object>();
            for (var i = 0; i < content.Count; i++)
             {
                 if (content[i].combobox != null)
                 {
                     var _attr = AttributeProject.First(b => b.Title == content[i].combobox.Text);
                     attr.Add(_attr.Name, content[i].combobox.Text);
                 }
                 else if (content[i].textBox != null)
                 {
                     var _attr = AttributeProject.First(b => b.Title == content[i].textBox.Text);
                     attr.Add(_attr.Name, content[i].textBox.Text);
                 }
             }

             return attr;
        }

      /*  private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            /*var selectedObject = TreeView.SelectedValue as ElementNodeViewModel;

            /* if (AttributeProject == null)
                 AttributeProject = selectedObject.repository.GetType("project").Attributes;*/
          /*  if (DiplayAttributeProject == null)
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
        }*/
        
        private void AddItemsGrid()
        {            
            grid.Children.Clear();



            for (var ii = 1; ii < AttributeProject.ToList().Count + 1; ii++)
            {
                var x = AttributeProject.ToList().FindIndex(n => n.DisplaySortOrder == ii);
                if (x > -1)
                {
                    var i = content.FindIndex(n => n.name == AttributeProject.ToList()[x].Name);
                    if (i > -1)
                    {
                        grid.Children.Add(content[i].Title);
                        if (content[i].combobox != null)
                        {
                            content[i].combobox.SelectionChanged += SelectionChanged;
                          //  content[i].combobox.Loaded += combobox_Loaded;
                            content[i].combobox.DropDownOpened += combobox_DropDownOpened;
                            content[i].combobox.DropDownClosed += combobox_DropDownClosed;

                            grid.Children.Add(content[i].combobox);
                        }
                        else if (content[i].button != null)
                        {
                            content[i].textBox.TextChanged += TextChanged;
                            content[i].button.Click += OpenDialog;
                            var st = new Grid() { Margin = new Thickness(0, 0, 0, 10) };
                            st.Children.Add(content[i].textBox);
                            st.Children.Add(content[i].button);
                            grid.Children.Add(st);

                        }
                        else if (content[i].textBox != null)
                        {
                            content[i].textBox.TextChanged += TextChanged;
                            grid.Children.Add(content[i].textBox);
                        }
                        else if (content[i].BEdit != null)
                        {
                            grid.Children.Add(content[i].BEdit);
                        }
                        else if (content[i].Date != null)
                        {
                            grid.Children.Add(content[i].Date);
                        }
                    }
                }
            }
        }

        void combobox_DropDownClosed(object sender, EventArgs e)
        {
            var attrItems = new Dictionary<string, object>();
            if (content.ToList().Exists(n => n.combobox == sender))
            {
                var i = content.ToList().FindIndex(n => n.combobox == sender);
                if (content[i].Items == null && content[i]._ItemsCB == null)
                {
                    var _index = content[i].combobox.SelectedIndex;                  
                    content[i]._ItemsCB = new ObservableCollection<ItemCB>(((TreeViewModel)DataContext).CBItems);
                    content[i].combobox.ItemsSource = content[i]._ItemsCB;
                    content[i].combobox.SelectedIndex = _index;
                }
            }
        }

        void combobox_DropDownOpened(object sender, EventArgs e)
        {
            var attrItems = new Dictionary<string, object>();
            if (content.ToList().Exists(n => n.combobox == sender))
            {

                var i = content.ToList().FindIndex(n => n.combobox == sender);
                if (content[i].Items != null)
                {
                    if (content[i].Items.Count > 0)
                    {
                        content[i].combobox.ItemsSource = content[i].Items;
                    }
                    else if (content[i].Attr != "" && content[i].Key != "")
                    {
                        GetItemsCB(content[i].Attr, content[i].Key);
                        // var _items = new List<string>();
                        // foreach (var item in items) { _items.Add(item.DispName); }
                        content[i].combobox.ItemsSource = ((TreeViewModel)DataContext).CBItems;
                    }
                }
                else if (content[i].Attr != "" && content[i].Key != "" && content[i]._ItemsCB == null)
                {
                    GetItemsCB(content[i].Attr, content[i].Key);
                    // var _items = new List<string>();
                    // foreach (var item in items) { _items.Add(item.DispName); }
                    content[i].combobox.ItemsSource = ((TreeViewModel)DataContext).CBItems;
                }
                else if (content[i]._ItemsCB != null)
                {
                    content[i].combobox.ItemsSource = content[i]._ItemsCB;
                }
            }
        }

       

   /*    */

       /* private void combobox_Loaded(object sender, RoutedEventArgs e)
        {
             var attrItems = new Dictionary<string, object>();
             if (content.ToList().Exists(n => n.combobox == sender))
             {
                 
                 var i = content.ToList().FindIndex(n => n.combobox == sender);
                 if (content[i].Items != null)
                 {
                     if (content[i].Items.Count > 0)
                     {
                         content[i].combobox.ItemsSource = content[i].Items;
                     }
                     else if (content[i].Attr != "" && content[i].Key != "")
                     {
                         var items = GetItemsCB(content[i].Attr, content[i].Key);
                         // var _items = new List<string>();
                         // foreach (var item in items) { _items.Add(item.DispName); }
                         content[i].combobox.ItemsSource =  ((TreeViewModel)DataContext).CBItems;
                     }
                 } else if (content[i].Attr != "" && content[i].Key != "")
                 {
                     var items = GetItemsCB(content[i].Attr, content[i].Key);
                     // var _items = new List<string>();
                     // foreach (var item in items) { _items.Add(item.DispName); }
                     content[i].combobox.ItemsSource = ((TreeViewModel)DataContext).CBItems;
                 }
                 
             }
        }*/

        private List<string> GetItemsCB(string attr, string key)
        {
            var items = ((TreeViewModel)DataContext).GetItemsCB(attr, key);
            var AttrItems = new List<string>();
           if (items != null)
           foreach (var item in items)
           {
               var _attr = "";
                   foreach (var atr in item.attr)
                   {

                       if (_attr.Length > 0)
                       {
                           _attr += " - ";
                       }
                       _attr += atr.Value;
                   }
                   AttrItems.Add(_attr);
           }

           return AttrItems;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var attrItems = new Dictionary<string, object>();
            if (content.ToList().Exists(n => n.combobox == sender))
            {
                var i = content.ToList().FindIndex(n => n.combobox == sender);               

                var index = content[i].Title.Inlines.ToList().Exists(n => ((Run)n).Text == "* ");
                if (!index)
                {
                    var _attr = AttributeProject.First(b => b.Title == ((Run)content[i].Title.Inlines.ToList()[0]).Text);
                    attrItems.Add(_attr.Name, content[i].combobox.Text);
                }
                else
                {
                    var text = ((Run)content[i].Title.Inlines.ToList()[1]).Text;
                    var _attr = AttributeProject.First(b => b.Title == text);
                    attrItems.Add(_attr.Name, content[i].combobox.Text);
                }
                if (content[i].Items == null && content[i]._ItemsCB==null)
                {
                    var _index = content[i].combobox.SelectedIndex;// Text;
                    //  content[i].combobox.ItemsSource = null;
                    content[i]._ItemsCB = new ObservableCollection<ItemCB>(((TreeViewModel)DataContext).CBItems);
                 //   foreach (var _item in ((TreeViewModel)DataContext).CBItems) { content[i].Items.Add(_item.DispName); }
                    content[i].combobox.ItemsSource = content[i]._ItemsCB;
                    content[i].combobox.SelectedIndex = _index;
                }
            }
            s.AttributesNewProject = attrItems;
            if (s.AllObligatoryAttr(s.AttributesNewProject))
            {
                s.getAllAttributes = true;
            } else s.getAllAttributes = false;
           
        }
                

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            var attrItems = new Dictionary<string, object>();
            if (content.ToList().Exists(n => n.textBox == sender))
            {
                var i = content.ToList().FindIndex(n => n.textBox == sender);
                var index = content[i].Title.Inlines.ToList().Exists(n => ((Run)n).Text == "* ");
                if (!index)
                {
                    var _attr = AttributeProject.First(b => b.Title == ((Run)content[i].Title.Inlines.ToList()[0]).Text);
                    attrItems.Add(_attr.Name, content[i].textBox.Text);
                }
                else
                {
                    var text = ((Run)content[i].Title.Inlines.ToList()[1]).Text;
                    var _attr = AttributeProject.First(b => b.Title == text);
                    attrItems.Add(_attr.Name, content[i].textBox.Text);
                }
                                
            }
        
            s.AttributesNewProject = attrItems;
            if (s.AllObligatoryAttr(s.AttributesNewProject))
            {
                s.getAllAttributes = true;
            }
            else s.getAllAttributes = false;
            //проверка на заполненые атрибуты 
        }

       

        private void CreateAttr(ElementNodeViewModel selected, bool project)
        {
            /* if (project)
              {
                  var attrItems = selected.Attributes;
                  foreach (var attr in attrItems)
                  {
                      var _attr = DiplayAttributeProject.First(b => b.Name == attr.Key);
                      // Grid.
                    //  _attr.IsService
                      if (!TextBlockItems.ToList().Exists(n => n.Text == _attr.Title))
                      {
                          var i = (TextBlockItems.Count * 2) / 2;
                          if (!_attr.IsObligatory)
                          {
                              TextBlockItems.Add(new TextBlock() { Margin = new Thickness(10, (top * i) + 20, 10, 0), Text = _attr.Title, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left });
                          }
                          else
                          {

                              TextBlockItems.Add(new TextBlock() { Margin = new Thickness(10, (top * i) + 20, 10, 0),  TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left });
                              TextBlockItems[TextBlockItems.Count - 1].Inlines.Add(new Run("* ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red")) });
                              TextBlockItems[TextBlockItems.Count - 1].Inlines.Add(new Run(_attr.Title));
                          
                          }
                              TextBoxItems.Add(new TextBox() { Margin = new Thickness(10, (top * i) + 20 + 16, 10, 0), Text = attr.Value.ToString(), TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top });
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
             else */if (!project)
             {
                 if (content.Count == 0)
                 {
                     foreach (var _attr in AttributeProject)
                     {

                         if (!_attr.IsService)
                         {
                             if (!_attr.IsObligatory)
                             {
                                 content.Add(new Content() { Title = new TextBlock() { Margin = new Thickness(0, 0, 0, 0), Text = _attr.Title, Foreground = Foreground, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left } });
                             }
                             else
                             {
                                 content.Add(new Content() { Title = new TextBlock() { Margin = new Thickness(0, 0, 0, 0), /*Text = new Run("* ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red")) }, run /*+ _attr.Title),*/ Foreground = Foreground, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left } });
                                 content[content.Count - 1].Title.Inlines.Add(new Run("* ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red")) });
                                 content[content.Count - 1].Title.Inlines.Add(new Run(_attr.Title));
                             }
                             content[content.Count - 1].name = _attr.Name;

                             /*  {
                                   var s = _attr.Configuration;
                                   var s2 = _attr.DisplayHeight;
                                   var s3 = _attr.DisplaySortOrder;
                                   var s4 = _attr.IsObligatory;
                                   var s5 = _attr.IsService;
                                   var s6 = _attr.ShowInObjectsExplorer;
                                   var s7 = _attr.GetType();
                                      __type : Numerator String DateTime"Double""Decimal"Integer

                               }*/
                             var __type = _attr.Type.ToString();
                             content[content.Count - 1].Config = _attr.Configuration;
                             if (_attr.Type.ToString() == "String")
                             {
                                 if (_attr.Configuration == "" || _attr.Configuration == null)
                                 {
                                     content[content.Count - 1].textBox = new TextBox() { Margin = new Thickness(0, 0, 0, 10), Text = "", TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top };
                                 }
                                 else
                                 {
                                     var config = _attr.Configuration;
                                     var i = _attr.Configuration.IndexOf("<!-- ");
                                     if (i > -1) { config = _attr.Configuration.Remove(i); }

                                     if (config.IndexOf("ComboBox") > -1)
                                     {
                                         content[content.Count - 1].Key = GetConfig(config, "Source");
                                         content[content.Count - 1].Attr = GetConfig(config, "StringFormat");
                                         content[content.Count - 1].combobox = new ComboBox() { Margin = new Thickness(0, 0, 0, 10), IsSynchronizedWithCurrentItem = true };
                                     }
                                     if (config.IndexOf("Enum") > -1)
                                     {
                                         content[content.Count - 1].Items = GetItem(config);
                                         /* if (config.IndexOf("IsEditable = \"True\"") > -1)
                                           content[content.Count - 1].IsEditable = true;
                                           else content[content.Count - 1].IsEditable = false;*/
                                         content[content.Count - 1].combobox = new ComboBox() { Margin = new Thickness(0, 0, 0, 10) };
                                     }
                                     else if (config.IndexOf("Dialog") > -1)
                                     {
                                         content[content.Count - 1].Key = GetConfig(config, "Source");
                                         content[content.Count - 1].Attr = GetConfig(config, "StringFormat");
                                         //  StringFormat="{
                                         //content[content.Count - 1].textBox = new TextBox() { Margin = new Thickness(0, 0, 0, 0), Text = "", TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top };
                                         //   var _style =  new Ascon.Pilot.Theme.StyleKeyExtension(Ascon.Pilot.Theme.StyleNames.ImageButtonStyle);

                                         content[content.Count - 1].button = new Button() { Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Right, Content = new Image() { Height = 16, Width = 16, Source = Ascon.Pilot.Theme.Icons.Instance.FolderIcon } };
                                         content[content.Count - 1].textBox = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch };
                                     }//"{DynamicResource {theme:StyleKey ImageButtonStyle}}"
                                     else
                                     {
                                         content[content.Count - 1].textBox = new TextBox() { Margin = new Thickness(0, 0, 0, 10), Text = "", TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top };

                                     }
                                 }
                             }
                             else if (_attr.Type.ToString() == "DateTime")
                             {
                                 content[content.Count - 1].Date = new DatePicker() { Margin = new Thickness(0, 0, 0, 10) };
                             }
                             else if (_attr.Type.ToString() == "Double")
                             {
                                 content[content.Count - 1].textBox = new TextBox() { Margin = new Thickness(0, 0, 0, 10), Text = "" };
                             }
                             else if (_attr.Type.ToString() == "Decimal")
                             {
                                 content[content.Count - 1].textBox = new TextBox() { Margin = new Thickness(0, 0, 0, 10), Text = "" };
                             }
                             else if (_attr.Type.ToString() == "Numerator")
                             {
                                 content[content.Count - 1].textBox = new TextBox() { Margin = new Thickness(0, 0, 0, 10), Text = "" };
                             }
                             else if (_attr.Type.ToString() == "Integer")
                             {
                                 content[content.Count - 1].BEdit = new Ascon.Pilot.Theme.Controls.ButtonEdit() { Margin = new Thickness(0, 0, 0, 10) };

                             }
                         }
                     }
                 }

               /*  for (var i = 0; i < content.Count; i++)
                 {
                     content[i].textBox.Text = "";
                     content[i].IsEnabled = true;
                 }*/
             }
        }

      /*  private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            //var selectedObject = TreeView.SelectedValue as ElementNodeViewModel;

             /*if (AttributeProject == null)
                 AttributeProject = selectedObject.repository.GetType("project").Attributes;*/

           
     //   }

        private List<string> GetItem(string config)
        {
            var items = new List<string>();

            var i = config.IndexOf("<Item>");
            while (i>-1)
            {
                config = config.Remove(0,i+6);
                i = config.IndexOf("</Item>");
                items.Add(config.Remove(i));
                i = config.IndexOf("<Item>");
            }
            return items;
        }

        private string GetConfig(string config, string pName)
        {
            var _config = config.Remove(0, config.IndexOf(pName));
            var i = _config.IndexOf("\"");
            _config = _config.Remove(0, i+1);
            i = _config.IndexOf("\"");
            _config = _config.Remove(i);
            return _config;
        }
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (content.Count == 0)
            {
                s = ((TreeViewModel)DataContext);
                if (AttributeProject == null)
                    AttributeProject = s.AttributeProject;
                
                CreateAttr(null, false);

                AddItemsGrid();
            }
        }

        private string ResultCatalog(IEnumerable<IDataObject> _itemsCatalog, List<string> _attr)
        {
            var AttrItems = "";
           // var s = attrToList(_attr);
            foreach (var itemCatalog in _itemsCatalog)
            {
                //if (itemCatalog.isCheck)
               // {
                    if (AttrItems.Length > 0)
                    {
                        AttrItems += "; ";
                    }
                    var attr = "";
                
                    foreach (var atr in itemCatalog.Attributes)
                    {
                        if (_attr.Exists(x => x == atr.Key))
                        {
                            if (attr.Length > 0)
                            {
                                attr += " - ";
                            }
                            attr += atr.Value;
                        }
                    } AttrItems += attr;
                //}
            }
            return AttrItems;
        }


        private List<string> attrToList(string attr)
        {
            var _attr = new List<string>();
            var i = attr.IndexOf("{");
            while (i > -1)
            {
                attr = attr.Remove(0, i + 1);
                i = attr.IndexOf("}");
                _attr.Add(attr.Remove(i));
                i = attr.IndexOf("{");
            }
            return _attr;
        }

        public void OpenDialog(object sender, RoutedEventArgs e)
        {
            if (content.ToList().Exists(n => n.button == sender))
            {
                var i = content.ToList().FindIndex(n => n.button == sender);
              //  var items = GetItemsCB(content[i].Attr, content[i].Key);
                var objItems = ((TreeViewModel)DataContext).openDialog(content[i].Config);
                if (objItems.ToList().Count > 0)
                {
                    content[i].textBox.Text = ResultCatalog(objItems, attrToList(content[i].Attr));
                    foreach (var a in objItems.ToList()[0].Attributes)
                    {
                        if (content.Exists(x => x.name == a.Key))
                        {
                            var z = content.FindIndex(x => x.name == a.Key);
                            if (i != z)
                            {
                                if (content[z].combobox != null)
                                {
                                    content[z].combobox.Text = ResultCatalog(objItems, new List<string>() {a.Key.ToString()});                                   
                                }                                
                                else if (content[z].textBox != null)
                                {
                                    content[z].textBox.Text = ResultCatalog(objItems, new List<string>() { a.Key.ToString() });
                                }
                                else if (content[z].BEdit != null)
                                {
                                    content[z].BEdit.Text = ResultCatalog(objItems, new List<string>() { a.Key.ToString() });
                                }                              
                            }
                        }
                    }
                }
                /* string atr = "";
                 foreach (var item in ItemsAttr)
                 {
                     var i = item.IndexOf(ItemContent.name);
                     if (i > 0)
                     {
                         atr += item.Remove(i, item.IndexOf("=")) + "; ";
                     }
                 }*/
                /*     if (ItemsAttr.Length > 0)
                     {
                         if (ItemsAttr.IndexOf(" - ; ") > -1)
                             ItemContent.TextBox.Text = ItemsAttr.Remove(ItemsAttr.Length - 5);
                         else 
                         ItemContent.TextBox.Text = ItemsAttr.Remove(ItemsAttr.Length - 3);
                     }*/
           /*     var attrItems = new Dictionary<string, object>();

                var index = content[i].Title.Inlines.ToList().Exists(n => ((Run)n).Text == "* ");
                if (!index)
                {
                    var _attr = DiplayAttributeProject.First(b => b.Title == ((Run)content[i].Title.Inlines.ToList()[0]).Text);
                    attrItems.Add(_attr.Name, content[i].TextBox.Text);
                }
                else
                {
                    var text = ((Run)content[i].Title.Inlines.ToList()[1]).Text;
                    var _attr = DiplayAttributeProject.First(b => b.Title == text);
                    attrItems.Add(_attr.Name, content[i].TextBox.Text);
                }



                s.AttributesNewProject = attrItems;
                if (s.AllObligatoryAttr(s.AttributesNewProject))
                {
                    s.getAllAttributes = true;
                }
                else s.getAllAttributes = false;
                */


                return;
            }


        }
    }
}
