using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

using System.Windows.Forms;


namespace Hand_Virtual_Mouse
{
    public class ImgProc
    {



        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            try
            {
                for (int x = destRegion.Location.X; x <= destRegion.Width; x++) 
                {
                    for (int y = destRegion.Location.Y; y <= destRegion.Height; y++) 
                    {
                        destBitmap.SetPixel(x, y, srcBitmap.GetPixel(x,y));
                    }
                }
            }
            catch (Exception e)
            {
                string stackTrace = e.StackTrace;
                string message = e.Message;

                MessageBox.Show(stackTrace + "\n" + message, "Error");
                
            }
        }

        public static Image<Gray, Byte> InsertROI(Image<Gray, Byte> hist, Image<Gray, Byte> fast, Rectangle roi) 
        {
            Image<Gray, Byte> result = new Image<Gray, Byte>(hist.Size);

            for (int x = roi.Location.X; x < roi.Location.X + roi.Width; x++) 
            {
                for (int y = roi.Location.Y; x < roi.Location.Y + roi.Height; y++)
                {
                    hist[x, y] = fast[x, y]; 
                }
            }

            return result;
        }
        public static Bitmap CropImage(Bitmap src, Rectangle roi) 
        {
            Bitmap result = new Bitmap(roi.Width, roi.Height);
            try
            {
                Bitmap target = new Bitmap(roi.Width, roi.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                     roi,
                                     GraphicsUnit.Pixel);


                    result = target;
                    return result;

                }
            }
            catch (Exception)
            {
                return result;
            }

