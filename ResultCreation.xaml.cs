using Ascon.Pilot.Theme.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    /// <summary>
    /// Логика взаимодействия для ResultCreation.xaml
    /// </summary>
    public partial class ResultCreation : UserControl
    {
        public ResultCreation()
        {
            InitializeComponent();
            Loaded += ResultCreation_Loaded;
            
        }

        void ResultCreation_Loaded(object sender, RoutedEventArgs e)
        {
           /* foreach (var rc in ((TreeViewModel)DataContext).resultCreation)
            {
                rbt.Items.Add(rc);  
                
            }       */    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((DialogWindow)((UserControl)((Grid)((StackPanel)((Button)sender).Parent).Parent).Parent).Parent).Close();
        }

       

    }
}
