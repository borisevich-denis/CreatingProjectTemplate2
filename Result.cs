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
        private ImageSource _Icon;

        public ImageSource Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
            }
        }
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

                using (Stream s = new MemoryStream())
                {

                    StreamSvgConverter Isc = new StreamSvgConverter(new WpfDrawingSettings());

                    Bitmap bm = null;
                    Isc.Convert(new MemoryStream(typeObj.SvgIcon), s);
                    bm = new Bitmap(s);
                    Isc.Dispose();
                    Icon = Imaging.CreateBitmapSourceFromHBitmap(bm.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    //bm.Dispose();
                }
            }
            else
            {
                Icon = null;
            }
            Text = text;
        }
    }
}
