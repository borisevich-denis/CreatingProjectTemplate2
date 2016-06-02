using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : UserControl
    {
        

        public string TextSearch
        {
            get;
            set;
        }
        /*public string ResultCatalog
        {
            get { return _ResultCatalog; }
        }*/
        public Catalog()
        {
            InitializeComponent();

        /*    var sri = new StreamResourceInfo();
            var iconByte = ParentCatalog.Type.SvgIcon;

            StreamSvgConverter Isc = new StreamSvgConverter(new WpfDrawingSettings());
            Stream s = new MemoryStream();

            Isc.Convert(new MemoryStream(iconByte), s);
            Bitmap bm = new Bitmap(s);

            image.Source = Imaging.CreateBitmapSourceFromHBitmap(bm.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); ;
*/
            //name.Text = DisplayName;
            var b = Ascon.Pilot.SDK.CreatingProjectTemplate.Properties.Resources.Icon3.ToBitmap();
            ButtonEdit.RightImageSource = Imaging.CreateBitmapSourceFromHBitmap(b.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        private void ButtonEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextSearch = ButtonEdit.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {         
            ((Ascon.Pilot.Theme.Controls.DialogWindow)((UserControl)((Grid)((StackPanel)((Button)sender).Parent).Parent).Parent).Parent).Close();
        }

        
    }
}
