using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Hand_Virtual_Mouse
{
    public class HSVFreq
    {
        public Hsv HSV { get; set; }
        public int Frequency { get; set; }

        public HSVFreq() { }
    
    }

    public class HSVStat
    {
        private const double DIVISOR = 35.0;
        private List<Hsv> _rawDataSkin = new List<Hsv>();
        private List<Hsv> _rawDataNSkin = new List<Hsv>();

        
        private List<Hsv> _approxHueSkin = new List<Hsv>();
        private List<Hsv> _approxHueNSkin = new List<Hsv>();

        private List<HSVFreq> _listSkinDataFreq = new List<HSVFreq>();
        private List<HSVFreq> _listNSkinDataFreq = new List<HSVFreq>();
        private List<Hsv> _countedSkin = new List<Hsv>();
        private List<Hsv> _countedNSkin = new List<Hsv>();

        private List<HSVFreq> _listSkinSortedHue = new List<HSVFreq>();
        private List<HSVFreq> _listNSkinSortedHue = new List<HSVFreq>();

        private List<double> _doneHueSkin = new List<double>();
        private List<double> _doneHueNSkin = new List<double>();

        private List<HSVFreq> _listSkinSortedSat = new List<HSVFreq>();
        private List<HSVFreq> _listNSkinSortedSat = new List<HSVFreq>(); 

        private List<string> _rawData = new List<string>();
        private char[] _commaSep = new char[] { ',' };

        private static double CalculateHue(double bluePrime, double greenPrime, double redPrime, double cMax, double delta) 
        {
            double hueFinal = 0.0;
            if (delta == 0.0) return hueFinal;
            else if (cMax == redPrime) 
            {
                double x = (greenPrime - bluePrime) / delta;
                double y = x % 6.0;
                hueFinal = 60.0 * y;
                return hueFinal;
            }
            else if (cMax == greenPrime) 
            {
                double x = (bluePrime - redPrime) / delta;
                double y = x + 2.0;
                hueFinal = 60.0 * y;
                return hueFinal;
            }
            else if (cMax == bluePrime) 
            {
                double x = (redPrime - greenPrime) / delta;
                double y = x + 4.0;
                hueFinal = 60.0 * y;
                return hueFinal;
                
            }

            return hueFinal;
        
        }

        private static double CalculateSat(double cMax, double delta) 
        {
            if (cMax == 0.0) return 0.0;
            else
                return delta / cMax;
            
        }
        
        
        public static Hsv BgrToHsV(Bgr bgr) 
        {
            //Hsv hsv = new Hsv();
            double bluePrime = bgr.Blue / 255.0;
            double greenPrime = bgr.Green / 255.0;
            double redPrime = bgr.Red / 255.0;

            List<double> primeColorLst = new List<double>();

            primeColorLst.Add(bluePrime);
            primeColorLst.Add(greenPrime);
            primeColorLst.Add(redPrime);

            double cMax = primeColorLst.Max();
            double cMin = primeColorLst.Min();

            double delta = cMax - cMin;

            double hue = CalculateHue(bluePrime,greenPrime,redPrime,cMax,delta);
            double sat = CalculateSat(cMax, delta);
            double val = cMax;

            return new Hsv(hue, sat, val);
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

        //private double BringDownTest(double num, double divider)
        //{
        //    double result = 0.0;
        //    if (num < 0.0) result = 0.0;
        //    else
        //    {
        //        if (num < divider) num = 0.0;
        //        else
        //        {
        //            int numInt = (int)num;
        //            int offset = numInt % divider;
        //            result = (double)(numInt - offset);
        //        }
        //    }

        //    return result;

        //}

        private void ApproximateHSV() 
        {
            foreach (Hsv hsv in _rawDataSkin) 
            {
                double hueTmp = BringDown(hsv.Hue, DIVISOR);
                double satTmp = BringDown(hsv.Satuation, DIVISOR);
                double valTmp = BringDown(hsv.Value, DIVISOR);

                _approxHueSkin.Add(new Hsv(hueTmp, satTmp, valTmp));
            }

            foreach (Hsv hsv in _rawDataNSkin)
            {
                double hueTmp = BringDown(hsv.Hue, DIVISOR);
                double satTmp = BringDown(hsv.Satuation, DIVISOR );
                double valTmp = BringDown(hsv.Value, DIVISOR );

                _approxHueNSkin.Add(new Hsv(hueTmp, satTmp, valTmp));
            }
        }

        private void CalculateFreq() 
        {
            foreach (Hsv hsv in _approxHueSkin) 
            {
                HSVFreq hsvFreq = new HSVFreq();

                if (!_countedSkin.Contains(hsv)) 
                {
                    _countedSkin.Add(hsv);
                    hsvFreq.HSV = hsv;
                    hsvFreq.Frequency = _approxHueSkin.FindAll(hsv1 => hsv1.Equals(hsv)).Count;
                    _listSkinDataFreq.Add(hsvFreq);
                }
            }

            foreach (Hsv hsv in _approxHueNSkin)
            {
                HSVFreq hsvFreq = new HSVFreq();

                if (!_countedNSkin.Contains(hsv))
                {
                    _countedNSkin.Add(hsv);
                    hsvFreq.HSV = hsv;
                    hsvFreq.Frequency = _approxHueNSkin.FindAll(hsv1 => hsv1.Equals(hsv)).Count;
                    _listNSkinDataFreq.Add(hsvFreq);
                }
            }
        }
        private void SortByHue() 
        {
            _listSkinDataFreq.Sort((x, y) => x.HSV.Hue.CompareTo(y.HSV.Hue));
            _listNSkinDataFreq.Sort((x, y) => x.HSV.Hue.CompareTo(y.HSV.Hue));

            _listSkinSortedHue = _listSkinDataFreq;
            _listNSkinSortedHue = _listNSkinDataFreq;
       
        }

        private void SortBySat() 
        {
            foreach (HSVFreq hsvf in _listSkinSortedHue)
            {
                double hueTmp = hsvf.HSV.Hue;
                if (!_doneHueSkin.Contains(hueTmp))
                {
                    _doneHueSkin.Add(hueTmp);

                    List<HSVFreq> hueListTmp = _listSkinSortedHue.FindAll(hsvf1 =>
                        hsvf1.HSV.Hue.Equals(hueTmp));

                    hueListTmp.Sort((x, y) =>
                        x.HSV.Satuation.CompareTo(y.HSV.Satuation));

                    _listSkinSortedSat.AddRange(hueListTmp);

                }
            }

            foreach (HSVFreq hsvf in _listNSkinSortedHue)
            {
                double hueTmp = hsvf.HSV.Hue;
                if (!_doneHueSkin.Contains(hueTmp))
                {
                    _doneHueSkin.Add(hueTmp);

                    List<HSVFreq> hueListTmp = _listNSkinSortedHue.FindAll(hsvf1 =>
                        hsvf1.HSV.Hue.Equals(hueTmp));

                    hueListTmp.Sort((x, y) =>
                        x.HSV.Satuation.CompareTo(y.HSV.Satuation));

                    _listNSkinSortedSat.AddRange(hueListTmp);

                }
            }        
        }

        public void SaveToFile(string fileSkin, string fileNonSkin)
        {
            List<string> listSkinFile = new List<string>();
            List<string> listNSkinFile = new List<string>();

            foreach (HSVFreq hsvf in _listSkinSortedSat) 
            {
                string s0 = hsvf.HSV.Hue.ToString();
                string s1 = hsvf.HSV.Satuation.ToString();
                string s2 = hsvf.HSV.Value.ToString();
                string s3 = hsvf.Frequency.ToString();
                string c = ",";
                string s = s0 + c + s1 + c + s2 + c + s3;

                listSkinFile.Add(s);
            }

            foreach (HSVFreq hsvf in _listNSkinSortedSat)
            {
                string s0 = hsvf.HSV.Hue.ToString();
                string s1 = hsvf.HSV.Satuation.ToString();
                string s2 = hsvf.HSV.Value.ToString();
                string s3 = hsvf.Frequency.ToString();
                string c = ",";
                string s = s0 + c + s1 + c + s2 + c + s3;

                listNSkinFile.Add(s);
            }

            File.WriteAllLines(fileSkin, listSkinFile.ToArray());
            File.WriteAllLines(fileNonSkin, listNSkinFile.ToArray());
           
        }

        public HSVStat(string file) 
        {
            _rawData = File.ReadAllLines(file).ToList();

            foreach (string lineStr in _rawData) 
            {
                string[] lineData = lineStr.Split(_commaSep);
                double hue = 0.0;
                double sat = 0.0;
                double val = 0.0;
                int skinNsIndicator = 0;

                Double.TryParse(lineData[0], out hue);
                Double.TryParse(lineData[1], out sat);
                Double.TryParse(lineData[2], out val);
                Int32.TryParse(lineData[3], out skinNsIndicator);

                if (skinNsIndicator == 1)
                {
                    Hsv hsvTmp = new Hsv(hue, sat, val);
                    _rawDataSkin.Add(hsvTmp);
                
                }
                else if (skinNsIndicator == 2) 
                {
                    Bgr bgrTmp = new Bgr(hue, sat, val);
                    Hsv hsvTmp = new Hsv(hue, sat, val);

                    _rawDataNSkin.Add(hsvTmp);
  
                }
                
            }

            ApproximateHSV();
            CalculateFreq();
            SortByHue();
            SortBySat();
            
        }
    }
}
