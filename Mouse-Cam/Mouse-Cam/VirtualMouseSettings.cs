using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hand_Virtual_Mouse
{
    public class VirtualMouseSettings
    {    
        public int WidthRatio { get; set; }          // Ratio between ROI/Main width
        public int HeightRatio { get; set; }         // Ratio between ROI/Main height 

        public VirtualMouseSettings() { }
    }
}
