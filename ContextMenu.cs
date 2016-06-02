using Ascon.Pilot.Theme.ColorScheme;
using Ascon.Pilot.Theme.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
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

        private const string CreateProjectTemplate = "CreateProjectTemplate";

        [ImportingConstructor]
        public ContextMenu(IFileProvider fileProvider, IObjectModifier modifier, IObjectsRepository repository, IPilotDialogService pilotDialogService)
        {
            _repository = repository;
            _modifier = modifier;
            _fileProvider = fileProvider;
            var accentColor = (Color)ColorConverter.ConvertFromString(pilotDialogService.AccentColor);
            ColorScheme.Initialize(accentColor);
        }


        public void BuildContextMenu(IMenuHost menuHost, IEnumerable<IDataObject> selection, bool isContext)
        {            
           var objects = selection.ToList();
           if (objects.Count() == 1)
           {
               //   var icon = IconLoader.GetIcon("/Resources/menu_icon.svg");//получение иконки

               //  var itemNames = menuHost.GetItems().ToList();
               //  const string indexItemName = "miShowSharingSettings";
               //  var insertIndex = itemNames.IndexOf(indexItemName) + 1;
               _selection = objects.FirstOrDefault();
               if (objects[0].Type.Name == "project")
               {
                   menuHost.AddItem("CreateProjectTemplate", "Создание проекта по шаблону", null, 1);
               }
           }
        }

        public void OnMenuItemClick(string itemName)
        {
            if (itemName == CreateProjectTemplate)
            {
                var window = new PureWindow();
                var _createProject = new CreateProject(window);
                _createProject.DataContext = new TreeViewModel(_repository, _selection, _modifier, _fileProvider);
                //var window = new PureWindow { Content = _createProject };
                window.Content = _createProject;
                window.Title = "Мастер создания проекта по шаблону";
                window.ShowDialog();
            }


        }

    }
}
