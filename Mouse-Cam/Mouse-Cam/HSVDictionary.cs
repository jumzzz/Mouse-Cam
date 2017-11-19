using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Hand_Virtual_Mouse
{
    public class HSVDictionary
    {
        private const int DIVISOR = 32; // must be base 2
        private List<string> listRawData = new List<string>();
        private Dictionary<Hsv, int> dataContainer = new Dictionary<Hsv,int>();
        private char[] sep = new char[] { ',' };

        public HSVDictionary(string file) 
        {
            LoadRawData(file);
            InitDictionary();
            string testDebug = "";
        }

        private void InitDictionary() 
        {
            foreach (string s in listRawData) 
            {
                string[] values = s.Split(sep);

                double hue  = Double.Parse(values[0]);
                double sat  = Double.Parse(values[1]);
                double val  = Double.Parse(values[2]);
                int freq = Int32.Parse(values[3]);

                Hsv hsvKey = new Hsv(hue, sat, val);

                dataContainer[hsvKey] = freq;
            }
        }

        private double BringDown(double val, double divisor)
        {
            if (val > 0.0)
            {
                return val - ((int)val & ((int)divisor - 1));
            }
            else
            {
                return 0.0;
            }
        }

        public int GetFreq(Hsv hsv) 
        {
            double scaledHue = BringDown(hsv.Hue, DIVISOR);
            double scaledSat = BringDown(hsv.Satuation, DIVISOR);
            double scaledVal = BringDown(hsv.Value, DIVISOR);

            Hsv hsvKey = (new Hsv(scaledHue, scaledSat, scaledVal));

            if (dataContainer.ContainsKey(hsvKey)) return dataContainer[hsvKey];
            else return 0;
            //return dataContainer[hsvKey];
        }



        private void LoadRawData(string fileInput)
        {
            listRawData = File.ReadAllLines(fileInput).ToList();
        }
    }
}
