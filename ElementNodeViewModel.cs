using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{

    class ElementNodeViewModel : PropertyChangedBase, IObserver<IDataObject>
    {
        private string _displayName;
        private string _type;
        private IDataObject _source;
        private readonly IObjectsRepository _repository;
        private readonly ObservableCollection<ElementNodeViewModel> _childNodes;        
        private bool _isExpanded = true;
        private readonly string _tree;
        private bool _check;
        private readonly ElementNodeViewModel _parendNodes;
//        private GetDataObj getDObj;
       // private IDictionary<string,object> _attr;
        public ElementNodeViewModel()
        {
            DisplayName = "Loading";
        }

        public ElementNodeViewModel(ElementNodeViewModel parend, IDataObject source, IObjectsRepository repository, string tree)
        {
            _tree = tree;
            _childNodes = new ObservableCollection<ElementNodeViewModel>();
            _parendNodes = parend;
            _source = source;
            _type = source.Type.Name;
            _displayName = _source.DisplayName;
            _repository = repository;
            Id = source.Id;
           // getDObj = new GetDataObj(_repository);
            if (tree == "_TreeStorage")
            {
                if ((_type == "Project_folder") || (_type == "project"))
                {
                    _check = true;
                }
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

        public Guid Id
        {
            get;
            private set;
        }

        public IDataObject Source
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

                if (!treeEnd())
                    _repository.SubscribeObjects(_source.Children).Subscribe(this);
             //   _childNodes.Add(new LoadingElementNodeViewModel());
                return _childNodes;
            }
        }

        private bool treeEnd()
        { 
            if (_tree == "_TreeObj")
            {
                foreach (var s in _source.Children)
                {
                    var getDObj=new GetDataObj(_repository,s);
                    /*getDObj.Obj = null;
                    while (getDObj.GetDataObject(s) == null)
                    { }*/
                    if (getDObj.Obj != null)
                    if (getDObj.Obj.Type.Name != "File")
                        if (getDObj.Obj.Type.Name != "Project_folder")
                            if (getDObj.Obj.Type.HasFiles == false)
                                return false;
                }
            }
            else
                if (_tree == "_TreeStorage")
                {
                    foreach (var s in _source.Children)
                    {
                        var getDObj = new GetDataObj(_repository, s);
                       /* getDObj.Obj = null;
                        while (getDObj.GetDataObject(s) == null)
                        { }*/
                        if (getDObj.Obj != null)
                        if (getDObj.Obj.Type.Name == "File" || getDObj.Obj.Type.Name == "Project_folder")
                            return false;
                    }
                }
                else
                    if (_tree == "_TreeProject")
                    {
                        foreach (var s in _source.Children)
                        {
                            var getDObj = new GetDataObj(_repository, s);
                          /*  getDObj.Obj = null;
                            while (getDObj.GetDataObject(s) == null)
                            { }*/
                            if (getDObj.Obj != null)
                            if (getDObj.Obj.Type.Name == "project")
                                return false;
                        }
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
                if (_isExpanded == value)
                    return;

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
                        if (value.Type.HasFiles == false)//1
                        {
                            if (!_source.Children.Contains(value.Id))
                                return;

                            if (IsLoading())
                                _childNodes.Clear();

                            if (!_childNodes.ToList().Exists(n => n.Id == value.Id))
                                _childNodes.Add(new ElementNodeViewModel(this, value, _repository, _tree));
                            else
                            {
                                var node = _childNodes.First(n => n.Id == value.Id);
                                node.Update(value);
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
                            _childNodes.Add(new ElementNodeViewModel(this, value, _repository, _tree));
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
                        if (value.Type.Name == "project")
                        {
                            if (!_source.Children.Contains(value.Id))
                                return;

                            if (IsLoading())
                                _childNodes.Clear();

                            if (!_childNodes.ToList().Exists(n => n.Id == value.Id))
                                _childNodes.Add(new ElementNodeViewModel(this, value, _repository, _tree));
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
            _source = newSource;
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

            var childIdsToLoad = new List<Guid>();
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
