using Ascon.Pilot.Theme.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    /// <summary>
    /// Логика взаимодействия для CreateProjectStructure.xaml
    /// </summary>
    public partial class CreateProjectStructure : UserControl
    {
        public CreateProjectStructure()
        {
            InitializeComponent();
        }
        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (textBlock2.Text != "")
            {
                if (TC1.Items.Count - 1 == TC1.SelectedIndex)
                {
                    var s = ((TreeViewModel)DataContext);
                    ((PureWindow)Parent).Content = new StageCreate() { DataContext = s };
                    s.CreateUpProject(true);
                }
                if (TC1.Items.Count - 1 > TC1.SelectedIndex)
                {
                    TC1.SelectedIndex += 1;

                }
                if (TC1.Items.Count - 1 == TC1.SelectedIndex)
                {
                    next.Content = "Создать структуру проекта";
                }
                if (TC1.SelectedIndex > 0)
                {
                    back.IsEnabled = true;
                    back.Visibility = Visibility.Visible;
                }
            }
            else MessageBox.Show("Необходимо выбрать проект");
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (TC1.SelectedIndex > 0)
            {
                TC1.SelectedIndex -= 1;
            }
            if (TC1.Items.Count - 1 > TC1.SelectedIndex)
            {
                next.Content = "Далее >";
            }
            if (TC1.SelectedIndex == 0)
            {
                back.IsEnabled = false;
                back.Visibility = Visibility.Hidden;
            }
        }

        private void TC1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TC1.SelectedIndex == 0)
            {
                textBlock1.Text = "Шаг 1 из 3. Выберите проект для использования в качестве шаблона";
            }
            else if (TC1.SelectedIndex == 1)
            {
                textBlock1.Text = "Шаг 2 из 3. Выберите необходимые элементы состава проекта";
            }
            else if (TC1.SelectedIndex == 2)
            {
                textBlock1.Text = "Шаг 3 из 3. Выберите необходимые папки и файлы проекта";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            ((PureWindow)Parent).Close();
        }

    }
}
