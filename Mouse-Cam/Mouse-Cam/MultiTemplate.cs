using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Util;


namespace Hand_Virtual_Mouse
{
    public class MultiTemplate
    {
        #region PROPERTIES
        private const double INIT_MIN_PROB = 100.0;
        private const double INIT_MAX_PROB = 0.0;

        public List<Image<Gray, Byte>> TemplateMainList { get; set; }
        public List<Image<Gray, Byte>> TemplateResizeList { get; set; }
        public Image<Gray, Byte> MaxTemplate { get; set; }
        public Image<Gray, Byte> MinTemplate { get; set; }
        public double MaxProb { get; set; }
        public double AveProb { get; set; }
        public double MinProb { get; set; }

        #endregion

        #region CONSTRUCTOR

        public MultiTemplate(string dir) 
        {
            LoadMainTemplate(dir);
        }

        #endregion

        #region METHODS
        private void LoadMainTemplate(string dir)
        {
            TemplateMainList = new List<Image<Gray, byte>>();
            
            string[] imagePathList = Directory.GetFiles(dir,"*.bmp");

            foreach (string imgPath in imagePathList) 
            {
                Bitmap bm = new Bitmap(imgPath);
                Image<Gray, Byte> template = new Image<Gray, Byte>(bm);
                TemplateMainList.Add(template);
            }
        }

        public void ResizeTemplate(Rectangle rect, int numRatio, int denRatio) 
        {
            TemplateResizeList = new List<Image<Gray, Byte>>();

            int resizeWidth = numRatio * rect.Width / denRatio;
            int resizeHeight = numRatio * rect.Height / denRatio;

            //int resizeWidth = 50;
            //int resizeHeight = 50;

            TemplateMainList.ForEach(img =>
                TemplateResizeList.Add(img.Resize(resizeWidth, resizeHeight, Inter.Area)));
            
          }

        public void TemplateMatchOnROI(Image<Gray, Byte> sourceCropped)
        {
            //Image<Gray, Byte> source = sourceCropped.Resize(50, 50, Inter.Area);

            MinProb = INIT_MIN_PROB;
            MaxProb = INIT_MAX_PROB;
            
            //double sumOfProb = 0.0;

            
            foreach(Image<Gray, Byte> tmp in TemplateResizeList) 
            {
                //double templateTmpProb = TemplateMatcher.TemplateMatchOnROI(sourceCropped, tmp);

                double templateTmpProb = TemplateMatcher.TemplateMatchOnROI(sourceCropped, tmp);
                
                 if (templateTmpProb < MinProb)
                 {
                        MinTemplate = tmp;
                        MinProb = templateTmpProb;
                 }

                 if (templateTmpProb > MaxProb)
                 {
                        MaxTemplate = tmp;
                        MaxProb = templateTmpProb;
                 }

             }
            
        }

        #endregion
    }

    public class TemplateMatcher
    {

        #region STATIC_METHODS
        public static double TemplateMatchOnROI(string dir, Rectangle roi, Image<Gray,Byte> image) 
        {
            Image<Gray, Byte> template = new Image<Gray, byte>(new Bitmap(dir));
            Image<Gray, Byte> source = new Image<Gray, byte>(ImgProc.CropImage(image.ToBitmap(), roi));

            if (source.Width >= template.Width && source.Height >= template.Height)
            {
                using (Image<Gray, float> result = source.MatchTemplate(
                template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
                {
                    double[] minValues;
                    double[] maxValues;
                    Point[] minLocations;
                    Point[] maxLocations;

                    result.MinMax(out minValues, out maxValues,
                        out minLocations, out maxLocations);

                   if (minValues.Length > 0) 
                     return maxValues[0];
                    

                    return 0.0;
                } 
            }

            return 0.0;
        }

        

        public static double TemplateMatchOnROI(Image<Gray, Byte> source, Image<Gray, Byte> template) 
        {
            Mat result = new Mat();
            CvInvoke.MatchTemplate(source, template,result,Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            double minVal = 0;
            double maxVal = 0;
            Point minLoc = new Point();
            Point maxLoc = new Point();

            
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            return maxVal * 100.0;          
                    

        }

        public static double TemplateMatchOnROI(Image<Gray, Byte> template, Rectangle roi, Image<Gray, Byte> image)
        {
            Image<Gray, Byte> source = new Image<Gray, byte>(ImgProc.CropImage(image.ToBitmap(), roi));

            if (source.Width >= template.Width && source.Height >= template.Height)
            {
                using (Image<Gray, float> result = source.MatchTemplate(
                template, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed))
                {
                    double[] minValues;
                    double[] maxValues;
                    Point[] minLocations;
                    Point[] maxLocations;

                    result.MinMax(out minValues, out maxValues,
                        out minLocations, out maxLocations);

                    if (minValues.Length > 0)
                        return maxValues[0]*100.0;


                    return 0.0;
                }
            }

            return 0.0;
        }

        public static void TemplateMatch(Image<Gray, Byte> sourceImage, Image<Gray, Byte> template, double treshold,
            ref Rectangle newTracker, ref double currentTrackingProbability) 
        {
            Mat result = new Mat();

            

            if (true)
            //{
                CvInvoke.MatchTemplate(sourceImage, template, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);

                double minVal = 0.0;
                double maxVal = 0.0;
                Point minLoc = new Point();
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                currentTrackingProbability = maxVal * 100.0;

                if (maxVal > treshold)
                {
                    //Point maxPoint = maxLocations.ToList().Max();
                    newTracker = new Rectangle(maxLoc, template.Size);
                }
                
            //}
            
        }

        #endregion
    }
}
