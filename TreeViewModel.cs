using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ascon.Pilot.SDK;
using Ascon.Pilot.Theme.Controls;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Interop;
//using System.Windows;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    class TreeViewModel : PropertyChangedBase, IObserver<IDataObject>, IDisposable
    {
        private readonly IObjectsRepository _repository;
        private readonly IObjectModifier _modifier;
        private readonly IFileProvider _fileProvider;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly IPersonalSettings _personalSettings;
        private ObjectLoader loader;
        private readonly IPilotDialogService _pilotDialogService;

        private DataObjectWrapper ParentObjectOpenDialog;
        private List<string> attributesOpenDialog;
        private Guid gParentObjectOpenDialog;
        private string _NameCatalog;

        private string nameTypeProjectFolder;
        private string nameTypeProject;

        //  private readonly Guid _rootGuid;
        private readonly DelegateCommand _search;

        private readonly ObservableCollection<ElementNodeViewModel> _TreeObj;
        private readonly ObservableCollection<ElementNodeViewModel> _TreeStorage;
        private readonly ObservableCollection<ElementNodeViewModel> _TreeProject;

        private readonly ObservableCollection<ElementNodeViewModel> _selectTreeObj;
        private readonly ObservableCollection<ElementNodeViewModel> _selectTreeStorage;

        private  ObservableCollection<ElementCatalogModel> _itemsCatalog;
        private readonly ObservableCollection<ElementCatalogModel> _searchItemsCatalog;
        private bool _SelectElementCatalogModel;
        private readonly ObservableCollection<Result> _resultCreation;
        private Catalog opDialog;

        private bool _CreateCompleted = false;
        private Visibility _visability = Visibility.Visible;
        private string _NameTitleToCreateProject;
        private string _processStatus;

        //private string _resultOpenDialog;
        private bool _getAllAttributes;

        private DataObjectWrapper _baseElement;
        private ElementNodeViewModel selectSample = null;
        // private IDataObject _baseElement;
        private IDataObject _parent;
        private readonly IPerson _currentPerson;
        private bool modifier = false;
        //private ElementNodeViewModel _selectedTree = null;
        private string _TitleSelect = "не выбран";

        private Guid gCBParent;
        private DataObjectWrapper CBParent;
        private ObservableCollection<ItemCB> _CBItems;
        private string CBAttr;
        private int _indexCreate;
        private int _countCreate;

        private Dictionary<string, object> attributes = new Dictionary<string, object>();
        //private GetDataObj getDObj;
        public TreeViewModel(IPilotDialogService pilotDialogService, ITabServiceProvider tabServiceProvider, IObjectsRepository repository, IDataObject selection, IObjectModifier modifier, IFileProvider fileProvider, IPersonalSettings personalSettings, string _nameTypeProjectFolder, string _nameTypeProject)
        {
            nameTypeProjectFolder = _nameTypeProjectFolder;
            nameTypeProject = _nameTypeProject;
            _itemsCatalog = new ObservableCollection<ElementCatalogModel>();
            _searchItemsCatalog = new ObservableCollection<ElementCatalogModel>();

            _TreeObj = new ObservableCollection<ElementNodeViewModel>();
            _TreeProject = new ObservableCollection<ElementNodeViewModel>();
            _TreeStorage = new ObservableCollection<ElementNodeViewModel>();
            _selectTreeObj = new ObservableCollection<ElementNodeViewModel>();
            _selectTreeStorage = new ObservableCollection<ElementNodeViewModel>();
            _resultCreation = new ObservableCollection<Result>();
            _CBItems = new ObservableCollection<ItemCB>();
            _personalSettings = personalSettings;
            _pilotDialogService = pilotDialogService;
            _repository = repository;
            _tabServiceProvider = tabServiceProvider;
            _modifier = modifier;
            _fileProvider = fileProvider;
            // _rootGuid = new Guid("00000001-0001-0001-0001-000000000001");
            _parent = selection;
            CreateHidden = false;
            //_repository.SubscribeObjects(new[] { select.Id }).Subscribe(this);
            _currentPerson = repository.GetCurrentPerson();
            CopyAccessObj = true;
            CopyAccessStorage = true;         
            _search = new DelegateCommand(search);
            //getDObj = new GetDataObj(_repository);
            //  itemcb = new List<ItemCB>() { new ItemCB() { Check = true, name = "Выбрать все" }, new ItemCB() { Check = false, name = "Очистить все" } };
            //  itemcb2 = new List<ItemCB>() { new ItemCB() { Check = true, name = "Выбрать все" }, new ItemCB() { Check = false, name = "Очистить все" }, new ItemCB() { Check = null, name = "Только папки"} };
            loader = new ObjectLoader(_repository);
            loader.Load(SystemObjectIds.RootObjectId, o =>
            {
                _baseElement = new DataObjectWrapper(o, repository);

                _repository.SubscribeObjects(new[] { _baseElement.Id }).Subscribe(this);
            });
        }

        /*  public List<ItemCB> itemcb { get; set; }
          public List<ItemCB> itemcb2 { get; set; }*/
        public bool getAllAttributes
        {
            get { return _getAllAttributes; }
            set
            {
                _getAllAttributes = value;
                NotifyPropertyChanged("getAllAttributes");
            }
        }
        public bool CreateCompleted
        {
            get { return _CreateCompleted; }
            set
            {
                _CreateCompleted = value;
                if (value == true)
                {
                    visability = Visibility.Collapsed;
                }
                NotifyPropertyChanged("CreateCompleted");
            }
        }

        public int indexCreate
        {
            get { return _indexCreate; }
            set
            {
                if (_indexCreate < _countCreate)
                {
                    _indexCreate = value;
                    NotifyPropertyChanged("indexCreate");
                }
            }
        }
        public Visibility visability
        {
            get { return _visability; }
            set
            {
                _visability = value;
                NotifyPropertyChanged("visability");
            }

        }
        public int countCreate
        {
            get { return _countCreate; }
            set
            {
                _countCreate = value;
                NotifyPropertyChanged("countCreate");
            }
        }

        public string NameTitleToCreateProject
        {
            get { return _NameTitleToCreateProject; }
            set
            {
                _NameTitleToCreateProject = value;
                NotifyPropertyChanged("NameTitleToCreateProject");
            }
        }

        public string processStatus
        {
            get { return _processStatus; }
            set
            {
                _processStatus = value;
                NotifyPropertyChanged("processStatus");
            }
        }

        public string NameTypeProject 
        {
            get 
            {
                return nameTypeProject;
            }
        }

        public bool CreateHidden { get; set; }
        public ObservableCollection<Result> resultCreation
        {
            get
            {
                return _resultCreation;
            }
        }

        public string TitleSelect
        {
            get { return _TitleSelect; }
            set
            {
                _TitleSelect = value;
                NotifyPropertyChanged("TitleSelect");
            }
        }
        public Dictionary<string, object> AttributesNewProject
        {
            get
            {
                return attributes;
            }
            set
            {
                foreach (var attr in value)
                {
                    if (attributes.ToList().Exists(n => n.Key == attr.Key))
                    {
                        if (attr.Value.ToString() == "")
                        {
                            attributes.Remove(attr.Key);
                            return;
                        }
                        attributes[attr.Key] = attr.Value;
                    }
                    else
                    {
                        attributes.Add(attr.Key, attr.Value);
                    }

                }
            }
        }

        public IEnumerable<IAttribute> AttributeProject
        {
            get
            {
                return _repository.GetType(nameTypeProject).Attributes;
            }
        }

        public bool CopyAccessObj
        {
            get;
            set;
        }

        public bool CopyAccessStorage
        {
            get;
            set;
        }

        public ObservableCollection<ElementNodeViewModel> TreeObj
        {
            get { return _TreeObj; }
        }
        public ObservableCollection<ElementNodeViewModel> TreeStorage
        {
            get { return _TreeStorage; }
        }
        public ObservableCollection<ElementNodeViewModel> TreeProject
        {
            get { return _TreeProject; }
        }

        public bool AllObligatoryAttr(Dictionary<string, object> attributes)
        {
            foreach (var attr in AttributeProject)
            {
                if (attr.IsObligatory)
                {
                    if (!attributes.ToList().Exists(n => n.Key == attr.Name))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public ElementNodeViewModel Select
        {
            get
            {
                return selectSample;
            }
            set
            {
                if (!getAllAttributes) { getAllAttributes = true; }

                if (selectSample == null)
                {
                    selectSample = value;
                    //проверка или добавление в список выгруженных
                    new SettingsViewModel(_personalSettings, value.Id.ToString());
                    _repository.SubscribeObjects(selectSample.Source.Children).Subscribe(this);
                    return;
                }
                if (selectSample.Id == value.Id)
                {
                    

                    IDataObject obj = null;

                    loader.Load(value.Source.Id, o => { obj = o; });
                    selectSample.Update(obj);
                    return;
                }
                else
                {
                    selectSample = value;
                    //проверка или добавление в список выгруженных
                    new SettingsViewModel(_personalSettings, value.Id.ToString());
                    _TreeObj.Clear();
                    _TreeStorage.Clear();
                    _repository.SubscribeObjects(selectSample.Source.Children).Subscribe(this);
                }
            }
        }
        public void OnNext(IDataObject value)
        {
            if (gParentObjectOpenDialog == value.Id)
            {
                NameCatalog = value.DisplayName;
                ParentObjectOpenDialog =  new DataObjectWrapper(value,_repository);
                _repository.SubscribeObjects(value.Children).Subscribe(this);

            }

            if (ParentObjectOpenDialog != null)
                if (ParentObjectOpenDialog.Children.ToList().Exists(n => n == value.Id))
                {

                    if (value.Children.Count == 0)
                    {
                        var attrItem = new Dictionary<string, string>();
                        foreach (var atr in attributesOpenDialog)
                        {
                            if (value.Attributes.ToList().Exists(n => n.Key == atr))
                            {
                                var obj = value.Attributes.ToList().Find(n => n.Key == atr);
                                attrItem.Add(obj.Key, obj.Value.ToString());
                            }
                        }
                        if (_itemsCatalog.ToList().Exists(n => n.Id == value.Id))
                        {
                            var itemC = _itemsCatalog.First(n => n.Id == value.Id);
                            itemC.Update(value.DisplayName);
                        }
                        else
                        {

                            _itemsCatalog.Insert(_itemsCatalog.Count - 1, new ElementCatalogModel(Ascon.Pilot.Theme.Icons.Instance.FileIcon, value.DisplayName, value.Id, attrItem, this));
                            _itemsCatalog = new ObservableCollection<ElementCatalogModel>(_itemsCatalog.OrderBy(n => n.Sort));
                            NotifyPropertyChanged("ItemsCatalog");
                        }
                    }
                }

            if (gCBParent == value.Id)
            {
                CBParent = new DataObjectWrapper(value, _repository);
                _repository.SubscribeObjects(value.Children).Subscribe(this);
                return;
            }
            if (CBParent != null)
                if (CBParent.Children.ToList().Exists(n => n == value.Id))
                {
                    var _attr = attrToList(CBAttr);
                    ItemCB item = new ItemCB() { attr = new Dictionary<string, string>() };
                    foreach (var atr in _attr)
                    {
                        if (value.Attributes.ToList().Exists(n => n.Key == atr))
                        {
                            var obj = value.Attributes.ToList().Find(n => n.Key == atr);
                            item.attr.Add(obj.Key, obj.Value.ToString());
                        }
                    }
                    item.DispName = TextToCombobox(item);
                    if (_CBItems.ToList().Exists(n => n.Id == value.Id))
                    {
                        var itemC = _CBItems.First(n => n.Id == value.Id);
                        itemC.Update(value.DisplayName);
                    }
                    else
                    {
                        _CBItems.Add(item);
                       // _CBItems = new ObservableCollection<ItemCB>(_CBItems.OrderBy(n => n.DispName));
                        //NotifyPropertyChanged("CBItems");
                    }
                    return;
                }

           

            if (SystemObjectIds.RootObjectId == value.Id)
            {
                _baseElement = new DataObjectWrapper(value, _repository);
                _repository.SubscribeObjects(value.Children).Subscribe(this);
                return;
            }
            if (_baseElement.Children.Contains(value.Id))
            {
                if (nameTypeProjectFolder == "")
                {
                    getNameProjectFolder(value);
                }

                if (value.Type.Name == nameTypeProjectFolder)// 3
                {
                    if (!_TreeProject.ToList().Exists(n => n.Id == value.Id))
                        _TreeProject.Add(new ElementNodeViewModel(_tabServiceProvider, null, value, _repository, "_TreeProject", nameTypeProject, nameTypeProjectFolder));
                    else
                    {
                        var node = _TreeProject.First(n => n.Id == value.Id);
                        node.Update(value);
                    }
                }
            }
            else if (selectSample != null)
                if (selectSample.Source.Children.ToList().Exists(n => n == value.Id))
                {
                    var s = value.Type.Name;
                    if (value.Type.HasFiles == false && value.Type.Name != "File" && value.Type.Name != "Smart_folder_type"	 && value.Type.Name != "Project_folder" && "Shortcut_E67517F1-93F5-4756-B651-133B816D43C8" != value.Type.Name)
                        if (!_TreeObj.ToList().Exists(n => n.Id == value.Id))
                            _TreeObj.Add(new ElementNodeViewModel(_tabServiceProvider, null, value, _repository, "_TreeObj", nameTypeProject, nameTypeProjectFolder));
                        else
                        {
                            var node = _TreeObj.First(n => n.Id == value.Id);
                            node.Update(value);
                        }

                    if (value.Type.Name == "File" || value.Type.Name == "Project_folder")
                        if (!_TreeStorage.ToList().Exists(n => n.Id == value.Id))
                            _TreeStorage.Add(new ElementNodeViewModel(_tabServiceProvider, null, value, _repository, "_TreeStorage", nameTypeProject, nameTypeProjectFolder));
                        else
                        {
                            var node = _TreeStorage.First(n => n.Id == value.Id);
                            node.Update(value);
                        }
                }
            // _repository.SubscribeObjects(value.Children).Subscribe(this);

        }

        private void getNameProjectFolder(IDataObject _object)
        {
            foreach (var i in _object.Type.Children)
            {
                var _type = _repository.GetType(i);
                if (_type.IsMountable)
                {
                    nameTypeProjectFolder = _object.Type.Name;                    
                    return;
                }
            }            
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }




        private string TextToCombobox(ItemCB item) // переделать для комбобокса
        {
            var AttrItems = "";

            var attr = "";
            foreach (var atr in item.attr)
            {
                if (attr.Length > 0)
                {
                    attr += " - ";
                }
                attr += atr.Value;

            } AttrItems += attr;


            return AttrItems;
        }

        public ObservableCollection<ItemCB> CBItems
        {
            get
            {
                return _CBItems;
            }
            set
            {
                _CBItems = new ObservableCollection<ItemCB>(value.OrderBy(n => n.DispName));
                NotifyPropertyChanged("CBItems");
            }

        }
        public bool SelectElementCatalogModel
        {
            get { return _SelectElementCatalogModel; }
            set
            {
                _SelectElementCatalogModel = value;
                NotifyPropertyChanged("SelectElementCatalogModel");
            }
        }
        public ObservableCollection<ElementCatalogModel> SearchItemsCatalog
        {
            get { return _searchItemsCatalog; }
        }
        public ObservableCollection<ElementCatalogModel> ItemsCatalog
        {
            get { return _itemsCatalog; }            
        }
        public string NameCatalog
        {
            get
            {
                return _NameCatalog;
            }
            set
            {
                _NameCatalog = value;
                NotifyPropertyChanged("NameCatalog");
            }
        }

        /*  public DelegateCommand OpenDialog
          {
              get
              {
                  return _openDialog;
              }
          }*/

        public string openDialog(string attr, string key)
        {
            //   new Ascon.Pilot.Theme.Controls.DialogWindow().Title

            GetItemsCatalog(attr, key);
            _SelectElementCatalogModel = false;
            DialogWindow dw = new DialogWindow();
            opDialog = new Catalog();
            dw.Content = opDialog;
            dw.Title = "Справочник";
            dw.DataContext = this;
            dw.ShowDialog();
            
            return ResultCatalog();

            //  System.Windows.MessageBox.Show("");

        }

        private string ResultCatalog()
        {
            var AttrItems = "";

            foreach (var itemCatalog in _itemsCatalog)
            {
                if (itemCatalog.isCheck)
                {
                    if (AttrItems.Length > 0)
                    {
                        AttrItems += "; ";
                    }
                    var attr = "";
                    foreach (var atr in itemCatalog.Attr)
                    {
                        if (attr.Length > 0)
                        {
                            attr += " - ";
                        }
                        attr += atr.Value;

                    } AttrItems += attr;
                }
            }
            foreach (var itemCatalog in _searchItemsCatalog)
            {
                if (itemCatalog.isCheck)
                {
                    if (AttrItems.Length > 0)
                    {
                        AttrItems += "; ";
                    }
                    var attr = "";
                    foreach (var atr in itemCatalog.Attr)
                    {
                        if (attr.Length > 0)
                        {
                            attr += " - ";
                        }
                        attr += atr.Value;
                    } AttrItems += attr;
                }
            }

            return AttrItems;
        }
        public DelegateCommand Search
        {
            get
            {
                return _search;
            }
        }
        private void search()//поиск
        {
            if (opDialog.TextSearch != null)
            {
                var newSearchItemsCatalog = new ObservableCollection<ElementCatalogModel>();
                //  opDialog.
                if (opDialog.TextSearch != " " && opDialog.TextSearch != "")
                {
                    foreach (var itemCatalog in _itemsCatalog)
                    {
                        if (itemCatalog != _itemsCatalog[ItemsCatalog.Count - 1])
                            if (itemCatalog.DisplayName.ToUpper().IndexOf(opDialog.TextSearch.ToUpper()) > -1)
                        {
                            newSearchItemsCatalog.Add(itemCatalog);
                        }

                    }
                }
                if (newSearchItemsCatalog.Count > 0)
                {
                    ItemsCatalog[ItemsCatalog.Count-1].SearchItemsCatalog = newSearchItemsCatalog;

                }
                else
                {
                    ItemsCatalog[ItemsCatalog.Count-1].SearchItemsCatalog = new ObservableCollection<ElementCatalogModel>() { new SearchElementCatalogModel() };

                }

            }
        }
        public void GetItemsCatalog(string attr, string key)
        {
            _itemsCatalog.Clear();
            var b = Ascon.Pilot.SDK.CreatingProjectTemplate.Properties.Resources.Icon3.ToBitmap();
            _itemsCatalog.Add(new ElementCatalogModel(Imaging.CreateBitmapSourceFromHBitmap(b.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()), "Результаты поиска", new ObservableCollection<ElementCatalogModel>() { new SearchElementCatalogModel() }, this));

           // IDataObject objParent = null;
            gParentObjectOpenDialog = new Guid(key);
            attributesOpenDialog = attrToList(attr);
           /* loader.Load(gParentObjectOpenDialog, o => { objParent = o; });

            attributesOpenDialog = attrToList(attr);
            if (objParent == null) { return null; }
            string _displayName = objParent.DisplayName;
            ParentObjectOpenDialog = objParent;*/

            _repository.SubscribeObjects(new[] { gParentObjectOpenDialog }).Subscribe(this);

            //return _displayName;
        }

        /*public string ResultOpenDialog
        {
            get
            {
                return _resultOpenDialog;
            }
        }*/
        public List<ItemCB> GetItemsCB(string attr, string key)
        {
            var items = new List<ItemCB>();
            

           // IDataObject objParent = null;
            //  while (objParent == null)
            // {
            _CBItems.Clear();
            gCBParent = new Guid(key);
            CBAttr=attr;
            _repository.SubscribeObjects(new [] {gCBParent}).Subscribe(this);
           // loader.Load(g, o => { objParent = o; });
            
            //   }

          /*  var _attr = attrToList(attr);
            if (objParent == null) { return null; }
            foreach (var child in objParent.Children)
            {
                IDataObject objChild = null;
                loader.Load(child, o => { objChild = o; });
                //new GetDataObj(_repository, child).Obj;
                if (objChild != null)
                {
                    if (objChild.Children.Count == 0)
                    {
                        ItemCB item = new ItemCB() { attr = new Dictionary<string, string>(), DispName = objChild.DisplayName };


                        foreach (var atr in _attr)
                        {
                            if (objChild.Attributes.ToList().Exists(n => n.Key == atr))
                            {
                                var obj = objChild.Attributes.ToList().Find(n => n.Key == atr);
                                item.attr.Add(obj.Key, obj.Value.ToString());
                            }
                        }
                        items.Add(item);
                    }
                }
            }*/
            return items;
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





        private int SelectCount()
        {
            int count = 0;
            foreach (var obj in _TreeObj)
            {
                count += SelectCount(obj);
                count += 1;
            }
            foreach (var obj in _TreeStorage)
            {
                count += SelectCount(obj);
                count += 1;
            }

            return count;
        }

        private int SelectCount(ElementNodeViewModel elem)
        {
            int count = 0;
            foreach (var obj in elem.ChildNodes)
            {
                count += SelectCount(obj);
                count += 1;
            }

            return count;
        }

        public bool CreateUpProject(bool b)
        {
            NameTitleToCreateProject = "Выполняется создание структуры проекта";
            bool b2 = true;
            if (b)
            {
                countCreate = SelectCount();
                b2 = UpProject(_parent);
            }
            else if (!b)
            {
                countCreate = SelectCount();
                createProject(_parent, attributes);
            }
            if (modifier && b2)
                _modifier.Apply();

            NameTitleToCreateProject = "Создание структуры проекта успешно завершено!";
            CreateCompleted = true;
            //AddItemResult(null, "Готово");
            return b2;
        }

        private AccessLevel GetMyAccessLevel(IDataObject element)
        {
            var currentAccesLevel = AccessLevel.None;
            foreach (var position in _currentPerson.Positions)
            {
                IAccess access;
                if (!element.Access.TryGetValue(position.Position, out access))
                    continue;

                currentAccesLevel = access.AccessLevel;
            }

            return currentAccesLevel;
        }

        private bool UpProject(IDataObject project)
        {
            var myAccess = GetMyAccessLevel(project);
            if ((myAccess == AccessLevel.ViewEdit) || (myAccess == AccessLevel.ViewCreate))
            {
                //обновляем структуру                

                _selectTreeObj.Add(new ElementNodeViewModel(_tabServiceProvider, null, project, _repository, "_TreeObj", nameTypeProject, nameTypeProjectFolder));
                _selectTreeStorage.Add(new ElementNodeViewModel(_tabServiceProvider, null, project, _repository, "_TreeStorage", nameTypeProject, nameTypeProjectFolder));

                createUpdateStructure(project, true);
                return true;
            }
            else return false;

        }
        private void createProject(IDataObject projectfolder, Dictionary<string, object> attributes)
        {
            //обновляем структуру 
            createUpdateStructure(createObject(projectfolder, nameTypeProject, attributes), false);
        }
        private IDataObject createObject(IDataObject patent, string _type, IDictionary<string, object> attributes)
        {
            var typeObj = _repository.GetType(_type);
            var builder = _modifier.Create(patent, typeObj);

            AddItemResult(typeObj, "Проект создан");
            foreach (var attribute in attributes)
            {
                if (attribute.Value is string)
                    builder.SetAttribute(attribute.Key, (string)attribute.Value);
                if (attribute.Value is int)
                    builder.SetAttribute(attribute.Key, (int)attribute.Value);
                if (attribute.Value is double)
                    builder.SetAttribute(attribute.Key, (double)attribute.Value);
                if (attribute.Value is DateTime)
                    builder.SetAttribute(attribute.Key, (DateTime)attribute.Value);
            }
            if (CreateHidden) { builder.MakeSecret(); }
            modifier = true;
            return builder.DataObject;
        }

        private IDataObject createObject(IDataObject patent, string _type, DataObjectWrapper obj, bool b)
        {
            var typeObj = _repository.GetType(_type);
            var builder = _modifier.Create(patent, typeObj);
            foreach (var attribute in obj.Attributes)
            {
                if (attribute.Value is string)
                    builder.SetAttribute(attribute.Key, (string)attribute.Value);
                if (attribute.Value is int)
                    builder.SetAttribute(attribute.Key, (int)attribute.Value);
                if (attribute.Value is double)
                    builder.SetAttribute(attribute.Key, (double)attribute.Value);
                if (attribute.Value is DateTime)
                    builder.SetAttribute(attribute.Key, (DateTime)attribute.Value);
            }


            IDataObject _obj = null;

            loader.Load(obj.Id, o => { _obj = o; });
            try
            {
                foreach (var file in _obj.Files)
                {

                    builder.AddFile(file.Name, _fileProvider.OpenRead(file), file.Created, file.Accessed, file.Modified);

                }
                AddItemResult(typeObj, typeObj.Title + ": " + obj.DisplayName + " скопирован");
            }
            catch
            {
                AddItemResult(typeObj, typeObj.Title + ": " + obj.DisplayName + " не найден");
            }
           
            if (b)
                foreach (var access in obj.Access)
                {
                    builder.SetAccessRights(access.Key, access.Value.AccessLevel, access.Value.ValidThrough, access.Value.IsInherited);
                    var OrganUnit = _repository.GetPerson(access.Key);
                    var name = OrganUnit.DisplayName;

                    if (OrganUnit.DisplayName == "")
                    {
                        var OrgUnit = _repository.GetOrganisationUnit(access.Key);
                        name = OrgUnit.Title;
                    }
                    AddItemResult(null, "- Назначены права доступа для " + name + " , уровня " + access.Value.AccessLevel);

                }

            modifier = true;
            if (CreateHidden) { builder.MakeSecret(); }
            return builder.DataObject;
        }

        private void AddItemResult(IType typeObj, string Text)
        {
            _resultCreation.Add(new Result(typeObj, Text));
            if (Text[0].ToString() != "-")
            processStatus = Text;
            indexCreate += 1;
            NotifyPropertyChanged("resultCreation");        
        }

        public void Dispose()
        {
            foreach (var obj in _itemsCatalog)
            {
        //        obj.Dispose();
            }
            foreach (var obj in _searchItemsCatalog)
            {
        //        obj.Dispose();
            }
            foreach (var obj in _TreeObj)
            {
                obj.Dispose();
            }
            foreach (var obj in _TreeProject)
            {
                obj.Dispose();
            }
            foreach (var obj in _TreeStorage)
            {
                obj.Dispose();
            }
            foreach (var obj in _selectTreeObj)
            {
                obj.Dispose();
            }
            foreach (var obj in _selectTreeStorage)
            {
                obj.Dispose();
            }  
        }

        private bool comparisonObject(ObservableCollection<ElementNodeViewModel> _object, ElementNodeViewModel _object2, out ElementNodeViewModel obj2)
        {
            ObservableCollection<ElementNodeViewModel> s = new ObservableCollection<ElementNodeViewModel>();
            if (_object != null)
                while (_object.ToList().Exists(n => n.DisplayName == _object2.DisplayName))
                {
                    obj2 = _object.First(n => n.DisplayName == _object2.DisplayName);
                    s.Add(obj2);
                    _object.Remove(obj2);
                }

            obj2 = null;
            if (s.ToList().Exists(n => n.TypeObj == _object2.TypeObj))
                obj2 = s.First(n => n.TypeObj == _object2.TypeObj);

            if (obj2 != null)
            {
                return true;
            }
            return false;
        }

        private void comparisonStructure(ObservableCollection<ElementNodeViewModel> objProject, ObservableCollection<ElementNodeViewModel> Obj, IDataObject parent, bool b)
        {
            // IDataObject _parent = null;
            foreach (var obj in Obj)
            {
                IDataObject _parent = null;
                if (obj.Check)
                {

                    /*     ObservableCollection<IDataObject> DataObjects = new ObservableCollection<IDataObject>();
                         if (objProject != null)
                             foreach (var guidObj in objProject)
                             {
                                 DataObjects.Add(getDObj.GetDataObject(guidObj));
                             }*/
                    ElementNodeViewModel obj2 = null;
                    // if (DataObjects.Count > 0)

                    if (!comparisonObject(objProject, obj, out obj2))
                    {
                        //создание объекта 
                        _parent = createObject(parent, obj.TypeObj, obj.Source, b);
                    }
                    if (_parent != null)
                        comparisonStructure(null, obj.ChildNodes, _parent, b);
                    else
                    {
                        
                        IDataObject _obj = null;
                        loader.Load(obj2.Source.Id, o => { _obj = o; });
                        comparisonStructure(obj2.ChildNodes, obj.ChildNodes, _obj, b);
                    }
                }
            }
        }

        private void createUpdateStructure(IDataObject Parent, bool b)
        {
            if (b)
            {
                // foreach (var obj in _TreeObj)
                // {
                
                IDataObject _obj = null;
                loader.Load(_selectTreeObj[0].Source.Id, o => { _obj = o; });
                comparisonStructure(_selectTreeObj[0].ChildNodes, _TreeObj, _obj, CopyAccessObj);
                //   }

                //foreach (var obj in _TreeStorage)
                // {var GetObj = new ObjectLoader(_repository);
                _obj = null;
                loader.Load(_selectTreeStorage[0].Source.Id, o => { _obj = o; });
                comparisonStructure(_selectTreeStorage[0].ChildNodes, _TreeStorage, _obj, CopyAccessStorage);
                // }
            }
            else
            {
                //foreach (var obj in _TreeObj)
                // {

                comparisonStructure(null, _TreeObj, Parent, CopyAccessObj);
                //  }

                //  foreach (var obj in _TreeStorage)
                //  {

                comparisonStructure(null, _TreeStorage, Parent, CopyAccessStorage);
                //  }
            }

        }



    }
}
