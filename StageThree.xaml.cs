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
        public ComboBox combobox { get; set; }
        public DatePicker Date { get; set; }
        public ButtonEdit BEdit { get; set; }
        public string Key { get; set; }
        public string Attr { get; set; }
        public string name { get; set; }
        public List<string> Items { get; set; }
        public string Config { get; set; }
        public int IndexCb { get; set; }
        public ObservableCollection<ItemCB> _ItemsCB { get; set; }
        public Content()
        {

        }

    }

    public partial class StageThree : UserControl
    {
        private List<Content> content = new List<Content>();
        private IEnumerable<IAttribute> AttributeProject;
        private TreeViewModel s;

        public StageThree()
        {
            InitializeComponent();
        }

        private void AddItemsGrid()
        {
            grid.Children.Clear();
            var attrProject = new List<IAttribute>(AttributeProject.ToList().OrderBy(n => n.DisplaySortOrder));

            for (var ii = 0; ii < attrProject.ToList().Count; ii++)
            {
                /*var x = AttributeProject.ToList().FindIndex(n => n.DisplaySortOrder == ii);
                if (x > -1)
                {*/
                var i = content.FindIndex(n => n.name == attrProject.ToList()[ii].Name);
                if (i > -1)
                {
                    grid.Children.Add(content[i].Title);
                    if (content[i].combobox != null)
                    {
                        content[i].combobox.SelectionChanged += SelectionChanged;
                        content[i].combobox.DropDownOpened += combobox_DropDownOpened;
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
                    //   }
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
                        GetItemsCB(content[i]);
                        content[i].combobox.ItemsSource = ((TreeViewModel)DataContext).CBItems[content[i].IndexCb];
                    }
                }
                else if (content[i].Attr != "" && content[i].Key != "" && content[i]._ItemsCB == null)
                {
                    GetItemsCB(content[i]);
                    content[i].combobox.ItemsSource = ((TreeViewModel)DataContext).CBItems[content[i].IndexCb];
                }
            }
        }


        private void GetItemsCB(Content cont)
        {
            var i = cont.IndexCb;
            ((TreeViewModel)DataContext).GetItemsCB(cont.Attr, cont.Key, ref i);
            cont.IndexCb = i;
            var AttrItems = new List<string>();

            return;
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
                    if (content[i].Items != null)
                    {
                        if (content[i].combobox.SelectedIndex > -1) attrItems.Add(_attr.Name, content[i].Items[content[i].combobox.SelectedIndex]);
                        else attrItems.Add(_attr.Name, "");
                    }
                    else
                        if (content[i]._ItemsCB != null)
                            if (content[i].combobox.SelectedIndex > -1) attrItems.Add(_attr.Name, content[i]._ItemsCB[content[i].combobox.SelectedIndex]);
                            else attrItems.Add(_attr.Name, "");
                        else
                        {
                            if (content[i].combobox.SelectedIndex > -1) attrItems.Add(_attr.Name, ((TreeViewModel)DataContext).CBItems[content[i].IndexCb][content[i].combobox.SelectedIndex].ToString());
                            else attrItems.Add(_attr.Name, "");
                        }
                }
                else
                {
                    var text = ((Run)content[i].Title.Inlines.ToList()[1]).Text;
                    var _attr = AttributeProject.First(b => b.Title == text);
                    if (content[i].Items != null)
                    {
                        if (content[i].combobox.SelectedIndex > -1) attrItems.Add(_attr.Name, content[i].Items[content[i].combobox.SelectedIndex]);
                        else attrItems.Add(_attr.Name, "");
                    }
                    else
                        if (content[i]._ItemsCB != null)
                        {
                            if (content[i].combobox.SelectedIndex > -1) attrItems.Add(_attr.Name, content[i]._ItemsCB[content[i].combobox.SelectedIndex]);
                            else attrItems.Add(_attr.Name, "");
                        }
                        else
                        {
                            if (content[i].combobox.SelectedIndex > -1) attrItems.Add(_attr.Name, ((TreeViewModel)DataContext).CBItems[content[i].IndexCb][content[i].combobox.SelectedIndex].ToString());
                            else attrItems.Add(_attr.Name, "");
                        }
                }
            }
            s.AttributesNewProject = attrItems;
            if (s.AllObligatoryAttr(s.AttributesNewProject))
            {
                s.getAllAttributes = true;
            }
            else s.getAllAttributes = false;

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

        }



        private void CreateAttr(ElementNodeViewModel selected, bool project)
        {
            if (!project)
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
                                content.Add(new Content() { Title = new TextBlock() { Margin = new Thickness(0, 0, 0, 0), Foreground = Foreground, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left } });
                                content[content.Count - 1].Title.Inlines.Add(new Run("* ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red")) });
                                content[content.Count - 1].Title.Inlines.Add(new Run(_attr.Title));
                            }
                            content[content.Count - 1].name = _attr.Name;
                            content[content.Count - 1].IndexCb = -1;

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
                                        if (config.IndexOf("IsEditable=\"True\"") > -1) content[content.Count - 1].combobox.IsEditable = true;
                                    }
                                    if (config.IndexOf("Enum") > -1)
                                    {
                                        content[content.Count - 1].Items = GetItem(config);
                                        content[content.Count - 1].combobox = new ComboBox() { Margin = new Thickness(0, 0, 0, 10) };
                                    }
                                    else if (config.IndexOf("Dialog") > -1)
                                    {
                                        content[content.Count - 1].Key = GetConfig(config, "Source");
                                        content[content.Count - 1].Attr = GetConfig(config, "StringFormat");
                                        content[content.Count - 1].button = new Button() { Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Right, Content = new Image() { Height = 16, Width = 16, Source = Ascon.Pilot.Theme.Icons.Instance.FolderIcon } };
                                        content[content.Count - 1].textBox = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch };
                                    }
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
                                content[content.Count - 1].textBox = new TextBox() { Margin = new System.Windows.Thickness(0, 0, 0, 10), Text = "" };
                                content[content.Count - 1].textBox.IsEnabled = false;

                                var s = _attr.Configuration.Remove(0, _attr.Configuration.IndexOf("<FormatString>") + 14);
                                string name = s.Remove(s.IndexOf("</FormatString>"));
                                content[content.Count - 1].textBox.Text = name;
                                var attrItems = new Dictionary<string, object>();
                                attrItems.Add(_attr.Name, name);
                                ((TreeViewModel)DataContext).AttributesNewProject = attrItems;
                                if (((TreeViewModel)DataContext).AllObligatoryAttr(((TreeViewModel)DataContext).AttributesNewProject))
                                {
                                    ((TreeViewModel)DataContext).getAllAttributes = true;
                                }
                                else ((TreeViewModel)DataContext).getAllAttributes = false;
                            }
                            else if (_attr.Type.ToString() == "Integer")
                            {
                                content[content.Count - 1].BEdit = new Ascon.Pilot.Theme.Controls.ButtonEdit() { Margin = new Thickness(0, 0, 0, 10) };

                            }
                        }
                    }
                }
            }
        }

        private List<string> GetItem(string config)
        {
            var items = new List<string>();

            var i = config.IndexOf("<Item>");
            while (i > -1)
            {
                config = config.Remove(0, i + 6);
                i = config.IndexOf("</Item>");
                items.Add(config.Remove(i));
                i = config.IndexOf("<Item>");
            }
            return items;
        }

        private string GetConfig(string config, string pName)
        {
            if (config.IndexOf(pName) >= 0)
            {
                var _config = config.Remove(0, config.IndexOf(pName));
                var i = _config.IndexOf("\"");
                _config = _config.Remove(0, i + 1);
                i = _config.IndexOf("\"");
                _config = _config.Remove(i);
                return _config;
            }

            if (config.IndexOf("Attr Kind=\"OrgUnit\"") >= 0) { return "OrgUnit"; }
            return "";
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
            foreach (var itemCatalog in _itemsCatalog)
            {
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
                }
                AttrItems += attr;
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
                if (content[i].Key == "OrgUnit")
                {
                   content[i].textBox.Text = ((TreeViewModel)DataContext).openDialog();                    
                }
                else
                {
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
                                        content[z].combobox.Text = ResultCatalog(objItems, new List<string>() { a.Key.ToString() });
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
                }
                return;
            }
        }
    }
}
