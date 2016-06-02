using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    class ElementCatalogModel : PropertyChangedBase
    {
        private ObservableCollection<ElementCatalogModel> _searchItemsCatalog;
        private IDictionary<string, string> _attr;
        private string _displayName;
        private TreeViewModel _treemodel;
        private bool ischeck;
        private bool isExpanded;

        public Guid Id
        {
            get;
            set;
        }

        public string Sort
        {
            get;
            set;
        }

        public ImageSource Icon
        {
            get;
            set;
        }
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                isExpanded = value;
                NotifyPropertyChanged("IsExpanded");
            }
        }
        public System.Windows.Visibility visibility
        {
            get;
            set;
        }
        public bool isCheck
        {
            get
            {
                return ischeck;
            }
            set
            {
                ischeck = value;
                bool _ischeck=false;
                foreach (var item in _treemodel.ItemsCatalog)
                {
                    if(item.isCheck)
                    {
                        _ischeck = item.isCheck;
                        break;
                    }
                }
                if (!ischeck)
                foreach (var item in _treemodel.SearchItemsCatalog)
                {
                    if (item.isCheck)
                    {
                        _ischeck = item.isCheck;
                        break;
                    }
                }
                _treemodel.SelectElementCatalogModel = _ischeck;
            }
        }
        public IDictionary<string, string> Attr
        {
            get { return _attr; }
            set { _attr = value; }
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
        public ObservableCollection<ElementCatalogModel> SearchItemsCatalog 
        {
            get { return _searchItemsCatalog; }
            set
            {                
                _searchItemsCatalog = value;
                NotifyPropertyChanged("SearchItemsCatalog");
                IsExpanded = true; ;
            }   
        }

        public ElementCatalogModel( ImageSource _Icon, string displayName, Guid id, IDictionary<string, string> attr, TreeViewModel treemodel)
        {
            Id = id;
            _treemodel = treemodel;
            _displayName = displayName;
            Sort = displayName;
            _attr = attr;
            Icon = _Icon;
            visibility = System.Windows.Visibility.Visible;
        }
        public ElementCatalogModel(ImageSource _Icon, string displayName, ObservableCollection<ElementCatalogModel> searchItemsCatalog, TreeViewModel treemodel)
        {
            _treemodel = treemodel;
            _searchItemsCatalog = searchItemsCatalog;
            _displayName = displayName;        
            Icon = _Icon;
            Sort = "яяяяяяя";
            visibility = System.Windows.Visibility.Collapsed;
        }
        public ElementCatalogModel()
        {
            IsExpanded = false;
            DisplayName = "Поиск не дал результатов";
            visibility = System.Windows.Visibility.Collapsed;
        }

        public void Update(string displayName)
        {
            DisplayName = displayName;
        }
        
    }
    class SearchElementCatalogModel : ElementCatalogModel
    {
    }
}
