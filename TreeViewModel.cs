using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ascon.Pilot.SDK;
//using System.Windows;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    class TreeViewModel : IObserver<IDataObject>
    {
        private readonly IObjectsRepository _repository;
        private readonly IObjectModifier _modifier;
        private readonly IFileProvider _fileProvider;

        private readonly Guid _rootGuid;
        private readonly ObservableCollection<ElementNodeViewModel> _TreeObj;
        private readonly ObservableCollection<ElementNodeViewModel> _TreeStorage;
        private readonly ObservableCollection<ElementNodeViewModel> _TreeProject;
        private readonly ObservableCollection<ElementNodeViewModel> _selectTreeObj;
        private readonly ObservableCollection<ElementNodeViewModel> _selectTreeStorage;

        private IDataObject _baseElement;
        private IDataObject select;
        private readonly IPerson _currentPerson;
        private bool modifier = false;
       // private GetDataObj getDObj;
        public TreeViewModel(IObjectsRepository repository, IDataObject selection, IObjectModifier modifier, IFileProvider fileProvider)
        {
            _TreeObj = new ObservableCollection<ElementNodeViewModel>();
            _TreeProject = new ObservableCollection<ElementNodeViewModel>();
            _TreeStorage = new ObservableCollection<ElementNodeViewModel>();
            _selectTreeObj = new ObservableCollection<ElementNodeViewModel>();
            _selectTreeStorage = new ObservableCollection<ElementNodeViewModel>();
            _repository = repository;
            _modifier = modifier;
            _fileProvider = fileProvider;
            _rootGuid = new Guid("00000001-0001-0001-0001-000000000001");  
            select = selection;
            _repository.SubscribeObjects(new[] { _rootGuid }).Subscribe(this);
            _repository.SubscribeObjects(new[] { select.Id }).Subscribe(this);
            _currentPerson = repository.GetCurrentPerson();
            CopyAccessObj = true;
            CopyAccessStorage = true;
           // getDObj = new GetDataObj(_repository);
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
        public void OnNext(IDataObject value)
        {
            if (_rootGuid == value.Id)
            {
                _baseElement = value;
                _repository.SubscribeObjects(value.Children).Subscribe(this);
                return;
            }
            if (_baseElement.Children.Contains(value.Id))
            {

                if (value.Type.Name == "projectfolder")// 3
                {
                    if (!_TreeProject.ToList().Exists(n => n.Id == value.Id))
                        _TreeProject.Add(new ElementNodeViewModel(null, value, _repository, "_TreeProject"));
                    else
                    {
                        var node = _TreeProject.First(n => n.Id == value.Id);
                        node.Update(value);
                    }
                }
            }
            else if (select.Id == value.Id)
            {
                if (!_TreeObj.ToList().Exists(n => n.Id == value.Id))
                    _TreeObj.Add(new ElementNodeViewModel(null,value, _repository, "_TreeObj"));
                else
                {
                    var node = _TreeObj.First(n => n.Id == value.Id);
                    node.Update(value);
                }

                if (!_TreeStorage.ToList().Exists(n => n.Id == value.Id))
                    _TreeStorage.Add(new ElementNodeViewModel(null, value, _repository, "_TreeStorage"));
                else
                {
                    var node = _TreeStorage.First(n => n.Id == value.Id);
                    node.Update(value);
                }
            }
           // _repository.SubscribeObjects(value.Children).Subscribe(this);

        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }







        public bool CreateUpProject(ElementNodeViewModel selectedObject, Dictionary<string, object> attributes)
        {
            if (selectedObject.TypeObj == "project")
            {
               return UpProject(selectedObject.Source);
            }
            else if (selectedObject.TypeObj == "projectfolder")
           {
               createProject(selectedObject.Source, attributes);
           }
            if (modifier)
            _modifier.Apply();
            return true;
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

                _selectTreeObj.Add(new ElementNodeViewModel(null, project, _repository, "_TreeObj"));

                _selectTreeStorage.Add(new ElementNodeViewModel(null, project, _repository, "_TreeStorage"));

                createUpdateStructure(project, true);
                return true;
            }
            else return false;
        
        }
        private void createProject(IDataObject projectfolder, Dictionary<string,object> attributes)
        {           
            //обновляем структуру 
            createUpdateStructure(createObject(projectfolder, "project", attributes), false);
        }
        private IDataObject createObject(IDataObject patent, string _type, IDictionary<string,object> attributes)
        {
            var builder = _modifier.Create(patent, _repository.GetType(_type));
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

            modifier = true;
            return builder.DataObject;
        }

        private IDataObject createObject(IDataObject patent, string _type, IDataObject obj, bool b)
        {
            var builder = _modifier.Create(patent, _repository.GetType(_type));
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
            foreach (var file in obj.Files)
            {
                builder.AddFile(file.Name, _fileProvider.OpenRead(file), file.Created, file.Accessed, file.Modified);
            }
            if (b)
            foreach (var access in obj.Access)
            {
                builder.SetAccessRights(access.Key,access.Value.AccessLevel,access.Value.ValidThrough,access.Value.IsInherited);
            }

            modifier = true;
            return builder.DataObject;
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
                    else comparisonStructure(obj2.ChildNodes, obj.ChildNodes, obj2.Source, b);
                }
            }
        }

        private void createUpdateStructure(IDataObject Parent, bool b)
        {
            if (b)
            {
                foreach (var obj in _TreeObj)
                {
                    comparisonStructure(_selectTreeObj[0].ChildNodes, obj.ChildNodes, _selectTreeObj[0].Source, CopyAccessObj);
                }

                foreach (var obj in _TreeStorage)
                {
                    comparisonStructure(_selectTreeStorage[0].ChildNodes, obj.ChildNodes, _selectTreeStorage[0].Source, CopyAccessStorage);
                }
            }
            else
            {
                foreach (var obj in _TreeObj)
                {
                    comparisonStructure(null, obj.ChildNodes, Parent, CopyAccessObj);
                }

                foreach (var obj in _TreeStorage)
                {
                    comparisonStructure(null, obj.ChildNodes, Parent, CopyAccessStorage);
                }
            }
            
        }


        
    }
}
