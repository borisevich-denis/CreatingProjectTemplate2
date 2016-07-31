using Ascon.Pilot.Theme.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class CreateProject : UserControl
    {

        public CreateProject()
        {
            InitializeComponent();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (textBlock2.Text != "")
            {
                if (TC1.Items.Count - 1 == TC1.SelectedIndex)
                {
                    var s = (TreeViewModel)DataContext;
                    ((PureWindow)Parent).Content = new StageCreate() { DataContext = s };
                    s.CreateUpProject(false);
                }

                if (TC1.Items.Count - 1 > TC1.SelectedIndex)
                {
                    TC1.SelectedIndex += 1;
                }
                if (TC1.Items.Count - 1 == TC1.SelectedIndex)
                {
                    next.Content = "Создать проект";
                    CreateHidden.Visibility = System.Windows.Visibility.Visible;

                    if (((TreeViewModel)DataContext).AllObligatoryAttr(((TreeViewModel)DataContext).AttributesNewProject))
                    {
                        ((TreeViewModel)DataContext).getAllAttributes = true;
                    }
                    else ((TreeViewModel)DataContext).getAllAttributes = false;
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
                CreateHidden.Visibility = System.Windows.Visibility.Hidden;
                ((TreeViewModel)DataContext).getAllAttributes = true;
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
                textBlock1.Text = "Шаг 1 из 4. Выберите проект для использования в качестве шаблона";
            }
            else if (TC1.SelectedIndex == 1)
            {
                textBlock1.Text = "Шаг 2 из 4. Выберите необходимые элементы состава проекта";
            }
            else if (TC1.SelectedIndex == 2)
            {
                textBlock1.Text = "Шаг 3 из 4. Выберите необходимые папки и файлы проекта";
                textBlock2.Visibility = Visibility.Visible;
                textBlock.Visibility = Visibility.Visible;
            }
            else if (TC1.SelectedIndex == 3)
            {
                textBlock1.Text = "Шаг 4 из 4. Заполните атрибутную карточку нового проекта";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((PureWindow)Parent).Close();
        }

        private void CreateHidden_Click(object sender, RoutedEventArgs e)
        {
            var s = (TreeViewModel)DataContext;
            s.CreateHidden = true;

            if (s.CreateUpProject(false))
            {
                DialogWindow dw = new DialogWindow();
                dw.Title = "Создание структуры проекта завершено";
                dw.Content = new ResultCreation() { DataContext = s };
                dw.Show();

                ((PureWindow)Parent).Close();
            }
            else MessageBox.Show("Заполнены не все обязательные атрибуты");
        }
    }
}
