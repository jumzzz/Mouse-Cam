using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hand_Virtual_Mouse
{
    using System;
    using System.Collections.Generic;
    using Emgu.CV.Util;
    /// <summary>
    ///   Convex Hull Defects Extractor.
    /// </summary>
    /// 

    public class ConvexDefects 
    {
        public System.Drawing.Point StartPt { get; set; }
        public System.Drawing.Point EndPt { get; set; }
        public System.Drawing.Point DefectPt { get; set; }

        public double StartEndDist { get; set; }

        public ConvexDefects(System.Drawing.Point startPt, 
            System.Drawing.Point endPt, 
            System.Drawing.Point defectPt)
        {
            StartPt = startPt;
            EndPt = endPt;
            DefectPt = defectPt;
            StartEndDist = Geometry.Distance(StartPt, EndPt);
        }
    }

    public class ConvexHullDefects
    {

        /// <summary>
        /// Gets or sets the minimum depth which characterizes a convexity defect.
        /// </summary>
        /// 
        /// <value>The minimum depth.</value>
        /// 
        public double MinimumDepth { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConvexHullDefects"/> class.
        /// </summary>
        /// 
        /// <param name="minDepth">The minimum depth which characterizes a convexity defect.</param>
        /// 
        public ConvexHullDefects(double minDepth)
        {
            this.MinimumDepth = minDepth;
        }

        /// <summary>
        ///   Finds the convexity defects in a contour given a convex hull.
        /// </summary>
        /// 
        /// <param name="contour">The contour.</param>
        /// <param name="convexHull">The convex hull of the contour.</param>
        /// <returns>A list of <see cref="ConvexityDefect"/>s containing each of the
        /// defects found considering the convex hull of the contour.</returns>
        /// 
        

        public static int FindMaxDefectIndex(List<ConvexityDefect> listOfDefect, VectorOfPoint vp) 
        {
            double areaMax = 0.0;
            int indexFinal = 0;

            for (int i = 0; i < listOfDefect.Count; i++) 
            {
                System.Drawing.Point pt1 = vp[listOfDefect[i].Start];
                System.Drawing.Point pt2 = vp[listOfDefect[i].End];
                System.Drawing.Point pt3 = vp[listOfDefect[i].Point];

                double areaCurr = Geometry.TriangularArea(pt1, pt2, pt3);

                if (areaMax < areaCurr) 
                {
                    areaMax = areaCurr;
                    indexFinal = i;
                }
            }

            return indexFinal;
            
        }

        //private static List<AForge.IntPoint> AcquireStartEndPts(VectorOfPoint vp, List<ConvexityDefect> listCD) 
        //{
        //    List<AForge.IntPoint> lp = new List<AForge.IntPoint>();

        //    foreach (ConvexityDefect cd in listCD) 
        //    {
        //        AForge.IntPoint ptStart = new IntPoint(vp[cd.Start].X, vp[cd.Start].Y);
        //        AForge.IntPoint ptEnd = new IntPoint(vp[cd.End].X, vp[cd.End].Y);

        //        lp.Add(ptStart);
        //        lp.Add(ptEnd);
        //    }

        //    return lp;
        //}

        //private static List<AForge.IntPoint> InscribedPolygonApprox(VectorOfPoint vp, List<ConvexityDefect> listCD, List<AForge.IntPoint> convexList)
        //{
        //    List<AForge.IntPoint> listStartEnd = AcquireStartEndPts(vp, listCD);

        //    List<AForge.IntPoint> inscribedList = new List<IntPoint>();

        //    foreach (IntPoint ip in convexList) 
        //    {
        //        if (!listStartEnd.Contains(ip))
        //            inscribedList.Add(ip);
        //    }

        //    foreach (ConvexityDefect cd in listCD) 
        //    {
        //        System.Drawing.Point pt = vp[cd.Point];
        //        AForge.IntPoint intPt = ImgProc.PointToIntPoint(pt);
        //        inscribedList.Add(intPt);
        //    }

        //    return inscribedList;

        //}

        //public static AForge.Point CentroidInscIntPoint(VectorOfPoint vp, List<ConvexityDefect> listCD, List<AForge.IntPoint> convexList) 
        //{
        //    return PointsCloud.GetCenterOfGravity(InscribedPolygonApprox(vp, listCD, convexList));
        //}


    }

    public class ConvexityDefect
    {

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConvexityDefect"/> class.
        /// </summary>
        /// 
        /// <param name="point">The most distant point from the hull.</param>
        /// <param name="start">The starting index of the defect in the contour.</param>
        /// <param name="end">The ending index of the defect in the contour.</param>
        /// <param name="depth">The depth of the defect (highest distance from the hull to
        /// any of the contour points).</param>
        /// 
        public ConvexityDefect(int point, int start, int end, double depth)
        {
            this.Point = point;
            this.Start = start;
            this.End = end;
            this.Depth = depth;
        }

        /// <summary>
        ///   Gets or sets the starting index of the defect in the contour.
        /// </summary>
        /// 
        public int Start { get; set; }

        /// <summary>
        ///   Gets or sets the ending index of the defect in the contour.
        /// </summary>
        /// 
        public int End { get; set; }

        /// <summary>
        ///   Gets or sets the most distant point from the hull characterizing the defect.
        /// </summary>
        /// 
        /// <value>The point.</value>
        /// 
        public int Point { get; set; }

        /// <summary>
        ///   Gets or sets the depth of the defect (highest distance
        ///   from the hull to any of the points in the contour).
        /// </summary>
        /// 
        public double Depth { get; set; }

    }

}
