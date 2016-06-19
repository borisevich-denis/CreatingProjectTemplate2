using Ascon.Pilot.Theme.ColorScheme;
using Ascon.Pilot.Theme.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{

    [Export(typeof(IObjectContextMenu))]
    public class ContextMenu : IObjectContextMenu
    {
        private readonly IObjectsRepository _repository;
        private IDataObject _selection;
        private readonly IObjectModifier _modifier;
        private readonly IFileProvider _fileProvider;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly IPersonalSettings _personalSettings;
        private readonly IPilotDialogService _pilotDialogService;

        private const string ProjectTemplate = "ProjectTemplate";
        private const string ProjectFolderTemplate = "ProjectFolderTemplate";

        private string nameTypeProjectFolder;
        private string nameTypeProject;

        [ImportingConstructor]
        public ContextMenu(IPersonalSettings personalSettings, ITabServiceProvider tabServiceProvider, IFileProvider fileProvider, IObjectModifier modifier, IObjectsRepository repository, IPilotDialogService pilotDialogService)
        {
            _pilotDialogService = pilotDialogService;
            _personalSettings = personalSettings;
            _tabServiceProvider = tabServiceProvider;
            _repository = repository;
            _modifier = modifier;
            _fileProvider = fileProvider;
            var accentColor = (Color)ColorConverter.ConvertFromString(pilotDialogService.AccentColor);
            ColorScheme.Initialize(accentColor);
        }


        public void BuildContextMenu(IMenuHost menuHost, IEnumerable<IDataObject> selection, bool isContext)
        {
            var objects = selection.ToList();//+++
            if (objects.Count() == 1)
            {
                //   var icon = IconLoader.GetIcon("/Resources/menu_icon.svg");//получение иконки

                //  var itemNames = menuHost.GetItems().ToList();
                //  const string indexItemName = "miShowSharingSettings";
                //  var insertIndex = itemNames.IndexOf(indexItemName) + 1;
                _selection = objects.FirstOrDefault();
                var menu = menuHost.GetItems().ToList();//+++
                byte[] SvgIcon = null;//+++
                if (menu.Exists(n => n == "miCreate"))
                {
                    int index = 0;
                    var menuSub = menuHost.GetItems("miCreate").ToList();//+++
                    var m = menuHost;//+++
                    if (menuSub.Exists(n => n == "miCreateSmartfoldertype"))
                    {
                        index = menuSub.IndexOf("miCreateSmartfoldertype") - 1;
                    }
                    else index = menuSub.Count;
                    if (objects[0].Type.IsMountable)
                    {
                        nameTypeProject = objects[0].Type.Name;
                        menuHost.AddSubItem("miCreate", "ProjectTemplate", "Структура проекта по шаблону", objects[0].Type.SvgIcon, index);//нужно выделить хз как =)
                    }
                    else if (getProjectFolder(objects[0], ref SvgIcon)/*objects[0].Type.Name == "projectfolder"*/)
                    {
                        nameTypeProjectFolder = objects[0].Type.Name;
                        menuHost.AddSubItem("miCreate", "ProjectFolderTemplate", "Проект на основе шаблона", SvgIcon, index);
                    }
                }
            }
        }

        private bool getProjectFolder(IDataObject _object, ref byte[] SvgIcon)
        {
            foreach (var i in _object.Type.Children)
            {
                var _type = _repository.GetType(i);//+++
                if (_type.IsMountable)
                {
                    nameTypeProject = _type.Name;
                    SvgIcon = _type.SvgIcon;
                    return true;
                }
            }
            return false;
        }

        public void OnMenuItemClick(string itemName)
        {
            try
            {
                if (itemName == ProjectFolderTemplate)
                {
                    var window = new PureWindow();//+++
                    var _createProject = new CreateProject();//+++
                   _createProject.DataContext= new TreeViewModel(_pilotDialogService, _tabServiceProvider, _repository, _selection, _modifier, _fileProvider, _personalSettings, nameTypeProjectFolder, nameTypeProject);//+++
              
                    //var window = new PureWindow { Content = _createProject };
                    window.Content = _createProject;
                    window.Title = "Мастер создания проекта по шаблону";
                    window.Show();//+++
                   
                }

                if (itemName == ProjectTemplate)
                {
                    var window = new PureWindow();
                    var _createProject = new CreateProjectStructure();//+++
                    _createProject.DataContext = new TreeViewModel(_pilotDialogService, _tabServiceProvider, _repository, _selection, _modifier, _fileProvider, _personalSettings, nameTypeProjectFolder, nameTypeProject);//+++
                    //var window = new PureWindow { Content = _createProject };
                    window.Content = _createProject;
                    window.Title = "Мастер создания cструктуры проекта по шаблону";
                    window.Show();//+++
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
