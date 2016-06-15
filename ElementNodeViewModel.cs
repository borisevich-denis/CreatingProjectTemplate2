using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Media;
using System.Windows.Resources;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Windows.Interop;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{

    class ElementNodeViewModel : PropertyChangedBase, IObserver<IDataObject>, IDisposable
    {
        private string _displayName;
        private string _type;
        private DataObjectWrapper _source;
        private readonly IObjectsRepository _repository;
        private readonly ObservableCollection<ElementNodeViewModel> _childNodes;        
        private bool _isExpanded = true;
        private readonly string _tree;
        private bool _check;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly ElementNodeViewModel _parendNodes;
        private readonly DelegateCommand _openFile;
        private readonly DelegateCommand _openFileStorage;
        private string nameTypeProjectFolder;
        private string nameTypeProject;
        private ImageSource _icon;
        public ElementNodeViewModel()
        {
            DisplayName = "Loading";
        }

        public ElementNodeViewModel(ITabServiceProvider tabServiceProvider, ElementNodeViewModel parend, IDataObject source, IObjectsRepository repository, string tree, string _nameTypeProject, string _nameTypeProjectFolder)
        {
            nameTypeProject = _nameTypeProject;
            nameTypeProjectFolder = _nameTypeProjectFolder;
            _tabServiceProvider = tabServiceProvider;
            _tree = tree;
            _childNodes = new ObservableCollection<ElementNodeViewModel>();
            _parendNodes = parend;
            _source = new DataObjectWrapper(source, repository);
            _type = source.Type.Name;
            _displayName = _source.DisplayName;
            _repository = repository;
            Id = source.Id;
            _openFile = new DelegateCommand(openFile);
            _openFileStorage = new DelegateCommand(openFileStorage);
            GetIcon();
            // getDObj = new GetDataObj(_repository);
            if (tree == "_TreeStorage")
            {
                _check = true;
            }
            else if (tree == "_TreeObj")
            {
                _check = true;
            }
            else if (tree == "_TreeProject")
            {
                _check = false;
            }
        }

        public void Dispose()
        {      
            foreach (var obj in _childNodes)
            {
                obj.Dispose();
            }
        }

        private void GetIcon()
        {
            if (DisplayName == "Loading") return;
            if (TypeObj == "File")
            {
                _icon= Ascon.Pilot.Theme.Icons.Instance.FileIcon;
            }
            //  var sri = new StreamResourceInfo();//+++
            var iconByte = _repository.GetType(TypeObj).SvgIcon;//+++

            StreamSvgConverter Isc = new StreamSvgConverter(new WpfDrawingSettings());
            Bitmap bm = null;
            using (Stream s = new MemoryStream())
            {
                Isc.Convert(new MemoryStream(iconByte), s);
                bm = new Bitmap(s);
                Isc.Dispose();
                _icon = Imaging.CreateBitmapSourceFromHBitmap(bm.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                //bm.Dispose();
            }
        }

        public ImageSource Icon
        {
            get
            {
                GetIcon();
                return _icon;
            }
        }
       
        public Guid Id
        {
            get;
            private set;
        }     

        public DelegateCommand OpenFile
        {
            get { return _openFile; }
        }

        public DelegateCommand OpenFileStorage
        {
            get { return _openFileStorage; }
        }

        public DataObjectWrapper Source
        {
            get{return _source;}
           // private set;
        }

        public IObjectsRepository repository
        {
            get { return _repository; }
        }

        public IDictionary<string, object> Attributes
        {
            get 
            {
                return _source.Attributes; 
            }
        }
        private void openFileStorage()
        {
            if (TypeObj == nameTypeProject)
            {
                var directori = System.IO.Directory.EnumerateDirectories(_repository.GetStoragePath());//+++
                var s = Name(_displayName);//+++
                foreach (var folderName in directori)
                {
                    if (folderName.IndexOf(s) > -1)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = "explorer";
                        Proc.StartInfo.Arguments = (folderName + "\\");
                        Proc.Start();
                        return;
                    }
                }                
            }
        }

        private string Name(string _name)
        {
            int i = _name.IndexOf(" - ");
            while (i > -1)
            {
                _name = _name.Remove(i, 3);
                _name = _name.Insert(i, "-");
                i = _name.IndexOf(" - ");
            }
            if (_name.Length > 40)
            {
                return _name.Remove(39) + "~";
            } else return _name;
        }

        private void openFile()
        {
            _tabServiceProvider.ShowElement(Id);
        }

        public bool Check
        {
            get { return _check; }
            set
            {
                _check = value;
                if (!value)
                {
                    CheckChilds(value);
                }
                if (value)
                {
                    CheckChilds(value);
                }
                NotifyPropertyChanged("Check");
            }
        }

        private void CheckChilds(bool b)
        {
            if (!b)
                foreach (var child in _childNodes)
                {
                    child.Check = b;
                }
            else if (b)
            {
                if (_parendNodes != null)
                {
                    _parendNodes.Check = b;
                }
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                NotifyPropertyChanged("DisplayName");
            }
        }
        public string TypeObj
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("TypeObj");
            }
        }

        public ObservableCollection<ElementNodeViewModel> ChildNodes
        {
            get
            {
                if (_source == null)
                    return null;

                if (_source.Children.Count == 0)
                    return _childNodes;

                if (_childNodes.Count != 0)
                    return _childNodes;

                if (!treeEnd()) { }
                    _repository.SubscribeObjects(_source.Children).Subscribe(this);
             //   _childNodes.Add(new LoadingElementNodeViewModel());
                return _childNodes;
            }            
        }

        private bool treeEnd()
        { 
            if (_tree == "_TreeObj")
            {
                if (_source != null)
                        if (_source.ListViewChildren != null)
                            if (_source.ListViewChildren.Count > 0) 
                        {
                            _childNodes.Add(new LoadingElementNodeViewModel());
                            return false;
                        }
                
            }
            else
                if (_tree == "_TreeStorage")
                {
                    if (_source != null)
                        if (_source.PilotStorageChildren != null)
                            if (_source.PilotStorageChildren.Count > 0) 
                        {
                       /* getDObj.Obj = null;
                        while (getDObj.GetDataObject(s) == null)
                        { }*/
                        
                            _childNodes.Add(new LoadingElementNodeViewModel());
                      //  if (getDObj.Obj.Type.Name == "File" || getDObj.Obj.Type.Name == "Project_folder")
                            return false;
                    }
                }
                else
                    if (_tree == "_TreeProject")
                    {
                        if (_source != null)
                        if (_source.ListViewChildren != null)
                            if (_source.ListViewChildren.Count > 0) 
                            if (_source.Type.Name == "projectfolder")
                        {
                            _childNodes.Add(new LoadingElementNodeViewModel());
                            return false;
                        }
                       /* foreach (var s in _source.ListViewChildren)
                        {
                            var getDObj = new GetDataObj(_repository, s);
                          /*  getDObj.Obj = null;
                            while (getDObj.GetDataObject(s) == null)
                            { }*/
                        /*    if (getDObj.Obj != null)
                                if (getDObj.Obj.Type.Name == "project" || getDObj.Obj.Type.Name == "projectfolder")
                                {
                                    _childNodes.Add(new LoadingElementNodeViewModel());
                                    return false;
                                }
                        }*/
                    }
            return true;
        }

        public bool IsExpanded
        {
            get
            {
               /* if (_isExpanded)
                {
                    if (_childNodes != null)
                        if (_childNodes.Count == 1)
                        {
                            _repository.SubscribeObjects(_source.Children).Subscribe(this);
                            //return _isExpanded;
                        }

                }*/
          //      NotifyPropertyChanged("IsExpanded");
                return _isExpanded;
            }
            set
            {
                // if (_isExpanded == value)
                  //  return;

                if (value)
                {
                    if (_childNodes != null)
                        if (_childNodes.Count == 1)
                        {
                            _repository.SubscribeObjects(_source.Children).Subscribe(this);
                        }
                }

                _isExpanded = value;
                NotifyPropertyChanged("IsExpanded");
            }
        }

        public void OnNext(IDataObject value)
        {
            if (_tree == "_TreeObj")
            {
                if (value.Type.Name != "File")
                    if (value.Type.Name != "Project_folder")
                       // if ("Shortcut_E67517F1-93F5-4756-B651-133B816D43C8" != value.Type.Name) 
                        //1
                        {
                            if (!_source.Children.Contains(value.Id))
                                return;

                            if (IsLoading())
                                _childNodes.Clear();
                            if (value.Type.HasFiles == false && value.Type.Name != "Shortcut_E67517F1-93F5-4756-B651-133B816D43C8")
                            {
                                if (!_childNodes.ToList().Exists(n => n.Id == value.Id))
                                    _childNodes.Add(new ElementNodeViewModel(_tabServiceProvider, this, value, _repository, _tree, nameTypeProject,nameTypeProjectFolder));
                                else
                                {
                                    var node = _childNodes.First(n => n.Id == value.Id);
                                    node.Update(value);
                                }
                            }
                            else
                            {
                                if (_childNodes.ToList().Exists(n => n.Id == value.Id))
                                {
                                    var node = _childNodes.First(n => n.Id == value.Id);
                                    _childNodes.Remove(node);
                                }
                                
                            }
                        }
            }
            else
                if (_tree == "_TreeStorage")
                {
                    if (value.Type.Name == "File" || value.Type.Name == "Project_folder")//2
                    {


                        if (!_source.Children.Contains(value.Id))
                            return;

                        if (IsLoading())
                            _childNodes.Clear();

                        if (!_childNodes.ToList().Exists(n => n.Id == value.Id))
                            _childNodes.Add(new ElementNodeViewModel(_tabServiceProvider,this, value, _repository, _tree,nameTypeProject,nameTypeProjectFolder));
                        else
                        {
                            var node = _childNodes.First(n => n.Id == value.Id);
                            node.Update(value);
                        }
                    }
                }
                else
                    if (_tree == "_TreeProject")
                    {
                        if (value.Type.Name == nameTypeProject || value.Type.Name == nameTypeProjectFolder)
                        {
                            if (!_source.Children.Contains(value.Id))
                                return;

                            if (IsLoading())
                                _childNodes.Clear();

                            if (!_childNodes.ToList().Exists(n => n.Id == value.Id))
                                _childNodes.Add(new ElementNodeViewModel(_tabServiceProvider,this, value, _repository, _tree,nameTypeProject,nameTypeProjectFolder));
                            else
                            {
                                var node = _childNodes.First(n => n.Id == value.Id);
                                node.Update(value);
                            }
                        }
                    }
        }

        public void OnError(Exception error)
        {

        }

        public void OnCompleted()
        {

        }

        
        public void Update(IDataObject newSource)
        {
            _source = new DataObjectWrapper (newSource,repository);
            DisplayName = _source.DisplayName;
            UpdateChildNodes();
        }

        private void UpdateChildNodes()
        {
            NotifyPropertyChanged("ChildNodes");
            RemoveDeletedNodes();
            LoadNewNodes();
        }
        private void RemoveDeletedNodes()
        {
            if (IsLoading())
                return;

            foreach (var childNode in _childNodes.ToList())
            {
                if (!_source.Children.Contains(childNode.Id))
                    _childNodes.Remove(childNode);
            }
        }

        private void LoadNewNodes()
        {
            if (IsLoading())
                return;

            var childIdsToLoad = new List<Guid>();//+++
            foreach (var childId in _source.Children)
            {
                if (_childNodes.FirstOrDefault(x => x.Id == childId) == null)
                    childIdsToLoad.Add(childId);
            }
            
            _repository.SubscribeObjects(childIdsToLoad).Subscribe(this);
        }

        private bool IsLoading()
        {
            return _childNodes.FirstOrDefault() != null &&
                   _childNodes.First().GetType() == typeof(LoadingElementNodeViewModel);
        }
    }




    class LoadingElementNodeViewModel : ElementNodeViewModel
    {
    }
}
