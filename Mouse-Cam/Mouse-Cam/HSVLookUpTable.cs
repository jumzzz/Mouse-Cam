using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Emgu.CV.Structure;

namespace Hand_Virtual_Mouse
{

    public class Val
    {
        //public ushort Val{get;set;}
        public ushort Freq{get;set;}

        public Val()
        {
            //Val = 0;
            Freq = 0;  
        }
    }
    public class Sat 
    {
        public Val[] Val = new Val[255];
        public void InitVal() 
        {
            for (int i = 0; i < Val.Length; i++)
                Val[i] = new Val();
        }

        public Sat() 
        {
            InitVal();

        } 
    }
    public class Hue 
    {
        private const short DIVISOR = 32;
        public Sat[] Sat = new Sat[255];

        public void InitSat()
        {
            for (int i = 0; i < Sat.Length; i++)
                Sat[i] = new Sat();
        }

        public Hue() 
        {
            InitSat();
        }
    }
    public class HSVLookUpTable
    {
        private const double DIVISOR = 35.0; 
        //public Hue[] Hue = new Hue[255];
        private List<string> _dataRaw = new List<string>();
        private char[] comma = new char[] { ',' };

        private ushort[, ,] _hsvTable = new ushort[255, 255, 255];

        private ushort[] _bringDownTable = new ushort[500];

        private void LoadBringDownTable() 
        {
            for (int i = 0; i < _bringDownTable.Length; i++) 
                _bringDownTable[i] = (ushort)BringDown(i, DIVISOR);
            
        }

        public HSVLookUpTable(string dir) 
        {
            LoadRawData(dir);
            OrganizeData();
            LoadBringDownTable();
            _dataRaw.Clear();
        }

        private void LoadRawData(string dir) 
        {
            _dataRaw = File.ReadAllLines(dir).ToList();
        }

        private void OrganizeData() 
        {
            foreach (string dataLine in _dataRaw) 
            {
                string[] dataStr = dataLine.Split(comma);

                ushort indexHue  = UInt16.Parse(dataStr[0]);
                ushort indexSat  = UInt16.Parse(dataStr[1]);
                ushort indexVal  = UInt16.Parse(dataStr[2]);
                ushort freq = UInt16.Parse(dataStr[3]);

                _hsvTable[indexHue, indexSat, indexVal] = freq;

            }
        }


        private double BringDown(double val, double divisor)
        {
            if (val <= 0.0) return 0.0;
            else
            {
                double offset = val % divisor;
                return val - offset;
            }
        }

        //private double BringDown(double val, double divisor)
        //{
        //    if (val > 0.0)
        //    {
        //        return val - ((int)val & ((int)divisor - 1));
        //    }
        //    else
        //    {
        //        return 0.0;
        //    }
        //}

        public int GetFreq(Hsv hsv) 
        {
            //ushort hue = (ushort)hsv.Hue;
            //ushort sat = (ushort)hsv.Hue;
            //ushort val = (ushort)hsv.Hue;

            ushort indexHue = _bringDownTable[(ushort)hsv.Hue];
            ushort indexSat = _bringDownTable[(ushort)hsv.Satuation];
            ushort indexVal = _bringDownTable[(ushort)hsv.Value];

            //return Hue[indexHue].Sat[indexSat].Val[indexVal].Freq;
            return _hsvTable[indexHue, indexSat, indexVal];

        }


    }
}
