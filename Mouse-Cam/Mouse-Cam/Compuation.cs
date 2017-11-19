using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Hand_Virtual_Mouse
{
    public class CentroidCP 
    {
        public Point CenterPoint { get; set; }
        public ConnectedPair CP { get; set; }

        public CentroidCP() 
        {
            CenterPoint = new Point();
            CP = new ConnectedPair();
        }
    }

    public class MPDistCP 
    {
        public ConnectedPair CP { get; set; }
        public double Distance { get; set; }
        public MPDistCP() 
        {
            CP = new ConnectedPair();
            Distance = 0.0;
        }
    }
    public class CPProp 
    {
        public int NSides { get; set; }
        public double Sigma { get; set; }
    }
    public class Geometry
    {
        public static Rectangle TemplateMatchingDef(List<ConvexDefects> cdL, Image<Bgr, Byte> img) 
        {
            // find right most x and high most y

            int xMax = 0;
            int yMax = 0;
            foreach (ConvexDefects cd in cdL) 
            {
                if (xMax < cd.StartPt.X) xMax = cd.StartPt.X;
                if (xMax < cd.EndPt.X) xMax = cd.EndPt.X;
                if (xMax < cd.DefectPt.X) xMax = cd.DefectPt.X;

                if (yMax < cd.StartPt.Y) yMax = cd.StartPt.Y;
                if (yMax < cd.EndPt.Y) yMax = cd.EndPt.Y;
                if (yMax < cd.DefectPt.Y) yMax = cd.DefectPt.Y;
            }

            // find low most x and low most y
            int xMin = xMax;
            int yMin = yMax;

            foreach (ConvexDefects cd in cdL)
            {
                if (xMin > cd.StartPt.X) xMin = cd.StartPt.X;
                if (xMin > cd.EndPt.X) xMin = cd.EndPt.X;
                if (xMin > cd.DefectPt.X) xMin = cd.DefectPt.X;

                if (yMin > cd.StartPt.Y) yMin = cd.StartPt.Y;
                if (yMin > cd.EndPt.Y) yMin = cd.EndPt.Y;
                if (yMin > cd.DefectPt.Y) yMin = cd.DefectPt.Y;
            }


            Point locationRect = new Point(xMin, yMin);

            int width = xMax - xMin;
            int height = yMax - yMin;

            
            if (locationRect.X + width > img.Width - 1) width = img.Width - locationRect.X - 1;
            if (locationRect.Y + height > img.Height - 1) height = img.Height - locationRect.Y - 1;

            Size size = new Size(width, height);

            return new Rectangle(locationRect, size);

        }

        //public Rectangle MinRectangle(List<Point> lp) 
        //{
        //    // find right most  point 
        //    //int xRightMost = 0;
        //    //int yUpperMost = 0;

        //    //foreach (Point pt in lp) 
        //    //{
        //    //    if (pt.X > xRightMost) xRightMost = pt.X;
        //    //    if (pt.Y > yUpperMost) yUpperMost = pt.Y;
        //    //}

        //    //int xLoc = xRightMost;
        //    //int yLoc = yUpperMost;

        //    //foreach (Point pt in lp) 
        //    //{
        //    //    if(pt.X < xLoc) pt.X
        //    //}

        //    // left most 
        //}

        public static ModifiedRotatedRect CutArmToHand(ModifiedRotatedRect rotRect) 
        {
            double dist1 = Geometry.Distance(rotRect.Pll, rotRect.Pul);

            Point pt1 = rotRect.Pul;
            Point pt2 = rotRect.Pur;

            int xNewL = (rotRect.Pll.X + rotRect.Pul.X) / 2;
            int yNewL = (rotRect.Pll.Y + rotRect.Pul.Y) / 2;
            
            int xNewR = (rotRect.Plr.X + rotRect.Pur.X) / 2;
            int yNewR = (rotRect.Plr.Y + rotRect.Pur.Y) / 2;
            Point pt3 = new Point(xNewL, yNewL);
            Point pt4 = new Point(xNewR, yNewR);

            List<Point> ptL = new List<Point>();

            ptL.Add(pt1);
            ptL.Add(pt2);
            ptL.Add(pt3);
            ptL.Add(pt4);

            return new ModifiedRotatedRect(ptL);

        }

        public static Point PointDifference(Point pt1, Point pt2) 
        {
            int xDel = pt2.X - pt1.X;
            int yDel = pt2.Y - pt1.Y;

            return new Point(xDel, yDel);
        }

        public static List<ConvexDefects> CorrectDefects(List<ConvexDefects> cdL, double minDist)
        {
            List<ConvexDefects> toBeCorrected = cdL;
            List<ConvexDefects> listOfCorrected = new List<ConvexDefects>();

            
            ConvexDefects reference = new ConvexDefects(new Point(), new Point(), new Point());
            
            int indexReference = 0;
            double perimeter = 0;

            for(int i = 0; i < cdL.Count; i++)
            {
                double perimeterTmp = Distance(cdL[i].DefectPt, cdL[i].EndPt) +
                                      Distance(cdL[i].StartPt, cdL[i].DefectPt) +
                                      Distance(cdL[i].StartPt, cdL[i].EndPt);

                if (perimeterTmp > perimeter) perimeter = perimeterTmp;

                reference = cdL[i];
                indexReference = i;
            }

            Point newDefectPt = reference.DefectPt;

            for (int i = 0; i < cdL.Count; i++) 
            {
                if (indexReference != i) 
                {
                    double difference = Geometry.Distance(reference.DefectPt, cdL[i].DefectPt);

                    if (difference <= minDist) 
                    {
                        // start assigning new defect point
                        Point forReferencePt = new Point(reference.StartPt.X, reference.StartPt.Y +
                        (int)Distance(reference.StartPt, reference.DefectPt));

                        newDefectPt = forReferencePt;
                   
                    }
                }
            }

            ConvexDefects cdNew = reference;
            cdNew.DefectPt = newDefectPt;

            toBeCorrected[indexReference] = cdNew;

            return toBeCorrected;
           //return new List<ConvexDefects>();
        }

        public static bool CCW(Point A, Point B, Point C) 
        {
            return (C.Y - A.Y) * (B.X - A.X) > (B.Y - A.Y) * (C.X - A.X);
        }
        public static bool Intersects(Point A, Point B, Point C, Point D) 
        {
            // point A and B are connected
            // point C and D are connected;
            return CCW(A, C, D) != CCW(B, C, D) && CCW(A, B, C) != CCW(A, B, D);

        }

        public static void AprroxDir(int xOrig, int yOrig, ref int xNew, ref int yNew) 
        {
            int xDir = 0;
            int yDir = 0;
         
            if      (xOrig > 0) xDir = 1;
            else if (xOrig < 0) xDir = -1;

            if      (yOrig > 0) yDir = 1;
            else if (yOrig < 0) yDir = -1;

            double zVal = Math.Sqrt(xOrig * xOrig + yOrig * yOrig);
            double xSlope = Math.Abs(Math.Cos((double)xOrig / zVal));
            double ySlope = Math.Abs(Math.Sin((double)yOrig / zVal));
            
            double minTresh = 0.382;
            double maxTresh = 0.923;

            if (xSlope >= minTresh && xSlope <= maxTresh)
            {
                xNew = (int)zVal*xDir;
                yNew = (int)zVal*yDir;
            }
            else 
            {
                if (xSlope < minTresh && yOrig != 0) 
                { 
                    xNew = 0; 
                    yNew = (int)zVal*yDir;
                } 
                if (ySlope < minTresh)
                {
                    yNew = 0;
                    xNew = (int)zVal*xDir;
                }
                //if (xSlope > maxTresh) 
                //if (ySlope > maxTresh) 
            }

        }

        public static double Pyth(double width, double height) 
        {
            double xsq = width * width;
            double ysq = height * height;

            return Math.Sqrt(xsq + ysq);
        }

        private static void SumPoint(Point toBeSummed, ref int x, ref int y) 
        {
            x += toBeSummed.X;
            y += toBeSummed.Y;
        }
        public static Point Centroid(List<Point> pointSet) 
        {
            int xSum = 0;
            int ySum = 0;

            pointSet.ForEach(pt => SumPoint(pt, ref xSum, ref ySum));

            return new Point(xSum / pointSet.Count, ySum / pointSet.Count);
        }

        private static float Sign(Point pt1, Point pt2, Point pt3) 
        {
            return (pt1.X - pt3.X) * (pt2.Y - pt3.Y) - (pt2.X - pt3.X) * (pt1.Y - pt3.Y);
        }

        public static bool PointInTriangle(Point pt, Point v1, Point v2, Point v3)
        {
            bool b1, b2, b3;

            b1 = Sign(pt, v1, v2) < 0.0f;
            b2 = Sign(pt, v2, v3) < 0.0f;
            b3 = Sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        public static bool InBetXAxis(Point pt, ConnectedPair cp) 
        {
            // assumption that cp.Pt1.X is left and cp.Pt2.X is right

            bool valLR = (cp.Pt1.X <= pt.X) && (cp.Pt2.X >= pt.X);
            
             // assumption that cp.Pt1.X is right and cp.Pt2.X is left
            
            bool valRL = (cp.Pt2.X <= pt.X) && (cp.Pt1.X >= pt.X);

             return valLR || valRL;
        }


        public static double Distance(Point pt1, Point pt2)
        {
            double delX = pt1.X - pt2.X;
            double delY = pt1.Y - pt2.Y;

            return Math.Sqrt(delX * delX + delY * delY);
        }

        public static double TriangularPerimeter(Point pt1, Point pt2, Point pt3) 
        {
            double dist1 = Distance(pt1, pt2);
            double dist2 = Distance(pt2, pt3);
            double dist3 = Distance(pt3, pt1);

            return dist1 + dist2 + dist3;
        }

        //public static double DistanceToContour(Point contPt, ConnectedPair cp)
        //{
        //    double d1 = Distance(contPt, cp.Pt1);
        //    double d2 = Distance(contPt, cp.Pt2);

        //    return d1 + d2;
        //}

        public static Point GetMidPoint(Point pt1, Point pt2) 
        {
            int xMid = (pt1.X + pt2.X) / 2;
            int yMid = (pt1.Y + pt2.Y) / 2;

            return new Point(xMid, yMid);
        }

        public static bool PointInRect(Rectangle rect, Point pt) 
        {
            // A is upper left point
            // B is upper right point
            // C is lower right point
            // D is lower left point

            int Ax = rect.Location.X;
            int Ay = rect.Location.Y;

            int Bx = rect.Location.X + rect.Width;
            int By = rect.Location.Y;

            int Dx = rect.Location.X;
            int Dy = rect.Location.Y + rect.Height;

            int Mx = pt.X;
            int My = pt.Y;

            Vector2D AB = new Vector2D(Bx - Ax, By - Ay);
            Vector2D AD = new Vector2D(Dx - Ax, Dy - Ay);
            Vector2D AM = new Vector2D(Mx - Ax, My - Ay);

            int dotAMAB = AM.X * AB.X + AM.Y * AB.Y;
            int dotABAB = AB.X * AB.X + AB.Y * AB.Y;
            int dotAMAD = AM.X * AD.X + AM.Y * AD.Y;
            int dotADAD = AD.X * AD.X + AD.Y * AD.Y;

            bool v1 = 0 < dotAMAB && dotAMAB <= dotABAB;
            bool v2 = 0 < dotAMAD && dotAMAD <= dotADAD;

            return v1 && v2;
        }

        //public static bool PointInRect(RotRectMod rect, Point pt)
        //{
              
        //}

        public static double TriangularArea(Point pt1, Point pt2, Point pt3)
        {
            // applying herons formula
            double a = Distance(pt1, pt2);
            double b = Distance(pt2, pt3);
            double c = Distance(pt3, pt1);

            double s = (a + b + c) / 2.0;

            double x = s * (s - a) * (s - b) * (s - c);

            return Math.Sqrt(x);
        }

        public static double AngleBetTwoPt(Point ps, Point pe, Point pd) 
        {
            double d1Abs = Geometry.Distance(pd, ps);
            double d2Abs = Geometry.Distance(pd, pe);

            double dotd1d2 = (ps.X - pd.X) * (pe.X - pd.X) +
                (ps.Y - pd.Y) * (pe.Y - pd.Y);

            double cosTheta = dotd1d2 / (d1Abs * d2Abs);

            return Math.Acos(cosTheta);
        
        }

        public static Point RectangularCentroid(Rectangle rect) 
        {
            int xMid = (rect.Location.X + rect.Width) / 2;
            int yMid = (rect.Location.Y + rect.Height) / 2;

            return new Point(xMid, yMid);
        }

        public static double AngleBetTwoPtDeg(Point ps, Point pe, Point pd)
        {
            return (180.0/Math.PI)*Geometry.AngleBetTwoPt(ps,pe,pd);
        }

       
    }

    public class Vector2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2D(int xVal, int yVal) 
        {
            X = xVal;
            Y = yVal;
        }
    }

    public class PointDoublePair
    {
        public double Area { get; set; }
        public Point DefectPt { get; set; }

        public PointDoublePair(double area, Point pt) 
        {
            Area = area;
            DefectPt = pt;
        }
    }

    public class Statistics
    {

        #region FEATURE_SHAPE_FINDERS

        public static List<ConvexDefects> FilterDefects(List<ConvexDefects> cdL,double minAngle, double maxAngle,  
            Rectangle roi,double heightPercent) 
        {
            List<ConvexDefects> filtered = new List<ConvexDefects>();

            int newRectHeight = (int)(heightPercent * (double)roi.Height);

            Rectangle bandpassRect = new Rectangle(roi.Location, new Size(roi.Width, newRectHeight));

            // minAngle is in degrees
            // maxAngle is in degrees
            // function that gets angle returns radians

            foreach (ConvexDefects cd in cdL) 
            {
                Point startPt = cd.StartPt;
                Point endPt = cd.EndPt;
                Point defPt = cd.DefectPt;

                double angleDeg = Geometry.AngleBetTwoPt(startPt, endPt, defPt)*(180.0)/Math.PI ;

                bool angleAllowed = minAngle <= angleDeg && angleDeg <= maxAngle;
                bool inBandPassRect1 = Geometry.PointInRect(bandpassRect, startPt);
                bool inBandPassRect2 = Geometry.PointInRect(bandpassRect, endPt);
                bool inBandPassRect = inBandPassRect1 && inBandPassRect2;

                bool val0 = defPt.Y > startPt.Y;
                bool val1 = defPt.Y > endPt.Y;
                
                bool val = val0 || val1;

                //bool last0 = startPt.Y < defPt.Y && defPt.Y < endPt.Y;
                //bool last1 = startPt.Y > defPt.Y && defPt.Y > endPt.Y;

                //bool last = last1 || last0;

                double distX0 = (double)Math.Abs(startPt.X - endPt.X);
                double distY0 = (double)Math.Abs(startPt.Y - endPt.Y);
                double distR0 = Math.Sqrt(distX0*distX0 + distY0*distY0);

                //double sinTheta = Math.Sin(distY0/distR0)*180.0/Math.PI;
                //double cosTheta = Math.Cos(distX0/distR0)*180.0/Math.PI;

                //bool ratioAllowed = sinTheta < 70.0 && cosTheta < 70.0;
                   
                if (angleAllowed && inBandPassRect && val) filtered.Add(cd);
            }

            return filtered;

        }
        public static double[] DefectFeatureVector(List<ConvexDefects> cdL, double arcLength) 
        {
            int limit = cdL.Count <= 4 ? cdL.Count : 4;

            cdL.Sort((a, b) => (Geometry.TriangularPerimeter(b.StartPt, b.EndPt, b.DefectPt)).
                CompareTo(Geometry.TriangularPerimeter(a.StartPt, a.EndPt, a.DefectPt)));

            List<ConvexDefects> cdRange = cdL.GetRange(0, limit);
               
            cdRange.Sort((a, b) => (a.StartPt.X).CompareTo(b.StartPt.X));

            double[] featureVector = new double[] {0,0,0,0};
            List<double> featureVectorList = new List<double>();

            for (int i = 0; i < cdRange.Count; i++) 
            {
                
                double perimeter = Geometry.TriangularPerimeter(cdRange[i].StartPt, cdRange[i].EndPt, cdRange[i].DefectPt);
                double perimeterROI = arcLength;

                double finalFeature = (1000.0*perimeter * perimeter) / (perimeterROI *perimeterROI);

                if (finalFeature > 600.0)
                    featureVectorList.Add(finalFeature);
                //featureVector[i] = finalFeature;
                
            }

            for (int i = 0; i < featureVectorList.Count; i++) 
            {
                featureVector[i] = featureVectorList[i];                
            }
            return featureVector;
        }
        public static double[] DefectAreaDistribution(List<ConvexDefects> cdList) 
        {
            List<PointDoublePair> pdpList = new List<PointDoublePair>();

            cdList.ForEach( cd => 
                pdpList.Add(new PointDoublePair(Geometry.TriangularArea(cd.StartPt, cd.EndPt, cd.DefectPt), cd.DefectPt)));

            pdpList.Sort((a, b) => (a.DefectPt.X).CompareTo(b.DefectPt.X));

            List<double> defectAreas = new List<double>();

            pdpList.ForEach(pdp => defectAreas.Add(pdp.Area));

            double[] defectAreasFinal = new double[4] { 0, 0, 0, 0 };

            int iterLimit = defectAreasFinal.Length >= defectAreas.Count ?
                defectAreasFinal.Length : defectAreas.Count;

            // get sum of areas
            double sum = 100.0;
            //double sum = defectAreas.Sum();

            for (int i = 0; i < iterLimit; i++) 
            {
                defectAreasFinal[i] = defectAreas[i] / sum;
            }
            //return defectAreas.ToArray();
            return defectAreasFinal;
        }
        public static double Curvature(Vector2D firstDerivative, Vector2D secondDerivative) 
        {
            double numerator = Math.Abs(firstDerivative.X * secondDerivative.Y -
                firstDerivative.Y * secondDerivative.X);

            double x2 = firstDerivative.X * firstDerivative.X;
            double y2 = firstDerivative.Y * firstDerivative.Y;

            double POWER = 1.5;
            double denominator = Math.Pow(x2 + y2, POWER);

            return numerator / denominator;

        }
        public static double AverageBendingEnergy(VectorOfPoint vp) 
        {
            //double arcLength = Math.Abs(CvInvoke.ArcLength(vp, true));

            List<Vector2D> firstDerivativeList = GetDerivativeList(vp);
            List<Vector2D> secondDerivativeList = GetDerivativeList(firstDerivativeList);
            List<double> sumList = new List<double>();
            int limit = firstDerivativeList.Count > secondDerivativeList.Count
                ? secondDerivativeList.Count : firstDerivativeList.Count;

            for (int i = 0; i < limit; i++) 
            {
                Vector2D firstDerivativePt = firstDerivativeList[i];
                Vector2D secondDerivativePt = secondDerivativeList[i];

                double curvature = Curvature(firstDerivativePt, secondDerivativePt);
                sumList.Add(curvature * curvature);
            }

            double mean = Mean(sumList);
            double sigma = Sigma(sumList, mean);

            return sigma / mean;
        }
        public static List<Vector2D> GetDerivativeList(List<Vector2D> vl) 
        {
            List<Vector2D> derivativeList = new List<Vector2D>();

            for (int i = 0; i < vl.Count - 1; i++) 
            {
                int x0 = vl[i].X;
                int x1 = vl[i + 1].X;

                int y0 = vl[i].Y;
                int y1 = vl[i + 1].Y;

                int delX = x1 - x0;
                int delY = y1 - y0;

                derivativeList.Add(new Vector2D(delX, delY));
            
            }

            return derivativeList;
        }
        public static List<Vector2D> GetDerivativeList(VectorOfPoint vp)
        {
            List<Vector2D> derivativeList = new List<Vector2D>();
            for (int i = 0; i < vp.Size - 1; i++)
            {
                int x0 = vp[i].X;
                int x1 = vp[i + 1].X;

                int y0 = vp[i].Y;
                int y1 = vp[i + 1].Y;

                int delX = x1 - x0;
                int delY = y1 - y0;

                derivativeList.Add(new Vector2D(delX, delY));
            }

            return derivativeList;
        }
        public static double Solidity(VectorOfPoint contour, VectorOfInt convexHull) 
        {
            Point[] ptArray = new Point[convexHull.Size];

            for (int i = 0; i < convexHull.Size; i++)
            {
                Point pt = contour[convexHull[i]];
                ptArray[i] = pt;
            }

            VectorOfPoint vpHull = new VectorOfPoint(ptArray);

            double contourArea = CvInvoke.ContourArea(contour);

            VectorOfPoint polyDP = new VectorOfPoint();

            CvInvoke.ApproxPolyDP(vpHull, polyDP, 0.001, true);

            double hullArea = Math.Abs(CvInvoke.ContourArea(polyDP));

            return contourArea / hullArea;
        
        }
        public static double Solidity(VectorOfPoint contour, VectorOfPoint convexHull) 
        {
            double contourArea = CvInvoke.ContourArea(contour);

            VectorOfPoint vpHull = new VectorOfPoint();

            CvInvoke.ApproxPolyDP(convexHull, vpHull, 0.001, true);

            double hullArea = Math.Abs(CvInvoke.ContourArea(vpHull));

            return contourArea / hullArea;
        }
        public static double Convexity(VectorOfPoint contour, VectorOfInt convexHull)
        {
            Point[] ptArray = new Point[convexHull.Size];

            for (int i = 0; i < convexHull.Size; i++) 
            {
                Point pt = contour[convexHull[i]];
                ptArray[i] = pt;
            }

            VectorOfPoint vpHull = new VectorOfPoint(ptArray);

            double arcLengthContour = CvInvoke.ArcLength(contour, true);
            double arcLengthHull = CvInvoke.ArcLength(vpHull, true);
            double convexity = arcLengthHull / arcLengthContour;
            return convexity;
        }
        public static double Convexity(VectorOfPoint contour, VectorOfPoint convexHull) 
        {
            double arcLengthContour = CvInvoke.ArcLength(contour, true);
            double arcLengthHull = CvInvoke.ArcLength(convexHull, true);
            double convexity = arcLengthHull / arcLengthContour;
            return convexity;
        }
        public static double CircularRatioPerim(VectorOfPoint contour) 
        {
            double shapeArea = Math.Abs(CvInvoke.ContourArea(contour));
            double perimeter = CvInvoke.ArcLength(contour, true);

            return shapeArea / (perimeter * perimeter);
        }

        public static double CircularRatioVA(VectorOfPoint contour) 
        {
            MCvMoments moments = CvInvoke.Moments(contour);
            int xCenter = (int)(moments.M10 / moments.M00);
            int yCenter = (int)(moments.M01 / moments.M00);

            Point centroid = new Point(xCenter, yCenter);

            List<double> listOfDist = new List<double>();

            for (int i = 0; i < contour.Size; i++)
            {
                double distanceNow = Geometry.Distance(contour[i], centroid);
                listOfDist.Add(distanceNow);
            }

            double mean = Mean(listOfDist);

            double sigma = Sigma(listOfDist, mean);

            return sigma / mean;

        }

        #endregion

        public static double MinLD(VectorOfPoint vp, List<ConvexityDefect> cd) 
        {
            System.Drawing.Point ptMid = Geometry.GetMidPoint(vp[cd[0].Start], vp[cd[0].End]);

            double minDist = Geometry.Distance(ptMid, vp[cd[0].Point]);

            for (int i = 1; i < cd.Count; i++) 
            {
                ptMid = Geometry.GetMidPoint(vp[cd[i].Start], vp[cd[i].End]);
                double minDistTmp = Geometry.Distance(ptMid, vp[cd[i].Point]);

                if (minDist > minDistTmp) minDist = minDistTmp;
                
            }

            return minDist;
        }
        

        public static double Mean(List<double> vals) 
        {
            double sumTot = 0.0;

            vals.ForEach(v => sumTot += v);

            double n = (double)vals.Count;

            return sumTot;

        }

        public static double Mean(LinkedList<double> vals)
        {
            double sumTot = 0.0;

            foreach (double v in vals)
                sumTot += v;
            
            double n = (double)vals.Count;

            return sumTot;

        }

        public static double SigmaBiased(List<double> vals) 
        {
            double mean = Mean(vals);
            double n = (double)vals.Count;
            double x = 0.0;

            vals.ForEach(d =>  x += (d - mean) * (d - mean));

            return Math.Sqrt(x / (n));
        }

        public static double Sigma(List<double> vals, double mean)
        {
            double n = (double)vals.Count;
            double x = 0.0;

            vals.ForEach(d => x += (d - mean) * (d - mean));

            return Math.Sqrt(x / (n - 1));
        }

        public static double Sigma(LinkedList<double> vals) 
        {
            double mean = Mean(vals);
            double n = (double)vals.Count - 1;
            double x = 0.0;

            //vals.ForEach(d => x += (d - mean) * (d - mean));

            foreach (double d in vals)
                x += (d - mean) * (d - mean);

            return Math.Sqrt(x / n);
        
        }

        public static double RadiusAve(System.Drawing.Point pt, List<ConvexityDefect> cdList, VectorOfPoint vp) 
        {
            double sumOfDist = 0.0;
            double numOfSample = (double)cdList.Count;

            foreach (ConvexityDefect cd in cdList)
                sumOfDist += Geometry.Distance(pt, vp[cd.Point]);


            return sumOfDist / numOfSample;
        }

        public static double CoeffOfVariation(List<double> list)
        {
            double result = 0.0;

            double sumOfAll = 0.0;
            double n = list.Count;

            foreach (double val in list) sumOfAll += val;

            double mu = sumOfAll / n;

            double sumOfDiffs = 0.0;

            foreach (double val in list) sumOfDiffs = (val - mu) * (val - mu);

            double sqrd = sumOfDiffs / (n - 1);

            result = Math.Sqrt(sqrd);

            return result/mu;
        }
    }

    public class ConnectedPair 
    {
        public Point Pt1 { get; set; }
        public Point Pt2 { get; set; }

        public ConnectedPair(Point pt1, Point pt2) 
        {
            Pt1 = pt1;
            Pt2 = pt2;
        }

        public ConnectedPair() { }
    }

    

    public class ConvexDefect 
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Point OrthogonalPoint { get; set; }
        public Point ContourPoint { get; set; }
        public double Area { get; set; }
        public double OrthogonalDist { get; set; }

        public ConvexDefect() 
        {
            // empty constructor
        }

        public ConvexDefect(Point startPoint, Point endPoint, Point orthogonalPoint, Point contourPoint) 
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            OrthogonalPoint = orthogonalPoint;
            ContourPoint = contourPoint;
            OrthogonalDist = Geometry.Distance(OrthogonalPoint, ContourPoint);
            SolveArea();
        }

        //private void 
        private void SolveArea() 
        {
            double b = Geometry.Distance(StartPoint, EndPoint);
            Area = 0.5 * b * OrthogonalDist;

        }

        


    

    }

    public class PointPair
    {
        public Point pt1 { get; set; }
        public Point pt2 { get; set; }
    }

    public class ContourCutter 
    {
        public List<Point> Contour1 { get; set; }
        public List<Point> Contour2 { get; set; }

        //public SetContourLeft()
        //{
            
        //}
    
    }

    public class ModifiedRotatedRect 
    {
        public Point Pul { get; set; }
        public Point Pur { get; set; }
        public Point Pll { get; set; }
        public Point Plr { get; set; }

        public PointPair UpperMost { get; set; }
        public PointPair LowerMost { get; set; }

        public double ShortestDist() 
        {
            double dist1 = Geometry.Distance(Pul, Pur);
            double dist2 = Geometry.Distance(Pul, Pll);

            if (dist1 < dist2) return dist1;
            else return dist2;
        }

        private PointF ToPointF(Point pt) 
        {
            return new PointF(pt.X, pt.Y);
        }
        public Rectangle ToRect(Size size) 
        {
            //int width = Math.Abs(Pur.X - Pll.X);
            //int height = Math.Abs(Plr.Y - P)

            // find left most and right most

            int MIN_WIDTH = 1;
            int MIN_HEIGHT = 1;
            int MAX_WIDTH = size.Width - 1;
            int MAX_HEIGHT = size.Height - 1;

            int leftMost = 0;
            int rightMost = 0;
            int upMost = 0;
            int lowMost = 0;

            
            // find Left Most and Right Most
            if (Pul.X < Pll.X)
            {
                leftMost = Pul.X;
                rightMost = Plr.X;

                if (leftMost < MIN_WIDTH) leftMost = MIN_WIDTH;
                if (rightMost > MAX_WIDTH) rightMost = MAX_WIDTH; 

            }
            else 
            {
                leftMost = Pll.X;
                rightMost = Pur.X;

                if (leftMost < MIN_WIDTH) leftMost = MIN_WIDTH;
                if (rightMost > MAX_WIDTH) rightMost = MAX_WIDTH; 

            }

            if (Pul.Y < Pur.Y)
            {
                upMost = Pul.Y;
                lowMost = Plr.Y;

                if (upMost < MIN_HEIGHT) upMost = MIN_HEIGHT;
                if (lowMost > MAX_HEIGHT) lowMost = MAX_HEIGHT;

            }
            else 
            {
                upMost = Pur.Y;
                lowMost = Pll.Y;

                if (upMost < MIN_HEIGHT) upMost = MIN_HEIGHT;
                if (lowMost > MAX_HEIGHT) lowMost = MAX_HEIGHT;
            }

            int xLoc = leftMost;
            int yLoc = upMost;

            return new Rectangle(xLoc, yLoc, Math.Abs(rightMost - leftMost), Math.Abs(lowMost - upMost));

        }

        private static Point GetIntersectingPoint(Point pt1, Point pt2, Point pc) 
        {
            double y1 = pt1.Y;
            double y2 = pt2.Y;
            double yc = pc.Y;

            double x1 = pt1.X;
            double x2 = pt2.X;
            double xc = pc.X;
            
            double delY = y1 - y2;
            double delX = x1 - x2;

            if (delX == 0.0) return new Point((int)x1, (int)y1 + pc.Y);
            else 
            {
                double m = delY / delX;
                double b = y1 - m * x1;
                double b1 = yc + xc / m;

                double xInter = (b1 - b) * m / (m * m + 1);
                double yInter = m * xInter + b;

                return new Point((int)xInter, (int)yInter);
            
            }
                
        }

        public static ModifiedRotatedRect Cut(Point ptUpLeft, Point ptUpRight, Point ptLowLeft, Point ptLowRight, Point pc, bool reverse = false) 
        {
            
            Point pIntersectLeft = GetIntersectingPoint(ptUpLeft, ptLowLeft, pc);
            Point plntersectRight = GetIntersectingPoint(ptUpRight, ptLowRight, pc);

            List<Point> lv = new List<Point>();

            if (reverse)
            {
                lv.Add(pIntersectLeft);
                lv.Add(plntersectRight);
                lv.Add(ptLowLeft);
                lv.Add(ptLowRight);
            }
            else 
            {
                lv.Add(ptUpLeft);
                lv.Add(ptUpRight);
                lv.Add(pIntersectLeft);
                lv.Add(plntersectRight);
            }
            return new ModifiedRotatedRect(lv); 
           
        
        }
        public static ModifiedRotatedRect Cut(ModifiedRotatedRect source, Point pc)
        {
            double distanceUpDown = Geometry.Distance(source.Pul, source.Pll);
            double distanceLeftRight = Geometry.Distance(source.Pul, source.Pur);

            Point ptL1 = new Point();
            Point ptL2 = new Point();

            Point ptR1 = new Point();
            Point ptR2 = new Point();

            if (distanceUpDown >= distanceLeftRight)
            {
                ptL1 = source.Pul;
                ptL2 = source.Pll;

                ptR1 = source.Pur;
                ptR2 = source.Plr;
            }
            else 
            {
                ptL1 = source.Pul;
                ptL2 = source.Pur;

                ptR1 = source.Pll;
                ptR2 = source.Plr;
            }

            
                Point pllNew = GetIntersectingPoint(ptL1, ptL2, pc);
                Point plrNew = GetIntersectingPoint(ptR1, ptR2, pc);

                List<Point> lv = new List<Point>();
                lv.Add(source.Pul);
                lv.Add(source.Pur);
                lv.Add(pllNew);
                lv.Add(plrNew);

                return new ModifiedRotatedRect(lv); 
            
        }

        public ModifiedRotatedRect() { }

        public ModifiedRotatedRect(List<Point> pt) 
        {
            if (pt.Count == 4)
            {
                List<Point> lv = pt;
                lv.Sort((a, b) => (a.Y).CompareTo(b.Y));

                UpperMost = new PointPair();
                LowerMost = new PointPair();

                UpperMost.pt1 = new Point((int)lv[0].X, (int)lv[0].Y);
                UpperMost.pt2 = new Point((int)lv[1].X, (int)lv[1].Y);
                LowerMost.pt1 = new Point((int)lv[2].X, (int)lv[2].Y);
                LowerMost.pt2 = new Point((int)lv[3].X, (int)lv[3].Y);

                DesignatePoints();
            }
            else 
            {
                throw new Exception("Vertices must be equal to four.");
            }
        }

        public ModifiedRotatedRect(RotatedRect rect)
        {
            if (rect.GetVertices().Length == 4)
            {

                List<PointF> lv = rect.GetVertices().ToList();

                // sort the by Y axis from lowest to highest
                lv.Sort((a, b) => (a.Y).CompareTo(b.Y));

                UpperMost = new PointPair();
                LowerMost = new PointPair();

                UpperMost.pt1 = new Point((int)lv[0].X, (int)lv[0].Y);
                UpperMost.pt2 = new Point((int)lv[1].X, (int)lv[1].Y);
                LowerMost.pt1 = new Point((int)lv[2].X, (int)lv[2].Y);
                LowerMost.pt2 = new Point((int)lv[3].X, (int)lv[3].Y);

                DesignatePoints();
            }
            else 
            {
                throw new Exception("Vertices must be equal to four");
            }
        }

        private void DesignatePoints() 
        {
            if (UpperMost.pt1.X < UpperMost.pt2.X)
            {
                Pul = UpperMost.pt1;
                Pur = UpperMost.pt2;
            } 
            else
            {
                Pul = UpperMost.pt2;
                Pur = UpperMost.pt1;
            }

            if (LowerMost.pt1.X < LowerMost.pt2.X)
            {
                Pll = LowerMost.pt1;
                Plr = LowerMost.pt2;
            }
            else 
            {
                
                Pll = LowerMost.pt2;
                Plr = LowerMost.pt1;
            
            }
        }

    }


}