            //Bitmap bmpImage = new Bitmap(src);
            //return bmpImage.Clone(roi, bmpImage.PixelFormat);
        }

        public static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            System.Drawing.Imaging.ImageCodecInfo[] encoders;
            encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static Image<Gray, Byte> CropImage(Image<Gray, Byte> src, Rectangle roi) 
        {
            Image<Gray, Byte> result = new Image<Gray, Byte>(roi.Width, roi.Height);
            for (int i = roi.Location.X; i < roi.Width; i++) 
            {
                for (int j = roi.Location.Y; j < roi.Height; j++) 
                {
                    result.Data[j - roi.Location.Y, i - roi.Location.X, 0] = src.Data[j, i, 0];
                }
            }

            return result;
            
        }

        public static void SaveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private static Hsv GetItem(Image<Hsv, Byte> img, int i, int j) 
        {
            return img[i, j];
        }

      



        public static Image<Gray, Byte> SkinDetectHSV(HSVTree skinHsvTree, HSVTree nSkinHsvTree ,Image<Bgr, Byte> img, int erode, int radius)
        {
            HSVTree hsvSkinTreeCopy = skinHsvTree;
            HSVTree hsvNSkinTreeCopy = nSkinHsvTree;
            Image<Gray, Byte> resultImg = new Image<Gray, Byte>(img.Size);
            resultImg._EqualizeHist();
            Image<Hsv, Byte> hsvImg = img.Convert<Hsv, Byte>();
            //List<Hsv> hsvStack = new List<Hsv>();

            
            int height = resultImg.Height;
            int width = resultImg.Width;
            
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++) 
                {
                    Hsv hsv = hsvImg[i, j];
                         
                    int freqSkin = hsvSkinTreeCopy.GetFreq(hsv);
                    int freqNSkin = hsvNSkinTreeCopy.GetFreq(hsv);

                    if (freqSkin > freqNSkin)
                        resultImg.Data[i, j, 0] = 250;

                }


            resultImg._EqualizeHist();
            Image<Gray, Byte> erodedImg = resultImg.Erode(erode);


            Image<Gray, Byte> dilatedImg = erodedImg.Dilate(radius);
            dilatedImg._EqualizeHist();
            return dilatedImg;

        }

        public void HistSkinDetectWithTrain(BGRTree skinTree, BGRTree nSkinTree, Image<Bgr, Byte> inputImg, 
             ref Image<Gray, Byte> outImg, ref string[] logFile,bool enableLog = false)
        {
             outImg = new Image<Gray, Byte>(inputImg.Size);

             int height = outImg.Height;
             int width = outImg.Width;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Bgr bgrPix = inputImg[i, j];

                    
                    int probSkin = skinTree.GetFreq(bgrPix);

                    if (probSkin > 0.0)
                    {
                        int probNSkin = nSkinTree.GetFreq(bgrPix);
                        if (probSkin >= probNSkin)
                            outImg.Data[i, j, 0] = 255;
                    }


                }
            }

        }


        private static void PaintColor(ref Image<Gray, Byte> img, System.Drawing.Point startPt, int widthROI, int heightROI, int color) 
        {
            byte intesityByte = Convert.ToByte(color);
            for (int x = startPt.X; x <= widthROI; x++)
                for (int y = startPt.Y; y <= heightROI; y++)
                    img.Data[x, y,0] = intesityByte;
        }

        private static void CountBlackWhite(Image<Gray, Byte> img, System.Drawing.Point startPt, int widthROI, int heightROI,
            ref int whiteCnt, ref int blackCnt) 
        {
            whiteCnt = 0;
            blackCnt = 0;

            for (int x = startPt.X; x <= widthROI; x++)
                for (int y = startPt.Y; y <= heightROI; y++) 
                {
                    if (img.Data[x, y,0] > 200) whiteCnt++;
                    else blackCnt++;
                }
        }

        private static void PaintBlackOrWhite(ref Image<Gray, Byte> img, int widthROI, int heightROI) 
        {
            int refWidth = img.Width;
            int refHeight = img.Height;

            int currX = 0;
            int currY = 0;

            int extraStepWidth = 0;
            int extraStepHeight = 0;

            //if (refWidth % widthROI > 0) extraStepWidth = 1;
            //if (refHeight % heightROI > 0) extraStepHeight = 1;

            int stepWidthMax = (refWidth / widthROI) + extraStepWidth;
            int stepHeightMax = (refHeight / heightROI) + extraStepHeight;

            for (int x = 0; x < stepWidthMax; x++) 
            {
                for (int y = 0; y < stepHeightMax; y++) 
                {
                    int black = 0;
                    int white = 0;
                    System.Drawing.Point currPt = new System.Drawing.Point(currX, currY);
                    CountBlackWhite(img, currPt, widthROI, heightROI, ref white, ref black);

                    if (white > black) PaintColor(ref img, currPt, widthROI, heightROI, 255);
                    else PaintColor(ref img, currPt, widthROI, heightROI, 0);

                    currY += heightROI;
                }

                currX += widthROI;
            }

        }
        

        public static Image<Gray, Byte> DetectSkin(Image<Bgr, Byte> img, int erodeIter) 
        {
            Image<Hsv, Byte> currentHsvFrame = img.Convert<Hsv, Byte>();
            Image<Gray, Byte> skin = new Image<Gray, Byte>(img.Width, img.Height);
            Hsv min = new Hsv(0, 44, 0);
            Hsv max = new Hsv(20, 255, 255);

            //hsv_min = new Hsv(0, 45, 0);
            //hsv_max = new Hsv(20, 255, 255);

            skin = currentHsvFrame.InRange(min, max);
            Image<Gray, Byte> finalRes = skin.Erode(erodeIter);

            return finalRes;
  
  
        }
        
        

        private Rectangle GetLargestRect(Rectangle[] rects) 
        {
            Rectangle largest = new Rectangle();

            foreach (Rectangle rect in rects) 
            {
                if (rect.Height * rect.Width > largest.Height * largest.Width)
                    largest = rect;
            }

            return largest;
            
        }

        public static Image<Gray, Byte> HistSkinDetect(BGRTree skinTree, Image<Bgr, Byte> inputImg, double minThresh)
        {
            Image<Gray, Byte> resultImg = new Image<Gray, Byte>(inputImg.Size);

            int height = resultImg.Height;
            int width = resultImg.Width;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Bgr bgrPix = inputImg[i, j];

                    //int freqSkin = skinTree.GetFreq(bgrPix);

                    //if (freqSkin > 0)
                    //{
                    //    int freqNSkin = nSkinTree.GetFreq(bgrPix);
                    //    if (freqSkin >= freqNSkin)
                    //        resultImg.Data[i, j, 0] = 255;
                    //}

                    double probSkin = skinTree.GetProb(bgrPix);

                    if (probSkin >= minThresh)
                    {
                            resultImg.Data[i, j, 0] = 255;
                    }


                }
            }

            return resultImg;
        }

        public static Image<Gray, Byte> HistSkinDetect(BGRTree skinTree, Image<Bgr, Byte> inputImg, int minFreq)
        {
            Image<Gray, Byte> resultImg = new Image<Gray, Byte>(inputImg.Size);
             //   CvInvoke.CvtColor(inputImg, resultImg, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            int height = resultImg.Height;
            int width = resultImg.Width;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Bgr bgrPix = inputImg[i, j];

                    int freqSkin = skinTree.GetFreq(bgrPix);

                    if (freqSkin >= minFreq)
                    {
                            resultImg.Data[i, j, 0] = 255;
                    }



                }
            }

            return resultImg;
        }

        public static Image<Gray, Byte> HistSkinDetectHSV(HSVTree skinTree, HSVTree nSkinTree, Image<Hsv, Byte> inputImg) 
        {
            Image<Gray, Byte> resultImg = new Image<Gray, Byte>(inputImg.Size);

            int height = resultImg.Height;
            int width = resultImg.Width;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Hsv hsvPix = inputImg[i, j];

                    //int freqSkin = skinTree.GetFreq(hsvPix);

                    //if (freqSkin > 0)
                    //{
                    //    int freqNSkin = nSkinTree.GetFreq(hsvPix);
                    //    if (freqSkin >= freqNSkin)
                    //        resultImg.Data[i, j, 0] = 255;
                    //}

                    double probSkin = skinTree.GetProb(hsvPix);

                    if (probSkin > 0.0)
                    {
                        double probNSkin = nSkinTree.GetProb(hsvPix);
                        if (probSkin >= probNSkin)
                            resultImg.Data[i, j, 0] = 255;
                    }


                }
            }

            return resultImg;
        
        }

        public static double ContourROIRatio(Image<Gray, Byte> cropped) 
        {
            double sumOfWhites = 0;
            double tot = cropped.Width * cropped.Height;

            for (int i = 0; i < cropped.Height; i++)
                for (int j = 0; j < cropped.Width; j++)
                    if (cropped.Data[i, j, 0] > 0.0)
                        sumOfWhites += 1.0;

            return sumOfWhites / tot;
        }

        public static int LargestContourIndex(VectorOfVectorOfPoint contour) 
        {
            int index = 0;
            double area = 0.0;
            for (int i = 0; i < contour.Size; i++) 
            {
                double areaTmp = CvInvoke.ContourArea(contour[i], false);
                
                if (areaTmp > area) 
                {
                    area = areaTmp;
                    index = i;
                }
            }

            return index;
    
        }



        
        

        public static Image<Gray, Byte> SkinDetectUsingRange(Image<Bgr, Byte> img, Hsv hsvMin, Hsv hsvMax, int erode, int dilate) 
        {

            Image<Hsv, Byte> currentHsvFrame = img.Convert<Hsv, Byte>();
            Image<Gray, byte> skin = new Image<Gray, byte>(img.Width, img.Height);
            //Image<Gray, byte> skin = unNormedskin._EqualizeHist();
            currentHsvFrame._EqualizeHist();
            skin = currentHsvFrame.InRange(hsvMin, hsvMax);
            Image<Gray, Byte> erodeSkin =  skin.Erode(erode);
            Image<Gray, Byte> dilateSkin = erodeSkin.Dilate(dilate);

            //AForge.Imaging.Filters.FillHoles filter = new AForge.Imaging.Filters.FillHoles();
            //Bitmap imageToFill = erodeSkin.ToBitmap();
            //filter.MaxHoleHeight = holeHeight;
            //filter.MaxHoleWidth = holeWidth;
            //filter.CoupledSizeFiltering = false;
            //Bitmap result = filter.Apply(imageToFill);

            return dilateSkin;
            //return erodeSkin;
        }

        
        public static unsafe Image<Gray, Byte> SkinDetectHSV(HSVLookUpTable hsvLookUp, Image<Bgr, Byte> img, int minFreq, int erode, int radius)
        {
            HSVLookUpTable hsvLookUpCopy = hsvLookUp;
            byte value = 255;
            byte* valAdd = &value;
            Image<Gray, Byte> resultImg = new Image<Gray, Byte>(img.Size);

            Byte[, ,] resultImgByte = resultImg.Data;
            Image<Hsv, Byte> hsvImg = img.Convert<Hsv, Byte>();

            Mat result = new Mat(img.Height, img.Cols, Emgu.CV.CvEnum.DepthType.Default, 1);

            int height = resultImg.Height;
            int width = resultImg.Width;
            Hsv hsvPt = new Hsv();

            byte hsvHue;
            byte hsvSat;
            byte hsvVal;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {

                    hsvHue = hsvImg.Data[i, j, 0];
                    hsvSat = hsvImg.Data[i, j, 1];
                    hsvVal = hsvImg.Data[i, j, 2];

                    hsvPt = new Hsv(hsvHue, hsvSat, hsvVal);
                    if (hsvLookUpCopy.GetFreq(hsvPt) > minFreq)
                    {
                        resultImgByte[i, j, 0] = value;
                    }
                }

            }



            resultImg = new Image<Gray, byte>(resultImgByte);
            Image<Gray, Byte> erodedImg = resultImg.Erode(erode);
            Image<Gray, Byte> dilatedImg = erodedImg.Dilate(radius);
            return dilatedImg.SmoothMedian(7);
            //return resultImg.SmoothMedian(11);

        }

        public static double AreaRatio(Image<Gray, Byte> img, Rectangle roi) 
        {
            string METHOD_NAME = "Area Ratio";
            // starting row
            int rowStart = roi.Location.Y;
            // starting col
            int colStart = roi.Location.X;

            double actualAreaPix = 0.0;
            double totalAreaPix = (double)(roi.Width * roi.Height);

            try
            {
                int rowLimit = rowStart + roi.Height - 1;
                int colLimit = colStart + roi.Width - 1;
                for (int i = rowStart; i < (rowStart + roi.Height - 1); i++)
                {
                    for (int j = colStart; j < (colStart + roi.Width - 1); j++)
                    {
                        try
                        {
                            byte data = img.Data[i, j, 0];

                            bool onTresh = data > 0;

                            if (onTresh) actualAreaPix += 1.0;
                        }
                        catch (Exception exp)
                        {
                            string message1 = "Roi Location: " + roi.Location.ToString() + "\n";
                            string message2 = "Actual Area Pix: " + Math.Round(actualAreaPix, 2).ToString() + "\n";
                            string message3 = "Roi Location: " + Math.Round(totalAreaPix, 2).ToString() + "\n";
                            string message4 = "Index: " + "[" + i.ToString() + "," + j.ToString() + "]" + "\n";
                            string message5 = "Size of Image: " + img.Size.ToString() + "\n";
                            string message6 = "Row Limit: " + rowLimit.ToString() + "\n";
                            string message7 = "Col Limit: " + colLimit.ToString() + "\n";

                            string message = message1 + message2 + message3 + message4 + 
                                message5 + message6 + message7 + exp.Message;
                            MessageBox.Show(message, "Error: "+ METHOD_NAME + "(Inside Loop)");
                        }
                    }
                }

                return actualAreaPix / totalAreaPix;
            }
            catch (Exception )
            {
                string message1 = "Roi Location: " + roi.Location.ToString() + "\n";
                string message2 = "Actual Area Pix: " + Math.Round(actualAreaPix, 2).ToString() + "\n";
                string message3 = "Roi Location: " + Math.Round(totalAreaPix, 2).ToString() + "\n";

                string message = message1 + message2 + message3;

                MessageBox.Show(message, "Error: " + METHOD_NAME);
                
                return 0.0;
            }

        }

        public static double[] AreaSegmentsRatio(Image<Gray, Byte> img, int heightSegmentNum, int widthSegmentNum, Rectangle roiParent) 
        {
            string METHOD_NAME = "Area Segment Ratio";
           // determine the height and width for each segment
            int segmentHeight = roiParent.Height / heightSegmentNum - 1;
            int segmentWidth = roiParent.Width / widthSegmentNum  - 1;
            Size defSize = new Size(segmentWidth, segmentHeight);

            // determine the points each

            List<System.Drawing.Point> segmentPoints = new List<System.Drawing.Point>();

            int spX = roiParent.Location.X;
            int spY = roiParent.Location.Y;

            if (spX < 1) spX = 1;
            if (spY < 1) spY = 1;

            

            // start at lowest 
            try
            {

                for (int i = 0; i < heightSegmentNum - 1; i++)
                    for (int j = 0; j < widthSegmentNum - 1; j++)
                        segmentPoints.Add(new System.Drawing.Point(spX + j * segmentWidth, spY + i * segmentHeight));


                List<Rectangle> roiSegments = new List<Rectangle>();

                segmentPoints.ForEach(pt =>
                    roiSegments.Add(new Rectangle(pt, defSize)));


                // calculate each area
                List<double> areaList = new List<double>();

                //roiSegments.ForEach(r)

                roiSegments.ForEach(roiChild => areaList.Add(AreaRatio(img, roiChild)));

                return areaList.ToArray();

            }
            catch (Exception)
            {
                //int segmentHeight = roiParent.Height / heightSegmentNum - 1;
                //int segmentWidth = roiParent.Width / widthSegmentNum - 1;
                //Size defSize = new Size(segmentWidth, segmentHeight);

                //// determine the points each

                //List<System.Drawing.Point> segmentPoints = new List<System.Drawing.Point>();

                //int spX = roiParent.Location.X;
                //int spY = roiParent.Location.Y;

                string message1 = "Segment Height: " + segmentHeight.ToString() + "\n";
                string message2 = "Segment Width: " + segmentWidth.ToString() + "\n";
                string message3 = "Def Size: " + defSize.ToString() + "\n";
                string message4 = "SpX: " + spX.ToString() + "\n";
                string message5 = "SpY: " + spY.ToString() + "\n";

                string message = message1 + message2 + message3 + message4 + message5;

                MessageBox.Show(message, "Error: " + METHOD_NAME);

                return new double[6];

            }
        }

        

        
    }
}
