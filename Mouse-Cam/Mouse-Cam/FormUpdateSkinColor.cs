using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Cuda;

namespace Hand_Virtual_Mouse
{
    public partial class FormUpdateSkinColor : Form
    {
        private Image _currentCapImg;
        private bool _allowCrop = false;

        private Image<Bgr, Byte> _finalCrop;

        private Rectangle _rect = new Rectangle();

        private Pen _pen = new Pen(Color.Red, 2);
        private List<Rectangle> _rectList = new List<Rectangle>();
        private VideoCapture _capture = null;

        private Point _roiBegin = new Point();
        private Point _roiEnd = new Point();

        private Image<Bgr, Byte> _frameRT;

        private List<string> _samplingData = new List<string>();
        private List<string> _samplingDataHSV = new List<string>();

        private double FrameRate = 30;
        private double Framesno = 0;
        private int _sampleCounter = 0;

        private int _skinCode = 1;
        private const int SKIN_CONSTANT = 1;
        private const int NON_SKIN_CONSTANT = 2;

        private List<Hsv> _hsvRaw = new List<Hsv>();

        public FormUpdateSkinColor()
        {
            InitializeComponent();
            _currentCapImg = new Bitmap(pictureBoxVideo.Width, pictureBoxVideo.Height);
        }

        private void VideoRun()
        {
            try
            {

                Framesno = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);

                Image<Bgr, Byte> initImg = _capture.QueryFrame().ToImage<Bgr, Byte>();
          //      Image<Bgr, Byte> videoImg = new Image<Bgr, Byte>(initImg.Size);
                _frameRT = new Image<Bgr, Byte>(initImg.Size);


                CvInvoke.Flip(initImg, _frameRT, Emgu.CV.CvEnum.FlipType.Horizontal);
                double time_index = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec);
                double framenumber = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);

                pictureBoxVideo.Image = _frameRT.ToBitmap();

