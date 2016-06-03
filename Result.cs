using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    class Result 
    {
        public ImageSource Icon { get; set; }
        public string Text { get; set; }
        public Result(IType typeObj, string text)
        {
            if (typeObj != null) 
            {
                if (typeObj.Name == "File")
                {
                    Icon = Ascon.Pilot.Theme.Icons.Instance.FileIcon;
                    Text = text;
                    return;
                }
                StreamSvgConverter Isc = new StreamSvgConverter(new WpfDrawingSettings());
                Stream s = new MemoryStream();

                Isc.Convert(new MemoryStream(typeObj.SvgIcon), s);
                Bitmap bm = new Bitmap(s);

                Icon = Imaging.CreateBitmapSourceFromHBitmap(bm.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else 
            { 
                Icon = null; 
            }
            Text = text;
        }
    }
}
