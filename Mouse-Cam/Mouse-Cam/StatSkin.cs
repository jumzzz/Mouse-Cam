using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Hand_Virtual_Mouse
{
    public class PropPixel
    {
        private const int DIVIDER = 20;
        private char[] sep = new char[] { ',' };
        public Bgr BgrPix { get; set; }
        public bool IsSkin { get; set; }

        public PropPixel() { }

        private double BringDown(double num)
        {
            double result = 0.0;
            if (num < 0.0) result = 0.0;
            else
            {
                if (num < DIVIDER) num = 0.0;
                else
                {
                    int numInt = (int)num;
                    int offset = numInt % DIVIDER;
                    result = (double)(numInt - offset);
                }
            }

            return result;

        }

        public PropPixel(string lineData) 
        {
           // add conversion code here
            string[] data = lineData.Split(sep);

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;
            Double.TryParse(data[0], out blue);
            Double.TryParse(data[1], out green);
            Double.TryParse(data[2], out red);
            
            BgrPix = new Bgr(BringDown(blue), BringDown(green), BringDown(red));

            if (data[3].Equals("1")) IsSkin = true;
            else IsSkin = false;
        }

        public PropPixel(string lineData, bool skin) : this(lineData)
        {
            IsSkin = skin;
        }
            
    }

    public class StatPixelSkin 
    {
        public PropPixel PixelInfo { get; set; }
        public int Frequency { get; set; }

        private char[] sep = new char[] { ',' };
        public StatPixelSkin() 
        {
            
        }

        public StatPixelSkin(string lineData, bool skin) 
        {
            PixelInfo = new PropPixel(lineData, skin);
            string[] lineDataArr = lineData.Split(sep);
            int freq = 0;
            Int32.TryParse(lineDataArr[3], out freq);
            Frequency = freq;
    
        }

    }

    // contains methods and fields necessary for accumulating the data
    public class SkinDataSet 
    {
        private List<PropPixel> _countedPixel = new List<PropPixel>();
        private List<PropPixel> _listRawPixel = new List<PropPixel>(); 
        
        private List<StatPixelSkin> _listSkinUS = new List<StatPixelSkin>();
        private List<StatPixelSkin> _listNonSkinUS = new List<StatPixelSkin>();

        private List<StatPixelSkin> _listSkinBlueSort = new List<StatPixelSkin>();
        private List<StatPixelSkin> _listNonSkinBlueSort = new List<StatPixelSkin>();

        private List<StatPixelSkin> _listSkinFinalSort = new List<StatPixelSkin>();
        private List<StatPixelSkin> _listNonSkinFinalSort = new List<StatPixelSkin>();

        private List<StatPixelSkin> _listSkinRedSort = new List<StatPixelSkin>();
        private List<StatPixelSkin> _listNonSkinRedSort = new List<StatPixelSkin>();


        private List<double> _doneBlueSkin = new List<double>();
        private List<double> _doneGreenSkin = new List<double>();
        //private List<double> _doneRedSkin = new List<double>();

        private List<double> _doneBlueNonSkin = new List<double>();
        private List<double> _doneGreenNonSkin = new List<double>();
  //      private List<double> _doneRedNonSkin = new List<double>();

        private const string HIST_SKIN_FILE = "data\\histogram\\histskindata.csv";
        private const string HIST_NONSKIN_FILE = "data\\histogram\\histnonskindata.csv";
        private char[] sep = new char[] { ',' };

        public SkinDataSet(string fileInput) 
        {
           List<string> lineList = File.ReadAllLines(fileInput).ToList();

           lineList.ForEach(data => _listRawPixel.Add(new PropPixel(data)));

           lineList.Clear();
           UpdateListStat();
     //      ListStatistics.OrderBy(sps => sps.PixelInfo.BgrPix);
           
           
        }

        private void UpdateListStat() 
        {
            foreach (PropPixel pp in _listRawPixel) 
            {
                if (!_countedPixel.Exists(cp => cp.BgrPix.Equals(pp.BgrPix))) 
                {
                    StatPixelSkin sps = new StatPixelSkin();
                    sps.PixelInfo = pp;
                    sps.Frequency = _listRawPixel.FindAll(
                        propPixel => propPixel.BgrPix.Equals(pp.BgrPix)).Count;

                    if (sps.PixelInfo.IsSkin) _listSkinUS.Add(sps);
                    else _listNonSkinUS.Add(sps);

                    _countedPixel.Add(pp);

                   // ListStatistics.Add(sps);
                }         
            }

            SortListByBlue();
            SortListByGreen();
            SortListByRed();

            Console.WriteLine("Put debugger here");
        }

        private void SortListByBlue() 
        {
            _listSkinUS.Sort((x, y) => 
                x.PixelInfo.BgrPix.Blue.CompareTo(y.PixelInfo.BgrPix.Blue));
            _listNonSkinUS.Sort((x, y) =>
                x.PixelInfo.BgrPix.Blue.CompareTo(y.PixelInfo.BgrPix.Blue));

            _listSkinBlueSort = _listSkinUS;
            _listNonSkinBlueSort = _listNonSkinUS;
            
        }

        private void SortListByGreen() 
        {
            foreach (StatPixelSkin sps in _listSkinBlueSort) 
            {
                double blueTmp = sps.PixelInfo.BgrPix.Blue;
                if (!_doneBlueSkin.Contains(blueTmp)) 
                {
                    _doneBlueSkin.Add(blueTmp);

                    List<StatPixelSkin> greenListTmp = _listSkinBlueSort.FindAll(sps1 => 
                        sps1.PixelInfo.BgrPix.Blue.Equals(blueTmp));

                    greenListTmp.Sort((x, y) =>
                        x.PixelInfo.BgrPix.Green.CompareTo(y.PixelInfo.BgrPix.Green));

                    _listSkinFinalSort.AddRange(greenListTmp);
    
                }
            }

            foreach (StatPixelSkin sps in _listNonSkinBlueSort)
            {
                double blueTmp = sps.PixelInfo.BgrPix.Blue;
                if (!_doneBlueNonSkin.Contains(blueTmp))
                {
                    _doneBlueNonSkin.Add(blueTmp);

                    List<StatPixelSkin> greenListTmp = _listNonSkinBlueSort.FindAll(sps1 =>
                        sps1.PixelInfo.BgrPix.Blue.Equals(blueTmp));

                    greenListTmp.Sort((x, y) =>
                        x.PixelInfo.BgrPix.Green.CompareTo(y.PixelInfo.BgrPix.Green));

                    _listNonSkinFinalSort.AddRange(greenListTmp);

                }
            }
        }


        private void SortListByRed() 
        {
            foreach (StatPixelSkin sps in _listSkinFinalSort)
            {
                double greenTmp = sps.PixelInfo.BgrPix.Green;
                if (!_doneGreenSkin.Contains(greenTmp))
                {
                    _doneGreenSkin.Add(greenTmp);

                    List<StatPixelSkin> redListTmp = _listSkinFinalSort.FindAll(sps1 =>
                        sps1.PixelInfo.BgrPix.Green.Equals(greenTmp));

                    redListTmp.Sort((x, y) =>
                        x.PixelInfo.BgrPix.Red.CompareTo(y.PixelInfo.BgrPix.Red));

                    _listSkinRedSort.AddRange(redListTmp);

                }
            }

            foreach (StatPixelSkin sps in _listNonSkinFinalSort)
            {
                double greenTmp = sps.PixelInfo.BgrPix.Green;
                if (!_doneGreenNonSkin.Contains(greenTmp))
                {
                    _doneGreenNonSkin.Add(greenTmp);

                    List<StatPixelSkin> redListTmp = _listNonSkinFinalSort.FindAll(sps1 =>
                        sps1.PixelInfo.BgrPix.Green.Equals(greenTmp));

                    redListTmp.Sort((x, y) =>
                        x.PixelInfo.BgrPix.Red.CompareTo(y.PixelInfo.BgrPix.Red));

                    _listNonSkinRedSort.AddRange(redListTmp);

                }
            }
        
        }


        public void SaveToFile() 
        {
            List<string> skinData = new List<string>();
            List<string> nonSkinData = new List<string>();
            
            foreach (StatPixelSkin sps in _listSkinFinalSort) 
            {
                string s0 = sps.PixelInfo.BgrPix.Blue.ToString();
                string s1 = sps.PixelInfo.BgrPix.Green.ToString();
                string s2 = sps.PixelInfo.BgrPix.Red.ToString();
                string s3 = sps.Frequency.ToString();

                string lineDataSkin = s0 + "," + s1 + "," + s2 + "," + s3;
                skinData.Add(lineDataSkin);
            }

            foreach (StatPixelSkin sps in _listNonSkinFinalSort)
            {
                string s0 = sps.PixelInfo.BgrPix.Blue.ToString();
                string s1 = sps.PixelInfo.BgrPix.Green.ToString();
                string s2 = sps.PixelInfo.BgrPix.Red.ToString();
                string s3 = sps.Frequency.ToString();

                string lineDataSkin = s0 + "," + s1 + "," + s2 + "," + s3;
                nonSkinData.Add(lineDataSkin);
            }

            File.WriteAllLines(HIST_SKIN_FILE, skinData.ToArray());
            File.WriteAllLines(HIST_NONSKIN_FILE, nonSkinData.ToArray());
        }

        //private void LoadData() 
        //{
        //    List<string> skinData = File.ReadAllLines(HIST_SKIN_FILE).ToList();
        //    List<string> nonSkinData = File.ReadAllLines(HIST_NONSKIN_FILE).ToList();

        //    // start parsing skindata

        //    //foreach (string skinStr in skinData) 
        //    //{
        //    //    StatPixelSkin spsTmp = new StatPixelSkin(skinStr, true);
                
        //    //}

        //    skinData.ForEach(skinStr => 
        //        _listSkinFinalSort.Add(new StatPixelSkin(skinStr, true)));

        //    nonSkinData.ForEach(nSkinStr =>
        //        _listNonSkinFinalSort.Add(new StatPixelSkin(nSkinStr, false)));
        //}


    }
}
    