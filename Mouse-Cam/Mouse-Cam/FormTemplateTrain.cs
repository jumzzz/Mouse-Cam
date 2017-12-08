using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

#region EMGUCV_LIBS
using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Util;
#endregion

#region AFORGE_LIBS

//using AForge;
//using AForge.Math.Geometry;
//using Accord.Imaging.Filters;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;



#endregion



namespace Hand_Virtual_Mouse
{
    public partial class FormTemplateTrain : Form
    {
        #region PRIVATE_FIELDS

        #region ML_OBJECTS

        private MulticlassSupportVectorMachine _svmMachineInit;
        private MulticlassSupportVectorLearning _svmTeacherInit;

        private MulticlassSupportVectorMachine _svmMachineHand;
        private MulticlassSupportVectorLearning _svmTeacherHand;

        private KNearestNeighbors _templateGesture;

        private const string PATH_FEATURES_DEFECT = "Feature Extraction\\DEFECT_FEATURES\\defect_features.csv";
        //private TemplateMatchDecisionTree _tempMatchDecisionTree;
        
        #endregion


        #region INNER_ROI_SEARCH_FIELDS

        private HSVLookUpTable _hsvLookUpTable;
        private List<double> _distanceList = new List<double>();
      
        private Matrix<int> _convex;
        private List<Point> _contourTmp = new List<Point>();
        private List<ConvexDefects> _convexDefectList = new List<ConvexDefects>();
     
        private ModifiedRotatedRect _rotRectMod = new ModifiedRotatedRect();

        private RotatedRect _rectNew;
        private PointF[] _vertices;
               
        private double _ptLftToRight = 0.0;
        private double _ptUpToDown = 0.0;

        private Point _ptUpLeft = new Point();
        private Point _ptUpRight = new Point();
        private Point _ptLowLeft = new Point();
        private Point _ptLowRight = new Point();

        private Point _pc = new Point();
        private Point _pcPrev = new Point();

        private Point _pc1 = new Point();
        private Point _pc2 = new Point();
        private Point _pc1Prev = new Point();
        private Point _pc2Prev = new Point();

        private LinkedList<double> _pc1LinkedList = new LinkedList<double>();
        private LinkedList<double> _pc2LinkedList = new LinkedList<double>();

        private ModifiedRotatedRect _rotRectModNew = new ModifiedRotatedRect();
        private ModifiedRotatedRect _rotRectEval1 = new ModifiedRotatedRect();
        private ModifiedRotatedRect _rotRectEval2 = new ModifiedRotatedRect();

        private Rectangle _rectROIEval1 = new Rectangle();
        private Rectangle _rectROIEval2 = new Rectangle();

        private Mat _matToBeEval1 = new Mat();
        private Mat _matToBeEval2 = new Mat();

        private Mat _matHierachyEval1 = new Mat();
        private Mat _matHierachyEval2 = new Mat();

        private MCvMoments _moment = new MCvMoments();
        private double[] _huMoments;
        private List<double> _features = new List<double>();
          

        private VectorOfVectorOfPoint _contoursEval1 = new VectorOfVectorOfPoint();
        private VectorOfVectorOfPoint _contoursEval2 = new VectorOfVectorOfPoint();

        private int _largestContourIndexEval1;
        private int _largestContourIndexEval2;

        private MCvMoments _momentEval1 = new MCvMoments();
        private MCvMoments _momentEval2 = new MCvMoments();

        private List<double> _huMomentsEval1 = new List<double>();
        private List<double> _huMomentEval2 = new List<double>();

        private int _predictionEval1;
        private int _predictionEval2;

        private Rectangle _rectWithin = new Rectangle();
        private Point _actualLoc = new Point();
        private Rectangle _actualROI = new Rectangle();
                
        #endregion
        //

        private Thread _threadKNNTrainer;
        private KnnIO _mlInOut;
        private KNearestNeighbors _knn;

        private MulticlassSupportVectorMachine _machine;
        private MulticlassSupportVectorLearning _teacher;
        private IKernel _kernel;

        private const string PATH_FEATURES_INIT = "Feature Extraction\\feature.csv";
        private const string PATH_FEATURES_HAND = "Feature Extraction\\feature_hand.csv";
        
        private const int CLASSIFICATION_HAND = 0;
        private const int CLASSIFICATION_ARM = 1;
        private const int CLASSIFICATION_HAND2 = 2;

        private const int CLASSIFICATION_HAND_OPEN =  0;
        private const int CLASSIFICATION_HAND_LEFT =  1;
        private const int CLASSIFICATION_HAND_RIGHT = 2;
        private const int CLASSIFICATION_HAND_DOWN =  3;
        private const int CLASSIFICATION_HAND_UP =    4;
        private const int CLASSIFICATION_HAND_HALT =  5;

        private int _classificationInit = 0;
        private int _classificationHand = 3;

        private Image<Gray, Byte> _templateMainOPN;
        private Image<Gray, Byte> _templateMainLFT;
        private Image<Gray, Byte> _templateMainRGT;
        private Image<Gray, Byte> _templateMainHLT;

        private CircleF _circleTrack;
       
        private LinkedList<int> _listOfDeltasX = new LinkedList<int>();
        private LinkedList<int> _listOfDeltasY = new LinkedList<int>();

        private LinkedList<int> _listOfStartDefectPtX = new LinkedList<int>();
        private LinkedList<int> _listOfStartDefectPtY = new LinkedList<int>();

        private int _blurLevelNorm = 0;
        private ConvexHullDefects _chd = new ConvexHullDefects(25.0);
        
