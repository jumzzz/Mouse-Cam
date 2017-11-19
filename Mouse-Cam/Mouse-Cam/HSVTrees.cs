using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Hand_Virtual_Mouse
{

    public class BGRTree 
    {
        private const int DIVIDER = 20;
        public List<BlueTree> subTreesBlue = new List<BlueTree>();

        private List<string> listRawData = new List<string>();
        private char[] commaSep = new char[] { ',' };

        private void LoadRawData(string fileInput) 
        {
            listRawData = File.ReadAllLines(fileInput).ToList();
        }

        private void PopulateTree(double blue, double green, double red, int frequency)
        {
            if (subTreesBlue.Exists(bt => bt.Intensity == blue))
            {
                int indexBlue = subTreesBlue.FindIndex(bt => bt.Intensity == blue);

                // next find if the specific green exists in a subtree of blue
                // such that Gn | Bn

                if (subTreesBlue[indexBlue].subTreesGreen.
                    Exists(gt => gt.Intensity == green))
                {
                    int indexGreen = subTreesBlue[indexBlue].subTreesGreen.FindIndex(gt => gt.Intensity == green);
                    RedFIP pairRedFI = new RedFIP();
                    pairRedFI.Intensity = red;
                    pairRedFI.Frequency = frequency;
                    subTreesBlue[indexBlue].subTreesGreen[indexGreen].redNodes.Add(pairRedFI);

                }
                else
                {
                    GreenTree gt = new GreenTree();
                    gt.Intensity = green;
                    RedFIP pairRedFI = new RedFIP();
                    pairRedFI.Intensity = red;
                    pairRedFI.Frequency = frequency;

                    gt.redNodes.Add(pairRedFI);
                    subTreesBlue[indexBlue].subTreesGreen.Add(gt);


                }

            }
            else
            {
                BlueTree bt = new BlueTree();
                GreenTree gt = new GreenTree();
                RedFIP pairRedFI = new RedFIP();

                pairRedFI.Intensity = red;
                pairRedFI.Frequency = frequency;

                gt.Intensity = green;
                bt.Intensity = blue;

                gt.redNodes.Add(pairRedFI);
                bt.subTreesGreen.Add(gt);
                subTreesBlue.Add(bt);

            }
        }

        private void CalculateProb() 
        {
            int freqSumTot = 0;
            // get total sum of frequencies
            foreach (BlueTree bt in subTreesBlue) 
                foreach (GreenTree gt in bt.subTreesGreen) 
                    foreach (RedFIP rfip in gt.redNodes) 
                        freqSumTot += rfip.Frequency;
                    
            
            // get prob each
            foreach (BlueTree bt in subTreesBlue)
                foreach (GreenTree gt in bt.subTreesGreen)
                    foreach (RedFIP rfip in gt.redNodes)
                        rfip.Probabilty = (double)rfip.Frequency / (double)freqSumTot;
 
        }

        private void InitTrees() 
        {
            foreach (string rawStr in listRawData) 
            {
                string[] lineData = rawStr.Split(commaSep);

                double blue = 0.0;
                double green = 0.0;
                double red = 0.0;
                int frequency = 0;

                Double.TryParse(lineData[0], out blue);
                Double.TryParse(lineData[1], out green);
                Double.TryParse(lineData[2], out red);
                Int32.TryParse(lineData[3], out frequency);

                PopulateTree(blue, green, red, frequency);

            }

            CalculateProb();
        
        }

        public BGRTree(string fileInput) 
        {
            LoadRawData(fileInput);
            InitTrees();
            listRawData.Clear();
        }
        /// <summary>
        /// Find the index of value containing blue key
        /// using Binary Search.
        /// </summary>
        /// <param name="blue"></param>
        /// <returns></returns>
        private int BinSearchBlue(double blueKey) 
        {
            int low = 0;
            int high = subTreesBlue.Count - 1;

            while (low <= high) 
            {
                int mid = low + high >> 1;
                BlueTree midVal = subTreesBlue[mid];

                if (midVal.Intensity < blueKey)
                    low = mid + 1;
                else if (midVal.Intensity > blueKey)
                    high = mid - 1;
                else
                    return mid;
            }
            return -1;
        }

        private int BinSearchGreen(int blueIndex, double greenKey) 
        {
            int low = 0;
            int high = subTreesBlue[blueIndex].subTreesGreen.Count - 1;

            while (low <= high)
            {
                int mid = low + high >> 1;
                GreenTree midVal = subTreesBlue[blueIndex].
                    subTreesGreen[mid];

                if (midVal.Intensity < greenKey)
                    low = mid + 1;
                else if (midVal.Intensity > greenKey)
                    high = mid - 1;
                else
                    return mid;
            }
            return -1;
        }

        private double BringDown(double num)
        {
            double result = 0.0;
            if (num < 0.0) result = 0.0;
            else
            {
                if (num < 10.0) num = 0.0;
                else
                {
                    int numInt = (int)num;
                    int offset = numInt % DIVIDER;
                    result = (double)(numInt - offset);
                }
            }

            return result;

        }

        public int GetFreq(Bgr bgr) 
        {
            int result = 0;
            double blue = BringDown(bgr.Blue);
            double green = BringDown(bgr.Green);
            double red = BringDown(bgr.Red);

            int blueIndex = BinSearchBlue(blue);

            if (blueIndex < 0) return result;
            else 
            {
                int greenIndex = BinSearchGreen(blueIndex, green);

                if (greenIndex < 0) return result;
                else 
                {
                    RedFIP redFIP = subTreesBlue[blueIndex].
                                          subTreesGreen[greenIndex].
                                          redNodes.Find(prfi => prfi.Intensity == red);

                    if (redFIP == null) return result;
                    else 
                        return redFIP.Frequency;
                }

            }

        }

        public double GetProb(Bgr bgr) 
        {
            double result = 0.0;
            double blue = BringDown(bgr.Blue);
            double green = BringDown(bgr.Green);
            double red = BringDown(bgr.Red);

            int blueIndex = BinSearchBlue(blue);

            if (blueIndex < 0) return result;
            else
            {
                int greenIndex = BinSearchGreen(blueIndex, green);

                if (greenIndex < 0) return result;
                else
                {
                    RedFIP redFIP = subTreesBlue[blueIndex].
                                          subTreesGreen[greenIndex].
                                          redNodes.Find(prfi => prfi.Intensity == red);

                    if (redFIP == null) return result;
                    else
                        return redFIP.Probabilty;
                }

            }
        
        }

        
    }
    public class BlueTree 
    {
        public double Intensity{ get; set;}
        public List<GreenTree> subTreesGreen = new List<GreenTree>();
        public BlueTree()
        {

        
        }
    }
    public class GreenTree
    {
        public double Intensity { get; set; }
        public List<RedFIP> redNodes = new List<RedFIP>();
        public GreenTree()
        {

        }
    }
    public class RedFIP
    {
        public double Intensity{ get; set;}
        public int Frequency{ get; set;}

        public double Probabilty { get; set; }

        public RedFIP(){}
    }
    // checking hsv in terms of histogram detection is pretty similar algorithmically
    // now...
    public class HueFreq 
    {
        public double Hue { get; set; }
        public int Freq { get; set; }
    }
    public class HSVTree 
    {
        private List<double> _lookUpTable = new List<double>();

        private double DIVISOR = 32.0;

        public List<HueTree> subTreesHue = new List<HueTree>();

        private List<string> listRawData = new List<string>();
        private char[] commaSep = new char[] { ',' };



        public HSVTree(string fileInput)
        {
            LoadRawData(fileInput);
            InitTrees();
            listRawData.Clear();
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

        
        private void LoadRawData(string fileInput) 
        {
            listRawData = File.ReadAllLines(fileInput).ToList();
        }

        private void PopulateTree(double hue, double saturation, double value, int frequency)
        {
            if (subTreesHue.Exists(bt => bt.Magnitude == hue))
            {
                int indexHue = subTreesHue.FindIndex(bt => bt.Magnitude == hue);

                // next find if the specific green exists in a subtree of blue
                // such that Gn | Bn

                if (subTreesHue[indexHue].subTreesSaturation.
                    Exists(gt => gt.Magnitude == saturation))
                {
                    int indexSat = subTreesHue[indexHue].subTreesSaturation.FindIndex(gt => gt.Magnitude == saturation);
                    ValueFIP valueFIP = new ValueFIP();
                    valueFIP.Magnitude = value;
                    valueFIP.Frequency = frequency;
                    subTreesHue[indexHue].subTreesSaturation[indexSat].valueNodes.Add(valueFIP);

                }
                else
                {
                    SatTree st = new SatTree();
                    st.Magnitude = saturation;
                    ValueFIP valueFIP = new ValueFIP();
                    valueFIP.Magnitude = value;
                    valueFIP.Frequency = frequency;

                    st.valueNodes.Add(valueFIP);
                    subTreesHue[indexHue].subTreesSaturation.Add(st);


                }

            }
            else
            {
                HueTree ht = new HueTree();
                SatTree st = new SatTree();
                ValueFIP valueFIP = new ValueFIP();

                valueFIP.Magnitude = value;
                valueFIP.Frequency = frequency;

                st.Magnitude = saturation;
                ht.Magnitude = hue;

                st.valueNodes.Add(valueFIP);
                ht.subTreesSaturation.Add(st);
                subTreesHue.Add(ht);

            }
        }

        private void CalculateProb() 
        {
            int freqSumTot = 0;
            // get total sum of frequencies
            foreach (HueTree ht in subTreesHue) 
                foreach (SatTree st in ht.subTreesSaturation) 
                    foreach (ValueFIP vfip in st.valueNodes) 
                        freqSumTot += vfip.Frequency;
                    
            
            // get prob each
            foreach (HueTree ht in subTreesHue)
                foreach (SatTree st in ht.subTreesSaturation)
                    foreach (ValueFIP vfip in st.valueNodes)
                        vfip.Probabilty = (double)vfip.Frequency / (double)freqSumTot;
 
        }

        private void InitTrees() 
        {
            foreach (string rawStr in listRawData) 
            {
                string[] lineData = rawStr.Split(commaSep);

                double hue = 0.0;
                double sat = 0.0;
                double value = 0.0;
                int frequency = 0;

                Double.TryParse(lineData[0], out hue);
                Double.TryParse(lineData[1], out sat);
                Double.TryParse(lineData[2], out value);
                Int32.TryParse(lineData[3], out frequency);

                PopulateTree(hue, sat, value, frequency);

            }

            //Mat mat = new Mat();
            //mat.

            CalculateProb();
        
        }

  
        /// <summary>
        /// Find the index of value containing blue key
        /// using Binary Search.
        /// </summary>
        /// <param name="blue"></param>
        /// <returns></returns>
        private int BinSearchHue(double hueKey) 
        {
            int low = 0;
            int high = subTreesHue.Count - 1;

            while (low <= high) 
            {
                int mid = low + high >> 1;
                HueTree midVal = subTreesHue[mid];

                if (midVal.Magnitude < hueKey)
                    low = mid + 1;
                else if (midVal.Magnitude > hueKey)
                    high = mid - 1;
                else
                    return mid;
            }
            return -1;
        }

        private int BinSearchSaturation(int hueIndex, double satKey) 
        {
            int low = 0;
            int high = subTreesHue[hueIndex].subTreesSaturation.Count - 1;

            while (low <= high)
            {
                int mid = low + high >> 1;
                SatTree midVal = subTreesHue[hueIndex].
                    subTreesSaturation[mid];

                if (midVal.Magnitude < satKey)
                    low = mid + 1;
                else if (midVal.Magnitude > satKey)
                    high = mid - 1;
                else
                    return mid;
            }
            return -1;
        }

        //private double BringDown(double num)
        //{
        //    double result = 0.0;
        //    if (num < 0.0) result = 0.0;
        //    else
        //    {
        //        if (num < 10.0) num = 0.0;
        //        else
        //        {
        //            int numInt = (int)num;
        //            int offset = numInt % 10;
        //            result = (double)(numInt - offset);
        //        }
        //    }

        //    return result;

        //}

        private double Modulo(double dividend, double divisor)
        {
            int x = (int)dividend & ((int)divisor - 1);

            return (double)x;

        }


        public int GetFreq(Hsv hsv) 
        {
            int result = 0;
            double hue = BringDown(hsv.Hue, DIVISOR);
            double saturation = BringDown(hsv.Satuation, DIVISOR);
            double value = BringDown(hsv.Value, DIVISOR);

            int hueIndex = BinSearchHue(hue);

            if (hueIndex < 0) return result;
            else 
            {
                int saturationIndex = BinSearchSaturation(hueIndex, saturation);

                if (saturationIndex < 0) return result;
                else 
                {
                    ValueFIP valueFIP = subTreesHue[hueIndex].
                                          subTreesSaturation[saturationIndex].
                                          valueNodes.Find(prfi => prfi.Magnitude == value);

                    if (valueFIP == null) return result;
                    else 
                        return valueFIP.Frequency;
                }

            }

        }

        public double GetProb(Hsv hsv) 
        {
            
            double result = 0.0;
            double hue = BringDown(hsv.Hue, DIVISOR);
            double saturation = BringDown(hsv.Satuation,  DIVISOR);
            double value = BringDown(hsv.Value, DIVISOR);

            int blueIndex = BinSearchHue(hue);

            if (blueIndex < 0) return result;
            else
            {
                int greenIndex = BinSearchSaturation(blueIndex, saturation);

                if (greenIndex < 0) return result;
                else
                {
                    ValueFIP valueFIP = subTreesHue[blueIndex].
                                          subTreesSaturation[greenIndex].
                                          valueNodes.Find(prfi => prfi.Magnitude == value);

                    if (valueFIP == null) return result;
                    else
                        return valueFIP.Probabilty;
                }

            }
        
        }

    
    }

   
    public class HueTree 
    {
        public double Magnitude{ get; set;}
        public List<SatTree> subTreesSaturation = new List<SatTree>();
        public HueTree()
        {

        }
    }
    public class SatTree 
    {
        public double Magnitude { get; set; }
        public List<ValueFIP> valueNodes = new List<ValueFIP>();
        public SatTree()
        {

        }
    }
    public class ValueFIP 
    {
        public double Magnitude{ get; set;}
        public int Frequency{ get; set;}

        public double Probabilty { get; set; }

        public ValueFIP(){}
    
    }
   
}
