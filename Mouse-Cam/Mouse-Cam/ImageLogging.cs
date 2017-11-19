using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Hand_Virtual_Mouse
{
    public class ImageLogging
    {
        public static void ForTemplateSaving(string dir, Bitmap filtered, Rectangle rect)
        {
            Bitmap filteredCropped = ImgProc.CropImage(filtered, rect);
            filteredCropped.Save(dir);
        }


    }
}