        private Pen _pen = new Pen(Color.Red, 2);
        private List<Rectangle> _rectList = new List<Rectangle>();
        private VideoCapture _capture = null;

        
        private Image<Gray, Byte> _imgHistFilter;
        private Image<Gray, Byte> _imgHistFilterClone;
        
        private Image<Bgr, Byte> _frameRT;
        private Image<Bgr, Byte> _frameDefect;
        private Image<Bgr, Byte> _frameTracker;
        

        private Image<Bgr, Byte> _frameImg;
        private double Framesno = 0;        
        private System.Drawing.Point _locCamFrameMain;

        private const string HIST_SKIN_FILE_HSV = "data\\histogram\\histskindatahsv.csv";
        private const string HIST_NONSKIN_FILE_HSV = "data\\histogram\\histnonskindatahsv.csv";

        private const string CPPROP_DIR = "Logging\\ConvexData";

        private const string DIR_LOGGING_POINT = "Logging\\Point";

        #region training_object

        private HSVTree _hsvSkinTree;
        private HSVTree _hsvNSkinTree;
        #endregion


        #region PARAMETERS
        private List<ConvexityDefect> _listCD = new List<ConvexityDefect>();


        private int _indexLargestContour = 0;
        private VectorOfVectorOfPoint _contour = new VectorOfVectorOfPoint();
        private VectorOfPoint _largestContour = new VectorOfPoint();
        private VectorOfPoint _convexHull = new VectorOfPoint();
        private VectorOfInt _hull = new VectorOfInt();
        private Mat _convexityDefect = new Mat();

        private Rectangle _rect;

        private const int X_TOLERANCE = 3;
        private const int Y_TOLERANCE = 2;
        // hand paramete

        
        private System.Drawing.Point _cb = new Point(); // done
        
        private CircleF _circleMax = new CircleF();

        private Rectangle _rectTracker = new Rectangle();

        private Emgu.CV.Structure.MCvScalar _mcvBlue = new Emgu.CV.Structure.MCvScalar(255, 0, 0);
        private Emgu.CV.Structure.MCvScalar _mcvBlack = new Emgu.CV.Structure.MCvScalar(0, 0, 0);
        private Emgu.CV.Structure.MCvScalar _mcvRed = new Emgu.CV.Structure.MCvScalar(0, 0, 255);
        private Emgu.CV.Structure.MCvScalar _mcvGreen = new Emgu.CV.Structure.MCvScalar(0, 255, 0);
        private Emgu.CV.Structure.MCvScalar _mcvYellow = new Emgu.CV.Structure.MCvScalar(51.0, 255.0, 255.0);
        #endregion

        
        private bool _initView = true;

        #region MOUSE_PARAMETERS

        private const int MIN_VAL_INCREMENT = 15;

        #endregion

        #region FILE_CONSTANTS
        private const string TMP_OPN_DIR = "data\\Template Train\\Open";
        private const string TMP_LFT_DIR = "data\\Template Train\\Left";
        private const string TMP_RGT_DIR = "data\\Template Train\\Right";
        private const string TMP_HLT_DIR = "data\\Template Train\\Halt";
        private const string TMP_SDN_DIR = "data\\Template Train\\ScrollDown";
        private const string TMP_SUP_DIR = "data\\Template Train\\ScrollUp";

        private string _currentCropDir = "";


        #endregion


        #region ML_INIT

        private void KNNInit() 
        {
            KnnIO knnIO = new KnnIO(PATH_FEATURES_DEFECT, 1.0);

            _templateGesture = new KNearestNeighbors(4, knnIO.Inputs, knnIO.Outputs);
        
        }

        private void InitSVMInit() 
        {
            //_mlInOut = new TemplateIO(PATH_FEATURES_INIT,5000.0);
            ////_tempMatchDecisionTree = new TemplateMatchDecisionTree();
            //IKernel kernel = new Gaussian(150.0);
            //_svmMachineInit = new MulticlassSupportVectorMachine(inputs: 7, kernel: kernel, classes: 3);
            //_svmTeacherInit = new MulticlassSupportVectorLearning(_svmMachineInit, _mlInOut.Inputs, _mlInOut.Outputs);

            //_svmTeacherInit.Algorithm = (svm, classInputs, classOutputs, i, j) =>
            //    new SequentialMinimalOptimization(svm, classInputs, classOutputs);

            //double error = _svmTeacherInit.Run(); // output should be 0

            //_svmMachineInit.Save("Feature Extraction\\blob_analyzer.xml");
            _svmMachineInit = MulticlassSupportVectorMachine.Load("Feature Extraction\\blob_analyzer.xml");

        }

        private void InitSVMHand() 
        {
            //MLInOut mlInOut = new MLInOut(PATH_FEATURES_HAND);

            //IKernel kernel = new Gaussian(150.0);

            //_svmMachineHand = new MulticlassSupportVectorMachine(inputs: 7, kernel: kernel, classes: 6);
            //_svmTeacherHand = new MulticlassSupportVectorLearning(_svmMachineHand, mlInOut.Inputs, mlInOut.Outputs);

            //_svmTeacherHand.Algorithm = (svm, classInputs, classOutputs, i, j) =>
            //    new SequentialMinimalOptimization(svm, classInputs, classOutputs);

            //double error = _svmTeacherHand.Run();

            _svmMachineHand = MulticlassSupportVectorMachine.Load("Feature Extraction\\HAND_FEATURES\\template.xml");

            Console.WriteLine("Debugger");
        }

        private void InitSVMGesture() 
        {
        

        }
        #endregion


