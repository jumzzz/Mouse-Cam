using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Hand_Virtual_Mouse
{
    class MouseEvent
    {
    }

    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public class MousePoint
    {
        public event ChangedEventHandler Changed;
        private Point _mousePoint = new Point();
        public Point MouseLoc
        {
            get 
            {
                return _mousePoint;
            }
            set
            {
                if(!_mousePoint.Equals(value)) OnChanged(EventArgs.Empty);
                _mousePoint = value;
            }
        }

        protected virtual void OnChanged(EventArgs e) 
        {
            if (Changed != null)
                Changed(this, e);
        }
        public MousePoint(Point point) 
        {
            MouseLoc = point;
        }
    }
}
