using Emgu.CV;

namespace Hand_Virtual_Mouse
{
    public class Memory
    {
        public static void ReleaseData(ref VideoCapture cap)
        {
            if (cap != null)
                cap.Dispose();
        }
    }
}