        #region CONSTRUCTOR
        public FormTemplateTrain()
        {
            InitializeComponent();
            KNNInit();
            InitSVMInit();                 // initialize svm
            InitSVMHand();

            //_hsvDictSkin = new HSVDictionary(HIST_SKIN_FILE_HSV);
            _hsvLookUpTable = new HSVLookUpTable(HIST_SKIN_FILE_HSV);

            _frameRT = new Image<Bgr, Byte>(pictureBoxCam.Size);
            _locCamFrameMain = pictureBoxCam.Location;

            labelDilate.Text = trackBarDilate.Value.ToString();
            labelErode.Text = trackBarErode.Value.ToString();
            labelFreq.Text = trackBarFreq.Value.ToString();

            //if (radioButtonOpen.Checked) _currentCropDir = TMP_OPN_DIR;
            //else if (radioButtonLeft.Checked) _currentCropDir = TMP_LFT_DIR;
            //else if (radioButtonRight.Checked) _currentCropDir = TMP_RGT_DIR;

            //string dirOPN = "data\\Template Train\\Open\\main\\main.bmp";
            //string dirLFT = "data\\Template Train\\Left\\main\\main.bmp";
            //string dirRGT = "data\\Template Train\\Right\\main\\main.bmp";

            //_templateMainOPN = new Image<Gray, Byte>(new Bitmap(dirOPN));
            //_templateMainLFT = new Image<Gray, Byte>(new Bitmap(dirLFT));
            //_templateMainRGT = new Image<Gray, Byte>(new Bitmap(dirRGT));

            labelFreq.Text = trackBarFreq.Value.ToString();
            labelErode.Text = trackBarErode.Value.ToString();
            labelDilate.Text = trackBarDilate.Value.ToString();


            for (int i = 0; i < 50; i++) 
            {
                _pc1LinkedList.AddLast(0.0);
                _pc2LinkedList.AddLast(0.0);
            }


            UpdateHandDirsAndLabels();
         
        }
        #endregion        
        private void ErrorMessageMain(Exception err)
        {
            String message = String.Concat("Message: \n", err.Message);
            String stacktrace = String.Concat("Stack Trace: \n", err.StackTrace);
            String indexContourStr = "Index of Largest Contour: " + _indexLargestContour.ToString();

            String rectRoiStr = "ROI Rect Prop: " + _rect.ToString();
            String cbStr = "Circle cb: " + _cb.ToString();
            String countContourStr = "Count of Contour: " + _contour.Size.ToString();




            MessageBox.Show(String.Concat(message, "\n", stacktrace, "\n", indexContourStr
                , "\n", rectRoiStr
                , "\n", cbStr
                , "\n", countContourStr), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Point[] SmoothenContour(Point[] largestCont, int smoother) 
        {
            
            int x = 0;
            int y = 0;

            int m0X = 0;
            int m0Y = 0;
            int m1X = 0;
            int m1Y = 0;

            for (int i = 0; i < largestCont.Length; i++) 
            {
                m1X = m0X + (largestCont[i].X - m0X) / smoother;
                m1Y = m0Y + (largestCont[i].Y - m0Y) / smoother;

                largestCont[i] = new Point(m1X, m1Y);
                m0X = m1X;
                m0Y = m1Y;     
            }

            return largestCont;

        }
        private void ProcessROI()
        {
            _largestContour = _contour[_indexLargestContour];
            _distanceList = new List<double>();


            CvInvoke.ConvexHull(_largestContour, _hull, true, false);
            CvInvoke.ConvexityDefects(_largestContour, _hull, _convexityDefect);




            if (!_convexityDefect.IsEmpty) 
            {
                _convex = new Matrix<int>(_convexityDefect.Rows, _convexityDefect.Cols, _convexityDefect.NumberOfChannels);

                _convexityDefect.CopyTo(_convex);

                _convexDefectList = new List<ConvexDefects>();
                List<ConvexDefects> convexCorrected1 = new List<ConvexDefects>();


                
                for (int i = 0; i < _convex.Rows; i++) 
                {
                    // do not touch
                    int startIdx = _convex.Data[i, 0];
                    int endIdx = _convex.Data[i, 1];
                    int pointIdx = _convex.Data[i, 2];

                    Point startPt = _largestContour[startIdx];
                    Point endPt = _largestContour[endIdx];
                    Point defectPt = _largestContour[pointIdx];

                    _convexDefectList.Add(new ConvexDefects(startPt, endPt, defectPt));
                }

                
                for (int i = 1; i < _hull.Size; i++)
                {
                    //CvInvoke.Line(_frameRT, _largestContour[_hull[i - 1]], _largestContour[_hull[i]], _mcvRed, 2);
                    _distanceList.Add(Geometry.Distance(_largestContour[_hull[i - 1]], _largestContour[_hull[i]]));
                }

                //_convexDefectList.Sort((a, b) => (b.StartEndDist).CompareTo(a.StartEndDist));  // para saan to? get highest 2 dist
                                    
                //CvInvoke.Line(_frameRT, _convexDefectList[0].StartPt, _convexDefectList[0].EndPt, _mcvRed);
                //CvInvoke.Line(_frameRT, _convexDefectList[1].StartPt, _convexDefectList[1].EndPt, _mcvRed);
                //CvInvoke.Circle(_frameRT, _convexDefectList[0].DefectPt, 10, _mcvYellow, 2);
                //CvInvoke.Circle(_frameRT, _convexDefectList[1].DefectPt, 10, _mcvYellow, 2);

                if (_convexDefectList.Count > 3)
                {
                    _rectNew = CvInvoke.MinAreaRect(_largestContour);

                    _vertices = _rectNew.GetVertices();

                    _rectTracker = _rectNew.MinAreaRect();


                    int newWidth = (int)_rectNew.Size.Width;

                    for (int i = 0; i < 4; i++)
                    {
                        Point pt1 = new Point((int)_vertices[i].X, (int)_vertices[i].Y);
                        Point pt2 = new Point((int)_vertices[(i + 1) % 4].X, (int)_vertices[(i + 1) % 4].Y);

                        //CvInvoke.Line(_frameRT, pt1, pt2, _mcvYellow, 2);

                        List<Point> pointPair = new List<Point>();

                        pointPair.Add(pt1);
                        pointPair.Add(pt2);

                        if ((int)Geometry.Distance(pt1, pt2) < newWidth)
                            newWidth = (int)Geometry.Distance(pt1, pt2);

                    }


                    //CvInvoke.Line(_frameRT, _largestContour[_hull[0]], _largestContour[_hull[_hull.Size - 1]], _mcvRed, 2);
                    _distanceList.Add(Geometry.Distance(_largestContour[_hull[0]], _largestContour[_hull[_hull.Size - 1]]));

                    _moment = CvInvoke.Moments(_largestContour);
                    _huMoments = _moment.GetHuMoment();

                    _rotRectMod = new ModifiedRotatedRect(_rectNew);

                    double[] scaledFeatureVector = ScaleValues(_huMoments, 5000.0);

                    int prediction = _svmMachineInit.Compute(ScaleValues(_huMoments, 5000.0), MulticlassComputeMethod.Voting);

                    string state = "";


                    if (prediction == CLASSIFICATION_HAND || prediction == CLASSIFICATION_HAND2)
                    {

                        int yLabel = 0;
                        state = "Hand".ToUpper() + " Angle: " + Math.Round(_rectNew.Angle, 3).ToString();

                        //for (int i = 0; i < _huMoments.Length; i++)
                        //{
                        //    CvInvoke.PutText(_frameRT,
                        //    "I[" + i.ToString() + "]: " + Math.Round(5000.0 * (_huMoments[i]), 10).ToString(),
                        //     new Point(10, yLabel += 20), FontFace.HersheyPlain, 0.9, _mcvBlue, 2);
                        //}

                        if (checkBoxRecordFeatures.Checked) LogFeatures(PATH_FEATURES_INIT, _features, _classificationInit);

                        //CvInvoke.PutText(_frameRT, state, _rectTracker.Location, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                        CvInvoke.Rectangle(_frameRT, _rectTracker, _mcvBlack, 2);


                        double[] featureVectorHand = ScaleValues(_huMoments.ToList()
                        .GetRange(0, _svmMachineHand.Inputs).ToArray(), 1000.0);

                        foreach (ConvexDefects cd in convexCorrected1)
                        {
                            Point[] ptDefect = new Point[] { cd.StartPt, cd.EndPt, cd.DefectPt };

                            VectorOfPoint vDefect = new VectorOfPoint(ptDefect);
                            VectorOfPoint vDefectPoly = new VectorOfPoint();
                            CvInvoke.ApproxPolyDP(vDefect, vDefectPoly, 0.0001, true);

                            //CvInvoke.FillConvexPoly(_frameRT, vDefectPoly, _mcvGreen);
                        }

                        //double[] featureDefects = Statistics.DefectFeatureVector()
                        //TemplateFeatures tf = new TemplateFeatures(_huMoments.ToList());

                        convexCorrected1 = Statistics.FilterDefects(_convexDefectList, 20.0, 130.0, _rectTracker, 0.6);
                        foreach (ConvexDefects cd in convexCorrected1)
                        {
                            Point[] ptDefect = new Point[] { cd.StartPt, cd.EndPt, cd.DefectPt };

                            VectorOfPoint vDefect = new VectorOfPoint(ptDefect);
                            VectorOfPoint vDefectPoly = new VectorOfPoint();
                            CvInvoke.ApproxPolyDP(vDefect, vDefectPoly, 0.0001, true);

                            //CvInvoke.FillConvexPoly(_frameRT, vDefectPoly, _mcvGreen);
                        }


                        double shortestDist = _rotRectMod.ShortestDist();
                        double[] featuresDefect = Statistics.DefectFeatureVector(convexCorrected1, shortestDist);



                        Point labelPointHand = new Point((9) * _frameRT.Width / 10, 20);

                        int handPrediction = _templateGesture.Compute(featuresDefect);

                        switch (handPrediction)
                        {
                            case CLASSIFICATION_HAND_OPEN:
                                CvInvoke.PutText(_frameRT, "OPEN", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                            case CLASSIFICATION_HAND_LEFT:
                                CvInvoke.PutText(_frameRT, "LEFT", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                            case CLASSIFICATION_HAND_RIGHT:
                                CvInvoke.PutText(_frameRT, "RIGHT", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                            case CLASSIFICATION_HAND_DOWN:
                                CvInvoke.PutText(_frameRT, "DOWN", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                            case CLASSIFICATION_HAND_UP:
                                CvInvoke.PutText(_frameRT, "UP", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                            case CLASSIFICATION_HAND_HALT:
                                CvInvoke.PutText(_frameRT, "HALT", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                            default:
                                CvInvoke.PutText(_frameRT, "NONE", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                break;
                        }


                        List<double> featuresToBeLogged = new List<double>();

                        featuresToBeLogged.AddRange(_huMoments.ToList());

                        //featuresToBeLogged.Add(convexity);
                        //featuresToBeLogged.Add(circularRatioPerim);
                        //featuresToBeLogged.Add(circularRatioVA);
                        //featuresToBeLogged.Add(solidity);
                        //featuresToBeLogged.Add(aveBendingEnergy);

                        if (checkBoxRecordFeaturesHand.Checked)
                            LogFeatures(PATH_FEATURES_HAND, featuresToBeLogged, _classificationHand);

                        // on recording features of defects on actual hand detected use "_rectTracker" as roi


                        if (checkBoxRecordFeaturesHand.Checked)
                            LogFeatures(PATH_FEATURES_DEFECT, featuresDefect, _classificationHand);


                    }
                    else if (prediction == CLASSIFICATION_ARM)
                    {
                        int yDel = 0;
                        state = "ARM".ToUpper() + " Angle: " + Math.Round(_rectNew.Angle, 3).ToString();

                        _ptLftToRight = Geometry.Distance(_rotRectMod.Pul, _rotRectMod.Pur);
                        _ptUpToDown = Geometry.Distance(_rotRectMod.Pul, _rotRectMod.Pll);


                        if (_ptLftToRight <= _ptUpToDown)
                        {

                            Point pc1Tmp = _convexDefectList[0].DefectPt;
                            Point pc2Tmp = _convexDefectList[1].DefectPt;


                            _pc = pc1Tmp.Y > pc2Tmp.Y ? pc1Tmp : pc2Tmp;

                            _ptUpLeft = _rotRectMod.Pul;
                            _ptUpRight = _rotRectMod.Pur;
                            _ptLowLeft = _rotRectMod.Pll;
                            _ptLowRight = _rotRectMod.Plr;

                            _rotRectEval1 = ModifiedRotatedRect.Cut(_ptUpLeft, _ptUpRight, _ptLowLeft, _ptLowRight, _pc);
                            _rotRectEval2 = ModifiedRotatedRect.Cut(_ptUpLeft, _ptUpRight, _ptLowLeft, _ptLowRight, _pc, true);

                            Size sizeFrame = _frameRT.Size;
                            _rectROIEval1 = _rotRectEval1.ToRect(sizeFrame);
                            _rectROIEval2 = _rotRectEval2.ToRect(sizeFrame);

                            Mat cloneMat1 = _imgHistFilterClone.Clone().Mat;

                            _matToBeEval1 = new Mat(cloneMat1, _rectROIEval1);
                            _contoursEval1 = new VectorOfVectorOfPoint();

                            CvInvoke.FindContours(_matToBeEval1, _contoursEval1, _matHierachyEval1, Emgu.CV.CvEnum.RetrType.External,
                                Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

                            _largestContourIndexEval1 = ImgProc.LargestContourIndex(_contoursEval1);

                            _momentEval1 = CvInvoke.Moments(_contoursEval1[_largestContourIndexEval1]);

                            _huMomentsEval1 = _momentEval1.GetHuMoment().ToList();

                            double[] featureVectorSearch = ScaleValues(_huMomentsEval1.ToArray(), 5000.0);
                            _predictionEval1 = _svmMachineInit.Compute(featureVectorSearch, MulticlassComputeMethod.Elimination);

                            double[] featureVectorHand = ScaleValues(_huMomentsEval1.ToList()
                                .GetRange(0, _svmMachineHand.Inputs).ToArray(), 1000.0);

                            if (_predictionEval1 == CLASSIFICATION_HAND)
                            {
                                _rectWithin = CvInvoke.MinAreaRect(_contoursEval1[_largestContourIndexEval1]).MinAreaRect();
                                _actualLoc = new Point(_rectROIEval1.Location.X + _rectWithin.Location.X,
                                                            _rectROIEval1.Location.Y + _rectWithin.Location.Y);

                                _actualROI = new Rectangle(_actualLoc, _rectWithin.Size);

                                CvInvoke.PutText(_frameRT, state, _actualROI.Location, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                CvInvoke.Rectangle(_frameRT, _actualROI, _mcvBlack, 2);


                                int yLabel = 0;
                                //for (int i = 0; i < _huMomentsEval1.Count; i++)
                                //{
                                //    CvInvoke.PutText(_frameRT,
                                //        "I[" + i.ToString() + "]: " + Math.Round(5000.0 * (_huMomentsEval1[i]), 10).ToString(),
                                //        new Point(10, yLabel += 20), FontFace.HersheyPlain, 0.9, _mcvBlue, 2);
                                //}

                                if (checkBoxRecordFeatures.Checked) LogFeatures(PATH_FEATURES_INIT, _features, _classificationInit);

                                VectorOfPoint largestContourEval1 = _contoursEval1[_largestContourIndexEval1];

                                CircleF circleEnclosed = CvInvoke.MinEnclosingCircle(largestContourEval1);
                                VectorOfInt convexHullInside = new VectorOfInt();

                                CvInvoke.ConvexHull(largestContourEval1, convexHullInside, true, false);

                                Mat convexDefectsInsideOrig = new Mat();

                                CvInvoke.ConvexityDefects(largestContourEval1, convexHullInside, convexDefectsInsideOrig);

                                if (!convexDefectsInsideOrig.IsEmpty)
                                {
                                    Matrix<int> convexDefectInside = new Matrix<int>(convexDefectsInsideOrig.Rows,
                                        convexDefectsInsideOrig.Cols,
                                        convexDefectsInsideOrig.NumberOfChannels);

                                    convexDefectsInsideOrig.CopyTo(convexDefectInside);


                                    List<ConvexDefects> convexDefectListInside = new List<ConvexDefects>();
                                    List<ConvexDefects> convexCorrectedInside = new List<ConvexDefects>();

                                    for (int i = 0; i < convexDefectInside.Rows; i++)
                                    {
                                        // do not touch
                                        int startIdx = convexDefectInside.Data[i, 0];
                                        int endIdx = convexDefectInside.Data[i, 1];
                                        int pointIdx = convexDefectInside.Data[i, 2];
                                        Point startPt = largestContourEval1[startIdx];
                                        Point endPt = largestContourEval1[endIdx];
                                        Point defectPt = largestContourEval1[pointIdx];

                                        Point startPtCorr = new Point(_actualROI.Location.X + startPt.X, _actualROI.Location.Y + startPt.Y);
                                        Point endPtCorr = new Point(_actualROI.Location.X + endPt.X, _actualROI.Location.Y + endPt.Y);
                                        Point defectPtCorr = new Point(_actualROI.Location.X + defectPt.X, _actualROI.Location.Y + defectPt.Y);

                                        convexDefectListInside.Add(new ConvexDefects(startPtCorr, endPtCorr, defectPtCorr));
                                    }

                                    convexCorrectedInside = Statistics.FilterDefects(convexDefectListInside, 20.0, 130.0, _actualROI, 0.6);

                                    foreach (ConvexDefects cd in convexCorrectedInside)
                                    {
                                        Point[] ptDefect = new Point[] { cd.StartPt, cd.EndPt, cd.DefectPt };

                                        VectorOfPoint vDefect = new VectorOfPoint(ptDefect);
                                        VectorOfPoint vDefectPoly = new VectorOfPoint();
                                        CvInvoke.ApproxPolyDP(vDefect, vDefectPoly, 0.0001, true);
                                        //CvInvoke.FillConvexPoly(_frameRT, vDefectPoly, _mcvGreen);

                                    }

                                    double shortestDist = _rotRectEval1.ShortestDist();
                                    double[] featuresDefectInside = Statistics.DefectFeatureVector(convexCorrectedInside, shortestDist);

                                    if (checkBoxRecordFeaturesHand.Checked)
                                        LogFeatures(PATH_FEATURES_DEFECT, featuresDefectInside, _classificationHand);



                                    int handPrediction = _templateGesture.Compute(featuresDefectInside);

                                    Point labelPointHand = new Point((9) * _frameRT.Width / 10, 20);

                                    switch (handPrediction)
                                    {
                                        case CLASSIFICATION_HAND_OPEN:
                                            CvInvoke.PutText(_frameRT, "OPEN", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                        case CLASSIFICATION_HAND_LEFT:
                                            CvInvoke.PutText(_frameRT, "LEFT", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                        case CLASSIFICATION_HAND_RIGHT:
                                            CvInvoke.PutText(_frameRT, "RIGHT", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                        case CLASSIFICATION_HAND_DOWN:
                                            CvInvoke.PutText(_frameRT, "DOWN", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                        case CLASSIFICATION_HAND_UP:
                                            CvInvoke.PutText(_frameRT, "UP", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                        case CLASSIFICATION_HAND_HALT:
                                            CvInvoke.PutText(_frameRT, "HALT", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                        default:
                                            CvInvoke.PutText(_frameRT, "NONE", labelPointHand, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                                            break;
                                    }

                                    List<double> featuresToBeLogged = new List<double>();

                                    featuresToBeLogged.AddRange(_huMomentsEval1);


                                    if (checkBoxRecordFeaturesHand.Checked)
                                        LogFeatures(PATH_FEATURES_HAND, featuresToBeLogged, _classificationHand);



                                }

                            }

                        }
                        else
                        {
                            // choose between points that has greater X
                            if (_convexDefectList[0].DefectPt.X > _convexDefectList[1].DefectPt.X)
                                _pc = _convexDefectList[0].DefectPt;
                            else
                                _pc = _convexDefectList[1].DefectPt;

                            _ptUpLeft = _rotRectMod.Pll;
                            _ptUpRight = _rotRectMod.Pul;
                            _ptLowLeft = _rotRectMod.Pll;
                            _ptLowRight = _rotRectMod.Plr;

                            Rectangle roiRect1 = new Rectangle();
                            Rectangle roiRect2 = new Rectangle();

                        }


                        state = "Arm".ToUpper() + " Angle: " + Math.Round(_rectNew.Angle, 3).ToString();
                    }
                    else if (prediction == CLASSIFICATION_HAND2)
                    {
                        state = "reject".ToUpper() + " Angle: " + Math.Round(_rectNew.Angle, 3).ToString();

                        CvInvoke.PutText(_frameRT, state, _rectTracker.Location, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                        CvInvoke.Rectangle(_frameRT, _rectTracker, _mcvBlack, 2);

                    }

                    else
                        state = "none".ToUpper();

                    if (checkBoxRecordFeatures.Checked) LogFeatures(PATH_FEATURES_INIT, _huMoments.ToList(), _classificationInit);
                    
                }
           

            }
            
        }

        private double[] ScaleValues(double[] list, double scalingFactor) 
        {
            List<double> values = new List<double>();
            list.ToList().ForEach(d => values.Add(scalingFactor * d));

            return values.ToArray();
        }

        private List<Point> ShuffleList(Random rng, List<Point> list) 
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Point value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        private void LogFeatures(string path, double[] featureData, double classification)
        {
            string dataLine = "";

            //featureData.ForEach(d => dataLine += d.ToString() + ",");
            foreach (double d in featureData)
                dataLine += d.ToString() + ",";
            dataLine += classification.ToString();

            //List<string> dataLines = File.ReadAllLines(path).ToList();
            //dataLines.Add(dataLine);

            //File.WriteAllLines(path, dataLines.ToArray());

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(dataLine);
            }
        }

        
        private void LogFeatures(string path, List<double> featureData, double classification) 
        {
            string dataLine = "";

            featureData.ForEach(d => dataLine += d.ToString() + ",");
            dataLine += classification.ToString();
            
            //List<string> dataLines = File.ReadAllLines(path).ToList();
            //dataLines.Add(dataLine);

            //File.WriteAllLines(path, dataLines.ToArray());

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(dataLine);
            }
        }
        private void LocateLargestContourTempMatch()
        {
            _contour = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(_imgHistFilter, _contour, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            _indexLargestContour = ImgProc.LargestContourIndex(_contour);

        }
        private void ApplySkinHistFilter()
        {
            _imgHistFilter = ImgProc.SkinDetectHSV(_hsvLookUpTable, _frameRT, trackBarFreq.Value, trackBarErode.Value, trackBarDilate.Value);
            _imgHistFilterClone = _imgHistFilter.Clone();
        }

        private void ImageInit()
        {
            InitHandParam();

            Framesno = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);


            Image<Bgr, Byte> initImg = _capture.QueryFrame().ToImage<Bgr, Byte>();
            Image<Bgr, Byte> filteredImg = new Image<Bgr, Byte>(initImg.Size);
            _frameRT = new Image<Bgr, Byte>(initImg.Size);
            _frameImg = new Image<Bgr, Byte>(initImg.Size);
           
            if (!IsEven(_blurLevelNorm))
                CvInvoke.BilateralFilter(initImg, filteredImg, _blurLevelNorm, (double)_blurLevelNorm * 2, (double)_blurLevelNorm / 2.0);
            else
                filteredImg = initImg;


            if (checkBoxFlip.Checked)
            {
                CvInvoke.Flip(filteredImg, _frameRT, Emgu.CV.CvEnum.FlipType.Horizontal);
                CvInvoke.Flip(filteredImg, _frameImg, Emgu.CV.CvEnum.FlipType.Horizontal);
            }
            else
            {
                _frameRT = filteredImg;
                _frameImg = filteredImg;
            }

        }
        private void FormHMMTrain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        
        private void InitHandParam()
        {
            _cb = new Point();
        }
        private string GenerateDateString()
        {
            DateTime now = DateTime.Now;
            string s0 = now.Year.ToString();
            string s1 = now.Month.ToString();
            string s2 = now.Day.ToString();
            string s3 = now.Hour.ToString();
            string s4 = now.Minute.ToString();
            string s5 = now.Second.ToString();
            string s6 = now.Millisecond.ToString();
            return String.Concat(s0, s1, s2, s3, s4, s5, s6);
        }
        #endregion

        #region EVENTS

        private void ProcessFrame_Idle(object sender, EventArgs arg)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            VideoRun();
            watch.Stop();
            long elapsedTime = watch.ElapsedMilliseconds;
            long fps = 1000L / elapsedTime;
            labelFPS.Text = fps.ToString();
        }

        private void VideoRun()
        {
            try
            {
                #region INIT_VIEW

                if (_initView)
                {

                    ImageInit();

                    if (_frameRT != null)
                    {

                        ApplySkinHistFilter();

                        if (rbNormal.Checked)
                        {
                            try
                            {
                                LocateLargestContourTempMatch();

                                if (_indexLargestContour > 0)
                                {
                                    ProcessROI();
                                    string dir = Path.Combine(_currentCropDir, GenerateDateString() + ".bmp");
                                    if (checkBoxTemplateRec.Checked) ImageLogging.ForTemplateSaving(dir, _imgHistFilterClone.ToBitmap(), _rectTracker);
                                    
          
                                }

                                if (this.Visible) ThreadSafeSetPbBoxBgr(pictureBoxCam, _frameRT);

                            }
                            catch (Exception err)
                            {
                                ErrorMessageMain(err);
                            }
                        }
                        else if (rbHSD.Checked)
                            if (this.Visible)
                                ThreadSafeSetPbBoxGray(pictureBoxCam, _imgHistFilter);

                        double time_index = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec);
                        double framenumber = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);

                    }
                }

                


                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }

        }

        private void ThreadSafeSetPbBoxGray(PictureBox pb, Image<Gray, Byte> img)
        {
            if (pb.InvokeRequired)
            {
                pb.BeginInvoke((MethodInvoker)delegate
                {
                    pb.Image = img.ToBitmap();
                });
            }
            else pb.Image = img.ToBitmap();
        }

        private bool IsEven(int n)
        {
            if (n % 2 == 0) return true;
            else return false;
        }

        private void ThreadSafeSetPbBoxBgr(PictureBox pb, Image<Bgr, Byte> img)
        {
            if (pb.InvokeRequired)
            {
                pb.BeginInvoke((MethodInvoker)delegate
                {
                    pb.Image = img.ToBitmap();
                });
            }
            else pb.Image = img.ToBitmap();
        }

        private void btnStartCam_Click(object sender, EventArgs e)
        {
            #region camera_start

            _capture = null;
            _capture = new VideoCapture(0);

            if (_capture.QueryFrame() != null)
            {

                string dirOPN = "data\\Template Train\\Open\\main\\main.bmp";
                string dirLFT = "data\\Template Train\\Left\\main\\main.bmp";
                string dirRGT = "data\\Template Train\\Right\\main\\main.bmp";
                string dirHLT = "data\\Template Train\\Halt\\main\\main.bmp";

                //_templateMainOPN = new Image<Gray, Byte>(new Bitmap(dirOPN));
                //_templateMainLFT = new Image<Gray, Byte>(new Bitmap(dirLFT));
                //_templateMainRGT = new Image<Gray, Byte>(new Bitmap(dirRGT));
                //_templateMainHLT = new Image<Gray, Byte>(new Bitmap(dirHLT));


                btnStartCam.Enabled = false;
                btnStopCam.Enabled = true;
                int height = pictureBoxCam.Size.Height;
                int width = pictureBoxCam.Size.Width;
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 30);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, height);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, width);
                Application.Idle += ProcessFrame_Idle;
            }

            #endregion
        }

        private void btnStopCam_Click(object sender, EventArgs e)
        {
            #region camera_stop

            btnStartCam.Enabled = true;
            btnStopCam.Enabled = false;

            Application.Idle -= ProcessFrame_Idle;
            Memory.ReleaseData(ref _capture);

            pictureBoxCam.Image = null;
            #endregion

        }

        private void FormTemplateTrain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Memory.ReleaseData(ref _capture);
            this.Hide();
        }

        private void FormTemplateTrain_Load(object sender, EventArgs e)
        {
            LoadSkinTrees();
        }

        private void LoadSkinTrees()
        {

            _hsvSkinTree = new HSVTree(HIST_SKIN_FILE_HSV);
            _hsvNSkinTree = new HSVTree(HIST_NONSKIN_FILE_HSV);
        }

        private void UpdateHandDirsAndLabels() 
        {
            if (radioButtonOpen.Checked)
            {
                _currentCropDir = TMP_OPN_DIR;
                _classificationHand = CLASSIFICATION_HAND_OPEN;

            }
            else if (radioButtonLeft.Checked)
            {
                _currentCropDir = TMP_LFT_DIR;
                _classificationHand = CLASSIFICATION_HAND_LEFT;

            }
            else if (radioButtonRight.Checked)
            {
                _currentCropDir = TMP_RGT_DIR;
                _classificationHand = CLASSIFICATION_HAND_RIGHT;
            }
            else if (radioButtonHalt.Checked)
            {
                _currentCropDir = TMP_HLT_DIR;
                _classificationHand = CLASSIFICATION_HAND_HALT;
            }
            else if (radioButtonScrollDown.Checked)
            {
                _currentCropDir = TMP_SDN_DIR;
                _classificationHand = CLASSIFICATION_HAND_DOWN;
            }
            else if (radioButtonScrollUp.Checked)
            {
                _currentCropDir = TMP_SUP_DIR;
                _classificationHand = CLASSIFICATION_HAND_UP;
            } 
        }

        private void radioButtonLeft_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHandDirsAndLabels();
           
        }

        private void radioButtonRight_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHandDirsAndLabels();
        }

        private void radioButtonOpen_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHandDirsAndLabels();
        }

        private void trackBarFreq_Scroll(object sender, EventArgs e)
        {
            labelFreq.Text = trackBarFreq.Value.ToString();
        }

        private void trackBarErode_Scroll(object sender, EventArgs e)
        {
            labelErode.Text = trackBarErode.Value.ToString();
        }

        private void trackBarDilate_Scroll(object sender, EventArgs e)
        {
            labelDilate.Text = trackBarDilate.Value.ToString();
        }

        private void checkBoxTemplateRec_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonHalt_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHandDirsAndLabels();
        }

        private void radioButtonScrollDown_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHandDirsAndLabels();
        }

        private void radioButtonScrollUp_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHandDirsAndLabels();
        }

        private void buttonTrainClassifier_Click(object sender, EventArgs e)
        {
            //buttonTrainClassifier.Enabled = false;
            //btnStartCam.Enabled = false;
            //btnStopCam.Enabled = false;

            //_threadKNNTrainer = new Thread(new ThreadStart(delegate()
            //;

            //_knnInOut = new KNNInOut(PATH_FEATURES);

            //_knn = new KNearestNeighbors(4, _knnInOut.Inputs, _knnInOut.Outputs);


            //}
            //));

            //_threadKNNTrainer.Start();


            

            //buttonTrainClassifier.Enabled = true;
            //btnStartCam.Enabled = true;
            //btnStopCam.Enabled = true;

        }

        private void radioButtonHand_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHand.Checked) 
            {
                _classificationInit = CLASSIFICATION_HAND;
            } 
            if (radioButtonArm.Checked) _classificationInit = CLASSIFICATION_ARM;
            if (radioButtonReject.Checked) _classificationInit = CLASSIFICATION_HAND2;
        }

        private void radioButtonArm_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButtonHand.Checked) _classificationInit = CLASSIFICATION_HAND;
            if (radioButtonArm.Checked) _classificationInit = CLASSIFICATION_ARM;
            if (radioButtonReject.Checked) _classificationInit = CLASSIFICATION_HAND2;
        }

