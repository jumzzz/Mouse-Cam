using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

//using AForge.Math.Geometry;
//using Accord.Imaging.Filters;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace Hand_Virtual_Mouse
{
    public class ROISearcher
    {
        private VectorOfVectorOfPoint _contour;
        private List<VectorOfPoint> _listOfContours;
        private MulticlassSupportVectorMachine _svm;

        private const int CLASSIFICATION_HAND = 0;
        private const int CLASSIFICATION_ARM = 1;
        private const int CLASSIFICATION_REJECT = 2;

        private Image<Gray, Byte> ImageInput;
        private double[] ScaleValues(double[] list, double scalingFactor)
        {
            List<double> values = new List<double>();
            list.ToList().ForEach(d => values.Add(scalingFactor * d));

            return values.ToArray();
        }

        public ROISearcher()
        {
            string path = "Feature Extraction\\blob_analyzer.xml";
            _svm = MulticlassSupportVectorMachine.Load(path);

        }

        public Rectangle GetROI(Image<Gray,Byte> img) 
        {
            LocateLargestContour(img);
            return LocateROI();
        }

        private void LocateLargestContour(Image<Gray, Byte> img)
        {
            ImageInput = img;
            _contour = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(img, _contour, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxTc89L1);
            List<VectorOfPoint> listOfContours = new List<VectorOfPoint>();

            for (int i = 0; i < _contour.Size; i++)
            {
                listOfContours.Add(_contour[i]);
            }

            //List<VectorOfPoint> listOfContours = new List<VectorOfPoint>();


            //_listOfContours = listOfContours.OrderByDescending(vp => CvInvoke.ContourArea(vp, true)).ToList();
            
        }

        private Rectangle LocateROI()
        {
            int prediction = -1;
            VectorOfPoint contourOfInterest = new VectorOfPoint();
            
            int index = 0;

            index = ImgProc.LargestContourIndex(_contour);
            contourOfInterest = _contour[index];

            MCvMoments moment = CvInvoke.Moments(contourOfInterest);
            double[] huMoment = moment.GetHuMoment();

            prediction = _svm.Compute(huMoment);

            //foreach (VectorOfPoint vp in _listOfContours.GetRange(0, 5))
            //{
            //    MCvMoments moment = CvInvoke.Moments(vp);
            //    double[] huMoment = moment.GetHuMoment();

            //    prediction = _svm.Compute(huMoment);

            //    if (prediction == CLASSIFICATION_ARM || prediction == CLASSIFICATION_HAND)
            //    {
            //        contourOfInterest = vp;
            //        break;
            //    }
            //}

            if (prediction == CLASSIFICATION_REJECT)
                return Rectangle.Empty;
            else if (prediction == CLASSIFICATION_HAND)
            {
                //Rectangle rectRotRect = rectRot.MinAreaRect();
                //Rectangle init = CvInvoke.MinAreaRect(contoursEval1[largestContourIndexEval1]).MinAreaRect();
                //Point final = new Point(rectRotRect.X + init.X, rectRotRect.Y + init.Y);

                //return new Rectangle(final, init.Size);


                return CvInvoke.MinAreaRect(contourOfInterest).MinAreaRect();
            }
            else if (prediction == CLASSIFICATION_ARM)
            {
                Mat convexityDefect = new Mat();
                VectorOfInt hull = new VectorOfInt();
                CvInvoke.ConvexHull(contourOfInterest, hull, false, false);
                CvInvoke.ConvexityDefects(contourOfInterest, hull, convexityDefect);
                RotatedRect rectRot = CvInvoke.MinAreaRect(contourOfInterest);
                ModifiedRotatedRect rotRectMod = new ModifiedRotatedRect(rectRot);
                int yDel = 0;

                double ptLftToRight = Geometry.Distance(rotRectMod.Pul, rotRectMod.Pur);
                double ptUpToDown = Geometry.Distance(rotRectMod.Pul, rotRectMod.Pll);

                if (!convexityDefect.IsEmpty)
                {
                    Matrix<int> convex = new Matrix<int>(convexityDefect.Rows, convexityDefect.Cols, convexityDefect.NumberOfChannels);

                    convexityDefect.CopyTo(convex);

                    List<Point> contourTmp = new List<Point>();

                    for (int i = 0; i < contourOfInterest.Size; i++)
                        contourTmp.Add(contourOfInterest[i]);


                    List<ConvexDefects> convexDefectList = new List<ConvexDefects>();

                    for (int i = 0; i < convex.Rows; i++)
                    {
                        // do not touch
                        int startIdx = convex.Data[i, 0];
                        int endIdx = convex.Data[i, 1];
                        int pointIdx = convex.Data[i, 2];

                        Point startPt = contourOfInterest[startIdx];
                        Point endPt = contourOfInterest[endIdx];
                        Point defectPt = contourOfInterest[pointIdx];

                        // do not touch
                        convexDefectList.Add(new ConvexDefects(startPt, endPt, defectPt));
                    }


                    if (ptLftToRight <= ptUpToDown)
                    {

                        Point pc1Tmp = convexDefectList[0].DefectPt;
                        Point pc2Tmp = convexDefectList[1].DefectPt;

                        Point pc = pc1Tmp.Y > pc2Tmp.Y ? pc1Tmp : pc2Tmp;


                        Point ptUpLeft = rotRectMod.Pul;
                        Point ptUpRight = rotRectMod.Pur;
                        Point ptLowLeft = rotRectMod.Pll;
                        Point ptLowRight = rotRectMod.Plr;

                        ModifiedRotatedRect rotRectEval1 = ModifiedRotatedRect.Cut(ptUpLeft, ptUpRight, ptLowLeft, ptLowRight, pc);
                        ModifiedRotatedRect rotRectEval2 = ModifiedRotatedRect.Cut(ptUpLeft, ptUpRight, ptLowLeft, ptLowRight, pc, true);

                        Size sizeFrame = ImageInput.Size;
                        Rectangle rectROIEval1 = rotRectEval1.ToRect(sizeFrame);
                        Rectangle rectROIEval2 = rotRectEval2.ToRect(sizeFrame);

                        Mat cloneMat1 = ImageInput.Clone().Mat;

                        Mat matToBeEval1 = new Mat(cloneMat1, rectROIEval1);

                        VectorOfVectorOfPoint contoursEval1 = new VectorOfVectorOfPoint();

                        Mat matHierachyEval1 = new Mat();
                        CvInvoke.FindContours(matToBeEval1, contoursEval1, matHierachyEval1, Emgu.CV.CvEnum.RetrType.External,
                            Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxTc89L1);

                        int largestContourIndexEval1 = ImgProc.LargestContourIndex(contoursEval1);

                        MCvMoments momentEval1 = CvInvoke.Moments(contoursEval1[largestContourIndexEval1]);

                        double[] huMomentsEval1 = momentEval1.GetHuMoment();

                        double[] featureVectorSearch = ScaleValues(huMomentsEval1, 5000.0);

                        int predictionEval1 = _svm.Compute(featureVectorSearch, MulticlassComputeMethod.Elimination);

                        //double[] featureVectorHand = ScaleValues(huMomentsEval1.
                        //    .GetRange(0, _svmMachineHand.Inputs).ToArray(), 1000.0);

                        if (predictionEval1 == CLASSIFICATION_HAND)
                        {
                            Rectangle rectRotRect = rectRot.MinAreaRect();
                            Rectangle init = CvInvoke.MinAreaRect(contoursEval1[largestContourIndexEval1]).MinAreaRect();
                            Point final = new Point(rectRotRect.X + init.X, rectRotRect.Y + init.Y);

                            return new Rectangle(final, init.Size);

                        }
                        
                        else
                            return Rectangle.Empty;
                    }
                    else return Rectangle.Empty;


                }
                else return Rectangle.Empty;
            }
            else return Rectangle.Empty;
        }

    }
}
