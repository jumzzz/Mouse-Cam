using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace Hand_Virtual_Mouse
{
    public class Memory
    {
        public static void ReleaseData(ref Capture cap)
        {
            if (cap != null)
                cap.Dispose();
        }
    }
}
