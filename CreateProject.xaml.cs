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
        //private IEnumerable<IDataObject> _selection;
        //public PureWindow _win;
        //private string nameTypeProject;

        public CreateProject(/*string _nameTypeProject*/)
        {
            //_selection = selection;
            InitializeComponent();
           
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (textBlock2.Text != "")
            {
                if (TC1.Items.Count - 1 == TC1.SelectedIndex)
                {
                    // создание

                    var s = (TreeViewModel)DataContext;
                    //DialogWindow dw = new DialogWindow();//+++
                   // dw.Title = "Создание структуры проекта";
                   // dw.Content = new StageCreate() { DataContext = s };//+++
                   // dw.Show();
                    ((PureWindow)Parent).Content = new StageCreate() { DataContext = s };
                    s.CreateUpProject(false);
                   // ((PureWindow)((UserControl)((Grid)((StackPanel)((Button)sender).Parent).Parent).Parent).Parent).Close();
                 //   ((PureWindow)Parent).Close();
                  //  ((TreeViewModel)DataContext).Dispose();

                }

                if (TC1.Items.Count - 1 > TC1.SelectedIndex)
                {
                    TC1.SelectedIndex += 1;

                }
                if (TC1.Items.Count - 1 == TC1.SelectedIndex)
                {
                    next.Content = "Создать проект";
                    //  ImageNext.Visibility = System.Windows.Visibility.Collapsed;
                    CreateHidden.Visibility = System.Windows.Visibility.Visible;

                    if (((TreeViewModel)DataContext).AllObligatoryAttr(((TreeViewModel)DataContext).AttributesNewProject))
                    {
                        ((TreeViewModel)DataContext).getAllAttributes = true;
                    }
                    else ((TreeViewModel)DataContext).getAllAttributes = false;
                    //   ((TreeViewModel)DataContext).getAllAttributes = false;
                    //  next.IsEnabled = false;
                    //   back.Margin = new Thickness(0, 0, next.Margin.Right + 5 + 105, 10);
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
                //  ImageNext.Visibility = System.Windows.Visibility.Visible;
                ((TreeViewModel)DataContext).getAllAttributes = true;
                //next.IsEnabled = true;
                // next.Content = "Далее";
                //     back.Margin = new Thickness(0, 0, next.Margin.Right + 5 + 75, 10);
            }
            if (TC1.SelectedIndex == 0)
            {
                back.IsEnabled = false;
                back.Visibility = Visibility.Hidden;

                // tblock.Text = "Добро пожаловать в мастер создания проекта по шаблону";
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
                //textBlock2.Visibility = Visibility.Collapsed;
                //textBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewModel)DataContext).Dispose();
                ((PureWindow)Parent).Close();
         //   ((PureWindow)((UserControl)((Grid)((StackPanel)((Button)sender).Parent).Parent).Parent).Parent).Close();
        }

        private void CreateHidden_Click(object sender, RoutedEventArgs e)
        {
            var s = (TreeViewModel)DataContext;
            s.CreateHidden = true;

            if (s.CreateUpProject(false))
            {

                DialogWindow dw = new DialogWindow();//+++
                dw.Title = "Создание структуры проекта завершено";
                dw.Content = new ResultCreation() { DataContext = s };
                dw.Show();

                ((PureWindow)Parent).Close();
            }
            else MessageBox.Show("Заполнены не все обязательные атрибуты");


        }


    }
}