        private void radioButtonReject_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButtonHand.Checked) _classificationInit = CLASSIFICATION_HAND;
            if (radioButtonArm.Checked) _classificationInit = CLASSIFICATION_ARM;
            if (radioButtonReject.Checked) _classificationInit = CLASSIFICATION_HAND2;
        }

        private void trackBarHueMin_Scroll(object sender, EventArgs e)
        {
            //labelHueMin.Text = trackBarHueMin.Value.ToString();
        }

        private void trackBarHueMax_Scroll(object sender, EventArgs e)
        {
            //labelHueMax.Text = trackBarHueMax.Value.ToString();
        }

        private void trackBarSatMinScroll(object sender, EventArgs e)
        {
            //labelSatMin.Text = trackBarSatMin.Value.ToString();
        }

        
        
        private void trackBarSatMax_Scroll(object sender, EventArgs e)
        {
            //labelSatMax.Text = trackBarSatMax.Value.ToString();
        }

        private void trackBarValueMin_Scroll(object sender, EventArgs e)
        {
            //labelValueMin.Text = trackBarValueMin.Value.ToString();
        }

        private void trackBarValueMax_Scroll(object sender, EventArgs e)
        {
            //labelValueMax.Text = trackBarValueMax.Value.ToString();
        }

    }
}
        #endregion