                Thread.Sleep((int)(1000.0 / FrameRate));

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }

            
        }

        private void ProcessFrame_Idle(object sender, EventArgs arg)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            VideoRun();
            watch.Stop();
            long elapsedTime = watch.ElapsedMilliseconds;
            long fps = 1000L/elapsedTime;
            

            
        }

        private void btnStartVid_Click(object sender, EventArgs e)
        {
            _allowCrop = false;
            btnStartVid.Enabled = false;
            btnCapVid.Enabled = true;
            
            _capture = null;
            _capture = new VideoCapture(0);
            int height = pictureBoxVideo.Size.Height;
            int width = pictureBoxVideo.Size.Width;
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 30);
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, height);
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, width);

            
            Application.Idle += ProcessFrame_Idle;
            
        }

        private void btnCapVid_Click(object sender, EventArgs e)
        {

            //pictureBoxCapImg.Image = pictureBoxVideo.Image;
            btnCapVid.Enabled = false;
            btnStartVid.Enabled = true;
            _currentCapImg = pictureBoxVideo.Image;

            btnStartVid.Enabled = true;
            //btnStopVid.Enabled = false;
            Application.Idle -= ProcessFrame_Idle;
            Memory.ReleaseData(ref _capture);
            _allowCrop = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bgr"></param>
        /// <param name="code">If 1 its skin, if 2 its non-skin</param>
        /// <returns></returns>
        private string BgrToSkinData(Bgr bgr, int code)
        {
            string bS = bgr.Blue.ToString();
            string gS = bgr.Green.ToString();
            string rS = bgr.Red.ToString();
            string c = ",";

            return String.Concat(bS, c, gS, c, rS, c, code.ToString());

        }
        private string HSVToSkinData(Hsv hsv)
        {
            try
            {
                string h = Math.Round(hsv.Hue, 4).ToString();
                string s = Math.Round(hsv.Satuation, 4).ToString();
                string v = Math.Round(hsv.Value, 4).ToString();

                string c = ",";

                string code = _skinCode.ToString();
                return h + c + s + c + v + c + code;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n"+ e.StackTrace);
                return "";
            }
        }

        private List<string> ExtractData(Image<Bgr, Byte> inputData) 
        {
            List<string> dataList = new List<string>();
            for (int i = 0; i < inputData.Height; i++)
                for (int j = 0; j < inputData.Width; j++) 
                {
                    if (rbSkin.Checked) _skinCode = 1;
                    else if (rbNonSkin.Checked) _skinCode = 2;
                    
                    dataList.Add(BgrToSkinData(inputData[i, j], _skinCode));

                } 
                

            return dataList;
        }

        private List<string> ExtractDataHSV(Image<Bgr, Byte> inputData) 
        {
            try
            {
                List<string> hsvDataList = new List<string>();
                Image<Hsv, Byte> inputHsv = inputData.Convert<Hsv, Byte>();

                for (int i = 0; i < inputHsv.Height; i++)
                {
                    for (int j = 0; j < inputHsv.Width; j++)
                    {
                        
                            string hsvData = HSVToSkinData(inputHsv[i, j]);
                            hsvDataList.Add(hsvData);
                        
                    }
                }

                return hsvDataList;

            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message + "\n" + exe.StackTrace);
                return new List<string>();
            }
        }

        private void btnGetSample_Click(object sender, EventArgs e)
        {
            try
            {
                //List<string> dataList = ExtractData(_finalCrop);
                //_samplingData.AddRange(dataList);

                List<string> dataListHSV = ExtractDataHSV(_finalCrop);
                _samplingDataHSV.AddRange(dataListHSV);
                _sampleCounter++;


                lblSampleCnter.Text = _sampleCounter.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnSaveSamples_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> prevData = File.ReadAllLines("skindataset.csv").ToList();
                List<string> prevDataLog = File.ReadAllLines("Logging\\TrainLog\\trainlog.csv").ToList();

                List<string> finalData = new List<string>();

                finalData.AddRange(prevData);
                finalData.AddRange(_samplingData);



                File.WriteAllLines("skindataset.csv", finalData.ToArray());
                prevDataLog.Add("Last data updated: Line = " + prevData.Count.ToString());
                _samplingData = new List<string>();
                _sampleCounter = 0;
                lblSampleCnter.Text = _sampleCounter.ToString();

                // saving hsv data
                List<string> prevDataHSV = File.ReadAllLines("data\\trainhsv\\hsvskindataset.csv").ToList();

                List<string> finalDataHSV = new List<string>();

                finalDataHSV.AddRange(prevDataHSV);
                finalDataHSV.AddRange(_samplingDataHSV);

                File.WriteAllLines("data\\trainhsv\\hsvskindataset.csv", finalDataHSV.ToArray());
                _samplingDataHSV = new List<string>();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }


        }

        private void rbSkin_CheckedChanged(object sender, EventArgs e)
        {
            if(rbSkin.Checked) _skinCode = SKIN_CONSTANT;
            if (rbNonSkin.Checked) _skinCode = NON_SKIN_CONSTANT;
  
        }

        private void rbNonSkin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSkin.Checked) _skinCode = SKIN_CONSTANT;
            if (rbNonSkin.Checked) _skinCode = NON_SKIN_CONSTANT;
        }

        private void FormUpdateSkinColor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Memory.ReleaseData(ref _capture);
            this.Hide();
        }

        private void FormUpdateSkinColor_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxVideo_MouseUp(object sender, MouseEventArgs e)
        {
            if (_allowCrop)
            {
                
                int xRoi = e.X;
                int yRoi = e.Y;

                _roiEnd = new Point(xRoi, yRoi);

                _rect = new Rectangle(_roiBegin, new Size(_roiEnd.X - _roiBegin.X, _roiEnd.Y - _roiBegin.Y));

                using (Graphics g = this.pictureBoxVideo.CreateGraphics())
                {
                    Pen pen = new Pen(Color.Black, 2);

                    g.DrawRectangle(pen, _rect);

                    pen.Dispose();
                }

                try
                {
                    Bitmap src = pictureBoxVideo.Image as Bitmap;
                    //Bitmap src = _currentCapImg as Bitmap;
                    Bitmap target = new Bitmap(_rect.Width, _rect.Height);
                    Point adjPoint = new Point(_rect.Location.X - (pictureBoxVideo.Width - src.Width) / 2,
                        _rect.Location.Y - (pictureBoxVideo.Height - src.Height) / 2);

                    Rectangle newRect = new Rectangle(adjPoint, new Size(_rect.Width, _rect.Height));
                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         newRect,
                                         GraphicsUnit.Pixel);


                        pictureBoxCrop.Image = target;
                        _finalCrop = new Image<Bgr, Byte>(target);

                    }
                }
                catch (Exception)
                {

                } 
            }

        }

        private void pictureBoxVideo_MouseDown(object sender, MouseEventArgs e)
        {
            if (_allowCrop)
            {
                int xRoi = e.X;
                int yRoi = e.Y;

                _roiBegin = new Point(xRoi, yRoi); 
            }
        }

    }
}
