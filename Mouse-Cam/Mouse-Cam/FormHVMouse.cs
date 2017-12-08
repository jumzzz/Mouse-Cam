#region DOT_NET_LIBS
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
using System.Speech;
using System.Speech.Synthesis;
#endregion

#region EMGUCV_LIBS
using Emgu.CV;
using Emgu.CV.Shape;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Cuda;
#endregion

#region AFORGE_LIBS

//using AForge;
//using AForge.Math.Geometry;
//using Accord.Imaging.Filters;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
//using Accord.Statistics.Kernels;


#endregion


namespace Hand_Virtual_Mouse
{
    public partial class FormHVMouse : Form
    {
        double stdState = 0.0;

        private string DIR_OPN = "";
        private string DIR_LFT = "";
        private string DIR_RGT = "";
        private string DIR_HLT = "";
        private string DIR_SUP = "";
        private string DIR_SDN = "";

        private Image<Gray, Byte> TMP_OPN;
        private Image<Gray, Byte> TMP_LFT;
        private Image<Gray, Byte> TMP_RGT;
        private Image<Gray, Byte> TMP_HLT;
        private Image<Gray, Byte> TMP_SUP;
        private Image<Gray, Byte> TMP_SDN;



        private Thread _notifThread;
        private bool _speaking = false;
        private bool _leftDown = false;
        private bool _rightDown = false;
        private Image<Gray, Byte> _templateCurrent;
        private int _handStateInt = 0;

        private int PREVIOUS_STATE = -1;
        private int CURRENT_STATE = -1;

        
        private double _currentProb = 0.0;
        private Image<Gray, Byte> _templateImg;
        private Rectangle _rectFinalTemplate = new Rectangle();
        private Point pt1Mouse = new Point();
        private Point pt0Mouse = new Point();

        
        int X_PREV = 0;
        int Y_PREV = 0;

        int X_NOW = 0;
        int Y_NOW = 0;
        
        private Image<Bgr, Byte> _frameDefect;
        private Image<Bgr, Byte> _frameTracker;

            private RotatedRect _rotRectTrackFinal;

            #region ML_OBJECTS

          private MulticlassSupportVectorMachine _svmMachineInit;
          private MulticlassSupportVectorLearning _svmTeacherInit;

          private MulticlassSupportVectorMachine _svmMachineHand;
          private MulticlassSupportVectorLearning _svmTeacherHand;

          private TemplateMatchClassifier _templateGesture;

          private const string PATH_FEATURES_DEFECT = "Feature Extraction\\DEFECT_FEATURES\\defect_features.csv";
        //private TemplateMatchDecisionTree _tempMatchDecisionTree;
        
        #endregion

            
            #region CLASSIFICATION

              private const int CLASSIFICATION_HAND = 0;
              private const int CLASSIFICATION_ARM = 1;
              private const int CLASSIFICATION_HAND2 = 2;

              private const int CLASSIFICATION_HAND_OPEN = 0;
              private const int CLASSIFICATION_HAND_LEFT = 1;
              private const int CLASSIFICATION_HAND_RIGHT = 2;
              private const int CLASSIFICATION_HAND_DOWN = 3;
              private const int CLASSIFICATION_HAND_UP = 4;
              private const int CLASSIFICATION_HAND_HALT = 5;

            #endregion

            #region INNER_ROI_SEARCH_FIELDS
            private VectorOfInt _hull = new VectorOfInt();
            private Mat _convexityDefect = new Mat();
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
           
            #region PRIVATE_FIELDS

            private bool _initView = true;


            private ROISearcher _roiSearcher;

            private const string CONFIG_FILESETTINGS_PATH = "Settings\\File_Settings.xml";
            private const string CONFIG_IMGPROCSETTINGS_PATH = "Settings\\Image_Proc_Settings.xml";

            private const string HIST_SKIN_FILE_HSV = "data\\histogram\\histskindatahsv.csv";
            private const string HIST_NONSKIN_FILE_HSV = "data\\histogram\\histnonskindatahsv.csv";

           

            private const string HAND_STATE_HALT =        "HALT";
            private const string HAND_STATE_OPEN =        "OPEN";
            private const string HAND_STATE_LEFT =        "LEFT";
            private const string HAND_STATE_SCROLL_DOWN = "SCROLL DOWN";
            private const string HAND_STATE_SCROLL_UP =   "SCROLL UP";
            private const string HAND_STATE_RIGHT =       "RIGHT";

           
            private ConfigFileVirtualMouse _configFile;
       
            private const double FONT_SCALE = 0.4;
            private const int    THICKNESS = 1;
            private const int    DECIMAL_PLACE = 2;

            private ConfigImgProcVirtualMouse _configImgProc;
            private double _maxProb = 0.0;
           
            private MultiTemplate _mtOPN;
            private MultiTemplate _mtLFT;
            private MultiTemplate _mtRGT;
            private MultiTemplate _mtHLT;
            private MultiTemplate _mtSDN;
            private MultiTemplate _mtSUP;
            private Image<Gray, Byte> _templateCurrTrack;
        
            private bool _isHalt = true;  
         
            private long _elapsedTime = 0L;
            //private VirtualMouse _virtualMouse = new VirtualMouse();
            private Bitmap _histCroppedBM;
            private Image<Gray, Byte> _histCropped;
            private double _ratioContourArea;

            private long _leftClickCounter = 0L;
            private long _rightClickCounter = 0L;

            private double _templateClickProbOPN = 0.0;
            private double _templateClickProbLFT = 0.0;
            private double _templateClickProbRGT = 0.0;
            private double _templateClickProbHLT = 0.0;
            private double _templateClickProbSDN = 0.0;
            private double _templateClickProbSUP = 0.0;

            private string _handState = "";
            private double MIN_TRESH_TMP_TRACK = 0.2;
            private double _trackingProbability = 0.0;
            
            private int _xPrev = 0;
            private int _yPrev = 0;
            private int _xNow = 0;
            private int _yNow = 0;

            int _vxNow = 0;
            int _vyNow = 0;

            int _axNow = 0;
            int _ayNow = 0;


            private int _mPrevX = 0;
            private int _mNowX = 0;
            private int _mPrevY = 0;
            private int _mNowY = 0;

        
            private LinkedList<int> _listOfMX = new LinkedList<int>();
            private LinkedList<int> _listOfMY = new LinkedList<int>();
            private LinkedList<int> _listOfDeltasX = new LinkedList<int>();
            private LinkedList<int> _listOfDeltasY = new LinkedList<int>();
            private LinkedList<int> _listOfStartDefectPtX = new LinkedList<int>();
            private LinkedList<int> _listOfStartDefectPtY = new LinkedList<int>();
            private LinkedList<int> _listOfLargestArea = new LinkedList<int>();
            private LinkedList<double> _listOfTempOPN = new LinkedList<double>();
            private LinkedList<double> _listOfTempLFT = new LinkedList<double>();
            private LinkedList<double> _listOfTempRGT = new LinkedList<double>();
            private LinkedList<int> _listOfRectHeight = new LinkedList<int>();
            private LinkedList<int> _listOfState = new LinkedList<int>();
            private LinkedList<int> _listOfRectWidth = new LinkedList<int>();


            private FormUpdateSkinColor _formUpdateSkinColor = new FormUpdateSkinColor();
            private FormTemplateTrain _formTemplateTrain = new FormTemplateTrain();
        
            
            private VideoCapture _capture = null;

            private Thread _threadHSDTrainer;

            private Image<Gray, Byte> _imgHistFilter;
            private Image<Gray, Byte> _imgHistFilterClone;
            private Image<Bgr, Byte> _frameRT;
            private Image<Bgr, Byte> _frameRTClone;
        

            private Image<Bgr, Byte> _frameImg;
            private double Framesno = 0;
     
            #endregion

            #region training_object

        
            private HSVStat _hsvStat;
            private HSVTree _hsvSkinTree;
            private HSVTree _hsvNSkinTree;
            
            #endregion

            #region PARAMETERS
            private MousePoint _mousePointCurr;         // monitors centroid of ROI if there's a change
            private bool _allowMouseMovement = false;
            private int _indexLargestContour = 0;
            private VectorOfVectorOfPoint _contour = new VectorOfVectorOfPoint();
            private VectorOfPoint _largestContour = new VectorOfPoint();
            private VectorOfPoint _convexHull = new VectorOfPoint();
            private CircleF _mousePointCirc;
            private CircleF _mousePointCircPrev;
            private float _pointerRadius = 50f;
            private System.Drawing.Point _roiCentroid = new Point(); // done
            private CircleF _circleMax = new CircleF();
            private Rectangle _rectTracker = new Rectangle();

            private Emgu.CV.Structure.MCvScalar _mcvBlue = new Emgu.CV.Structure.MCvScalar(255, 0, 0);
            private Emgu.CV.Structure.MCvScalar _mcvBlack = new Emgu.CV.Structure.MCvScalar(0, 0, 0);
            private Emgu.CV.Structure.MCvScalar _mcvRed = new Emgu.CV.Structure.MCvScalar(0, 0, 255);
            private Emgu.CV.Structure.MCvScalar _mcvGreen = new Emgu.CV.Structure.MCvScalar(0, 255, 0);
            private Emgu.CV.Structure.MCvScalar _mcvYellow = new Emgu.CV.Structure.MCvScalar(51.0, 255.0, 255.0);
            #endregion

            #region PREV_PARAMETERS
            private System.Drawing.Point _roiCentroidPrev = new Point(); // done
            #endregion
        
            #region MOUSE_PARAMETERS

            private System.Drawing.Point _mouseDir = new System.Drawing.Point(0,0);
        
            private const int MIN_VAL_INCREMENT = 15;

            #endregion

            #region CURSOR_PARAMETERS

            private System.Drawing.Point _s0 = new Point(0, 0);
            private System.Drawing.Point _s1 = new Point(0, 0);
            private System.Drawing.Point _s2 = new Point(0, 0);

            private int _v0X = 0;
            private int _v1X = 0;
            private int _v2X = 0;

            private int _v0Y = 0;
            private int _v1Y = 0;
            private int _v2Y = 0;

            private int _a0X = 0;
            private int _a1X = 0;
            private int _a2X = 0;

            private int _a0Y = 0;
            private int _a1Y = 0;
            private int _a2Y = 0;

        
            #endregion

            #region ML_INIT

            private void InitTemplateClicker() 
            {
                KnnIO knnIO = new KnnIO(PATH_FEATURES_DEFECT, 1.0);
                _templateGesture = new TemplateMatchClassifier(6, knnIO.Inputs, knnIO.Outputs);
        
            }

            private void InitSVMInit() 
            {
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

                //Console.WriteLine("Debugger");
            }

            private void InitSVMGesture() 
            {
        

            }
            #endregion

            #region CONSTRUCTOR
        public FormHVMouse()
        {
            InitializeComponent();

            InitTemplateClicker();
            InitSVMInit();                 // initialize svm
            InitSVMHand();

            
            _frameRT = new Image<Bgr, Byte>(pictureBoxCam.Size);
            _frameDefect = new Image<Bgr, Byte>(pictureBoxCam.Size);
            _frameTracker = new Image<Bgr, Byte>(pictureBoxCam.Size);

            _configImgProc = ConfigImgProcVirtualMouse.DeserializeObject(CONFIG_IMGPROCSETTINGS_PATH);
            _configFile = ConfigFileVirtualMouse.DeserializeObject(CONFIG_FILESETTINGS_PATH);
            _frameRT = new Image<Bgr, Byte>(pictureBoxCam.Size);   
            _mousePointCurr = new MousePoint(new System.Drawing.Point(0, 0));

            _roiSearcher = new ROISearcher();

            string fileOPN = Directory.GetFiles(_configFile.FilePathOPN)[0];
            string fileLFT = Directory.GetFiles(_configFile.FilePathLFT)[0];
            string fileRGT = Directory.GetFiles(_configFile.FilePathRGT)[0];
            string fileHLT = Directory.GetFiles(_configFile.FilePathHLT)[0];
            string fileSUP = Directory.GetFiles(_configFile.FilePathSUP)[0];
            string fileSDN = Directory.GetFiles(_configFile.FilePathSDN)[0];


            Bitmap bmOPN = new Bitmap(fileOPN);
            Bitmap bmLFT = new Bitmap(fileLFT);
            Bitmap bmRGT = new Bitmap(fileRGT);
            Bitmap bmHLT = new Bitmap(fileHLT);
            Bitmap bmSUP = new Bitmap(fileSUP);
            Bitmap bmSDN = new Bitmap(fileSDN);


            
            _hsvLookUpTable = new HSVLookUpTable(_configFile.FileHistSkinHSV);


            for (int i = 0; i < 3; i++) 
            {
                _listOfDeltasX.AddLast(0);
                _listOfDeltasY.AddLast(0);
            }

            for (int i = 0; i < 3; i++)
                _listOfState.AddLast(0);

            for (int i = 0; i < 3; i++) 
            {
                _listOfRectHeight.AddLast(0);
                _listOfRectWidth.AddLast(0);
            }
            
        }

        #endregion

            #region THREAD_METHOD
        private void ThreadSafeEnableBtn(Button bt, bool enabled)
        {
            if (bt.InvokeRequired)
            {
                bt.BeginInvoke((MethodInvoker)delegate
                {
                    bt.Enabled = enabled;
                });
            }
            else bt.Enabled = enabled;
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

        #endregion
     
        #region MISC_METHODS

        private void LogParameters()
        {
            List<string> recordData = new List<string>();
            
            recordData.Add(_rectTracker.Location.X.ToString());   
            recordData.Add(_rectTracker.Location.Y.ToString());   
            recordData.Add((_rectTracker.Location.X + _rectTracker.Width).ToString());
            recordData.Add(_rectTracker.Location.Y.ToString());   
            recordData.Add(_rectTracker.Location.X.ToString());   
            recordData.Add((_rectTracker.Location.Y + _rectTracker.Height).ToString());  
            recordData.Add((_rectTracker.Location.X + _rectTracker.Width).ToString());   
            recordData.Add((_rectTracker.Location.Y + _rectTracker.Height).ToString());  
            recordData.Add(_handState);
            recordData.Add(_elapsedTime.ToString());
            recordData.Add(Math.Round(_templateClickProbOPN, DECIMAL_PLACE).ToString());
            recordData.Add(Math.Round(_templateClickProbLFT, DECIMAL_PLACE).ToString());
            recordData.Add(Math.Round(_templateClickProbRGT, DECIMAL_PLACE).ToString());
            recordData.Add(Math.Round(_templateClickProbHLT, DECIMAL_PLACE).ToString());
            recordData.Add(Math.Round(_templateClickProbSDN, DECIMAL_PLACE).ToString());
            recordData.Add(Math.Round(_templateClickProbSUP, DECIMAL_PLACE).ToString());
            recordData.Add(Math.Round(_trackingProbability,  DECIMAL_PLACE).ToString());
            
            StringBuilder lineData = new StringBuilder();
            recordData.ForEach(s => lineData.Append(s + ","));


            List<string> fileLines = File.ReadAllLines(_configFile.FilePathPRM).ToList();
            fileLines.Add(lineData.ToString().Substring(0, lineData.Length - 1));
            File.WriteAllLines(_configFile.FilePathPRM, fileLines.ToArray());


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

                            try
                            {
                                LocateLargestContourTempMatch();

                                if (_indexLargestContour > 0)
                                {
                                    ProcessROIAndTemplate();

                                    ThreadSafeSetPbBoxBgr(pictureBoxCam, _frameRT);
                                    ThreadSafeSetPbBoxGray(pictureBoxHSD, _imgHistFilterClone);
                                    ThreadSafeSetPbBoxBgr(pictureBoxDefect, _frameDefect);

                                }

                                if (this.Visible) ThreadSafeSetPbBoxBgr(pictureBoxCam, _frameRT);

                            }
                            catch (Exception err)
                            {
                                ErrorMessageMain(err);
                            }
                        
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

        private double[] ScaleValues(double[] list, double scalingFactor)
        {
            List<double> values = new List<double>();
            list.ToList().ForEach(d => values.Add(scalingFactor * d));

            return values.ToArray();
        }

        private void ErrorMessageMain(Exception err)
        {
            String message = String.Concat("Message: \n", err.Message);
            String stacktrace = String.Concat("Stack Trace: \n", err.StackTrace);
            String indexContourStr = "Index of Largest Contour: " + _indexLargestContour.ToString();

            String countContourStr = "Count of Contour: " + _contour.Size.ToString();


            MessageBox.Show(String.Concat(message, "\n", stacktrace, "\n"), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ProcessROIAndTemplate()
        {
            _largestContour = _contour[_indexLargestContour];
            _distanceList = new List<double>();


            CvInvoke.ConvexHull(_largestContour, _hull, true, false);
            CvInvoke.ConvexityDefects(_largestContour, _hull, _convexityDefect);

            double totalArea = (double)_frameRT.Width * (double)_frameRT.Height;
            double areaCountor = CvInvoke.ContourArea(_largestContour);

            bool areaRatioValid = (areaCountor / totalArea) > 0.01;

            if (!_convexityDefect.IsEmpty && areaRatioValid)
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
                    _distanceList.Add(Geometry.Distance(_largestContour[_hull[i - 1]], _largestContour[_hull[i]]));
                }


                //CvInvoke.Line(_frameRT, _convexDefectList[0].StartPt, _convexDefectList[0].EndPt, _mcvRed);
                //CvInvoke.Line(_frameRT, _convexDefectList[1].StartPt, _convexDefectList[1].EndPt, _mcvRed);

                if (_convexDefectList.Count > 3)
                {
                    //CvInvoke.Circle(_frameRT, _convexDefectList[0].DefectPt, 10, _mcvYellow, 2);
                    //CvInvoke.Circle(_frameRT, _convexDefectList[1].DefectPt, 10, _mcvYellow, 2);

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


                        //CvInvoke.PutText(_frameRT, state, _rectTracker.Location, FontFace.HersheyComplex, 0.6, _mcvBlue, 1);
                        //CvInvoke.Rectangle(_frameRT, _rectTracker, _mcvBlack, 2);


                        double[] featureVectorHand = ScaleValues(_huMoments.ToList()
                        .GetRange(0, _svmMachineHand.Inputs).ToArray(), 1000.0);

                        _frameDefect = new Image<Bgr, Byte>(_frameRT.Size);
                        _frameTracker = new Image<Bgr, Byte>(_frameRT.Size);

                        //List<Point> polyList = new List<Point>();
                        convexCorrected1 = Statistics.FilterDefects(_convexDefectList, 20.0, 130.0, _rectTracker, 0.6);



                        foreach (ConvexDefects cd in convexCorrected1)
                        {
                            Point[] ptDefect = new Point[] { cd.StartPt, cd.EndPt, cd.DefectPt };

                            VectorOfPoint vDefect = new VectorOfPoint(ptDefect);
                            VectorOfPoint vDefectPoly = new VectorOfPoint();
                            CvInvoke.ApproxPolyDP(vDefect, vDefectPoly, 0.0001, true);

                            if(checkBoxShowDefects.Checked)
                              CvInvoke.FillConvexPoly(_frameRT, vDefectPoly, _mcvGreen);
                            
                            CvInvoke.FillConvexPoly(_frameDefect, vDefectPoly, _mcvGreen);
                        }


                        double shortestDist = _rotRectMod.ShortestDist();
                        double[] featuresDefect = Statistics.DefectFeatureVector(convexCorrected1, shortestDist);

                        Point labelPointHand = new Point((9) * _frameRT.Width / 10, 20);

                        int handPrediction = _templateGesture.TemplateMatch(featuresDefect);

                        Rectangle rectFinal = Geometry.TemplateMatchingDef(convexCorrected1, _frameRT);

                        int correctedDim = rectFinal.Width;
                    
                        rectFinal = new Rectangle(rectFinal.Location, new Size(correctedDim, correctedDim));

                        bool validityX1 = rectFinal.Location.X > 0;
                        bool validityY1 = rectFinal.Location.Y > 0;

                        bool validityX2 = rectFinal.Location.X + correctedDim < _frameDefect.Width - 1;
                        bool validityY2 = rectFinal.Location.Y + correctedDim < _frameDefect.Height - 1;

                        bool rectFinalValid = validityX1 && validityX2 && validityY1 && validityY2;

                        if (rectFinalValid)
                        {
                            
                            _rectFinalTemplate = rectFinal;
                            CvInvoke.Rectangle(_frameRT, _rectFinalTemplate, _mcvRed, 2);

                            
                            TemplateMouseAction(handPrediction, labelPointHand);
                            
                            List<double> featuresToBeLogged = new List<double>();
                            featuresToBeLogged.AddRange(_huMoments.ToList());
                            

                        }


                    }
                    else if (prediction == CLASSIFICATION_ARM)
                    {
                        int yDel = 0;
                        state = "ARM".ToUpper() + " Angle: " + Math.Round(_rectNew.Angle, 3).ToString();

                        _ptLftToRight = Geometry.Distance(_rotRectMod.Pul, _rotRectMod.Pur);
                        _ptUpToDown = Geometry.Distance(_rotRectMod.Pul, _rotRectMod.Pll);


                        if (_ptLftToRight <= _ptUpToDown)
                        {
                            _rotRectEval1 = Geometry.CutArmToHand(_rotRectMod);

                            Size sizeFrame = _frameRT.Size;
                            _rectROIEval1 = _rotRectEval1.ToRect(sizeFrame);
                            _rectROIEval2 = _rotRectEval2.ToRect(sizeFrame);

                            Mat cloneMat1 = _imgHistFilterClone.Clone().Mat;

                            _matToBeEval1 = new Mat(cloneMat1, _rectROIEval1);
                            _contoursEval1 = new VectorOfVectorOfPoint();

                            CvInvoke.FindContours(_matToBeEval1, _contoursEval1, _matHierachyEval1, Emgu.CV.CvEnum.RetrType.Tree,
                                Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

                            _largestContourIndexEval1 = ImgProc.LargestContourIndex(_contoursEval1);

                            _momentEval1 = CvInvoke.Moments(_contoursEval1[_largestContourIndexEval1]);

                            _huMomentsEval1 = _momentEval1.GetHuMoment().ToList();

                            double[] featureVectorSearch = ScaleValues(_huMomentsEval1.ToArray(), 5000.0);
                            _predictionEval1 = _svmMachineInit.Compute(featureVectorSearch, MulticlassComputeMethod.Elimination);

                            double[] featureVectorHand = ScaleValues(_huMomentsEval1.ToList()
                                .GetRange(0, _svmMachineHand.Inputs).ToArray(), 1000.0);

                            if (_predictionEval1 == CLASSIFICATION_HAND || _predictionEval1 == CLASSIFICATION_HAND2)
                            {
                                _rectWithin = CvInvoke.MinAreaRect(_contoursEval1[_largestContourIndexEval1]).MinAreaRect();
                                _actualLoc = new Point(_rectROIEval1.Location.X + _rectWithin.Location.X,
                                                            _rectROIEval1.Location.Y + _rectWithin.Location.Y);

                                _actualROI = new Rectangle(_actualLoc, _rectWithin.Size);

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

                                    _frameDefect = new Image<Bgr, Byte>(_frameRT.Size);
                                    _frameTracker = new Image<Bgr, Byte>(_frameRT.Size);

                                    //List<Point> polyList = new List<Point>();

                                    foreach (ConvexDefects cd in convexCorrectedInside)
                                    {
                                        Point[] ptDefect = new Point[] { cd.StartPt, cd.EndPt, cd.DefectPt };

                                        //polyList.AddRange(ptDefect.ToList());

                                        VectorOfPoint vDefect = new VectorOfPoint(ptDefect);
                                        VectorOfPoint vDefectPoly = new VectorOfPoint();
                                        CvInvoke.ApproxPolyDP(vDefect, vDefectPoly, 0.0001, true);

                                        if (checkBoxShowDefects.Checked)
                                        {
                                            CvInvoke.FillConvexPoly(_frameRT, vDefectPoly, _mcvGreen);    
                                        } 

                                        CvInvoke.FillConvexPoly(_frameDefect, vDefectPoly, _mcvGreen);
                                    }


                                    double shortestDist = _rotRectEval1.ShortestDist();
                                    double[] featuresDefectInside = Statistics.DefectFeatureVector(convexCorrectedInside, shortestDist);

                                    int handPrediction = _templateGesture.TemplateMatch(featuresDefectInside);

                                    Point labelPointHand = new Point((9) * _frameRT.Width / 10, 20);


                                    Rectangle rectFinal = Geometry.TemplateMatchingDef(convexCorrectedInside, _frameRT);

                                    int correctedDim = rectFinal.Width;

                                    rectFinal = new Rectangle(rectFinal.Location, new Size(correctedDim, correctedDim));

                                    bool validityX1 = rectFinal.Location.X > 0;
                                    bool validityY1 = rectFinal.Location.Y > 0;

                                    bool validityX2 = rectFinal.Location.X + correctedDim < _frameDefect.Width - 1;
                                    bool validityY2 = rectFinal.Location.Y + correctedDim < _frameDefect.Height - 1;

                                    bool rectFinalValid = validityX1 && validityX2 && validityY1 && validityY2;

                                    if (rectFinalValid)
                                    {
                                        _rectFinalTemplate = rectFinal;
                                        CvInvoke.Rectangle(_frameRT, _rectFinalTemplate, _mcvRed, 2);

                                        _roiCentroid = Geometry.RectangularCentroid(_rectFinalTemplate);

                                        TemplateMouseAction(handPrediction, labelPointHand);


                                        List<double> featuresToBeLogged = new List<double>();

                                        featuresToBeLogged.AddRange(_huMomentsEval1);

                                    }

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

                    else
                        state = "none".ToUpper();
                }

            }
            else 
            {
                
                _notifThread = new Thread(new ThreadStart(delegate()
                {
                    _speaking = true;
                    SpeechSynthesizer synth = new SpeechSynthesizer();
                    synth.SetOutputToDefaultAudioDevice();
                    
                    synth.Speak("No hand Detected. Please put your right hand in front of camera.");
                    _speaking = false;

                }
               ));

                if (!_speaking) _notifThread.Start();
            }

        }

        private void LocateLargestContourTempMatch()
        {
            _contour = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(_imgHistFilter, _contour, hierarchy, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            _indexLargestContour = ImgProc.LargestContourIndex(_contour);

        }


        private void TemplateMouseControl(bool enableMove)
        {

            _histCroppedBM = ImgProc.CropImage(_imgHistFilterClone.ToBitmap(), _rectTracker);
            _histCropped = new Image<Emgu.CV.Structure.Gray, byte>(_histCroppedBM);
            _ratioContourArea = ImgProc.ContourROIRatio(_histCropped);

            CvInvoke.PutText(_frameRT, Math.Round(_ratioContourArea, DECIMAL_PLACE).ToString(), 
                new Point(20, 20), FontFace.HersheyDuplex, FONT_SCALE, _mcvBlack, 1);

            Image<Gray, Byte> source = new Image<Gray, byte>(ImgProc.CropImage(_imgHistFilterClone.ToBitmap(), _rectTracker));

            

            _mtHLT.TemplateMatchOnROI(source);
            _mtOPN.TemplateMatchOnROI(source);
            _mtLFT.TemplateMatchOnROI(source);
            _mtRGT.TemplateMatchOnROI(source);
            _mtSDN.TemplateMatchOnROI(source);
            _mtSUP.TemplateMatchOnROI(source);
            
            
            _templateClickProbOPN = _mtOPN.MaxProb;
            _templateClickProbLFT = _mtLFT.MaxProb;
            _templateClickProbRGT = _mtRGT.MaxProb;
            _templateClickProbHLT = _mtHLT.MaxProb;
            _templateClickProbSDN = _mtSDN.MaxProb;
            _templateClickProbSUP = _mtSUP.MaxProb;


            List<double> listOfTemplateProb = new List<double>();
            listOfTemplateProb.Add(_templateClickProbHLT);
            listOfTemplateProb.Add(_templateClickProbOPN);
            listOfTemplateProb.Add(_templateClickProbLFT);
            listOfTemplateProb.Add(_templateClickProbRGT);
            listOfTemplateProb.Add(_templateClickProbSDN);
            listOfTemplateProb.Add(_templateClickProbSUP);

            _maxProb = listOfTemplateProb.Max();
           
            if(_maxProb.Equals(_templateClickProbHLT))
            {
                //_templateCurrTrack = _mtHLT.MaxTemplate;
               _templateCurrTrack = ImgProc.CropImage(_imgHistFilterClone, _rectTracker);
              
                
                _handState = HAND_STATE_HALT;
               
                _rightClickCounter = 0L;
                
                _leftClickCounter = 0L;
                
                _roiCentroidPrev = _roiCentroid;
                UpdateCoordinatesForMouse();
                _isHalt = true;
                
            }
            else if (_maxProb.Equals(_templateClickProbOPN))
            {
                _templateCurrTrack = ImgProc.CropImage(_imgHistFilterClone, _rectTracker);
                //_templateCurrTrack = _mtOPN.MaxTemplate;
                //_handState = HAND_STATE_OPEN;

                // triggers mouse move 

                if (enableMove && checkBoxMouseMove.Checked) 
                {
                    UpdateCoordinatesForMouse();

                    if (_isHalt) _isHalt = false;
                    else MoveMouse();
                    _rightClickCounter = 0L;
                    
                    _leftClickCounter = 0L;
                    
                }

                // resets right click and left click counter
                _rightClickCounter = 0L;
                
                _leftClickCounter = 0L;
                
            }
            else if (_maxProb.Equals(_templateClickProbLFT))
            {
                  _templateCurrTrack = ImgProc.CropImage(_imgHistFilterClone, _rectTracker);
              
                //_templateCurrTrack = _mtLFT.MaxTemplate;
                _handState = HAND_STATE_LEFT;
                
                // limits leftClick counter into 2 so that it can avoid spamming left clicks
                if (checkBoxLeftClick.Checked && _leftClickCounter < 2L) 
                {
                    VirtualMouse.LeftClick();
                    _leftClickCounter++;
                }
                
            }
            else if (_maxProb.Equals(_templateClickProbSDN)) 
            {
               _templateCurrTrack = ImgProc.CropImage(_imgHistFilterClone, _rectTracker);
              
                //_templateCurrTrack = _mtSDN.MaxTemplate;
                _handState = HAND_STATE_SCROLL_DOWN;

                if (checkBoxScroll.Checked && !this.Focused) 
                {
                    //_virtualMouse.ScrollDown();
                    //_virtualMouse.ScrollDown();

                    //VirtualMouse.ScrollDown();
                    VirtualMouse.ScrollDown();
                }
                
                

            }
            else if (_maxProb.Equals(_templateClickProbSUP))
            {
               _templateCurrTrack = ImgProc.CropImage(_imgHistFilterClone, _rectTracker);
              
                //_templateCurrTrack = _mtSUP.MaxTemplate;
                _handState = HAND_STATE_SCROLL_UP;

                if (checkBoxScroll.Checked && !this.Focused)
                {
                    VirtualMouse.ScrollUp();
                }

            }
            else
            {
                 _templateCurrTrack = ImgProc.CropImage(_imgHistFilterClone, _rectTracker);
                 _handState = HAND_STATE_RIGHT;

                

                // triggers right click
                // limits rightClick counter into 2 so that it can avoid spamming right clicks
                if (checkBoxRightClick.Checked && _rightClickCounter == 0L) 
                {
                    VirtualMouse.RightClick();
                    _rightClickCounter++;
                }
                 
            }

        }

        private void UpdateCoordinatesForMouse() 
        {
            _xPrev = _roiCentroid.X - _roiCentroidPrev.X;
            _yPrev = _roiCentroid.Y - _roiCentroidPrev.Y;

            _vxNow = _s1.X - _s0.X;
            _vyNow = _s1.Y - _s0.Y;

            _axNow = _v1X - _v0X;
            _ayNow = _v1Y - _v0Y;

            int t = 4;
            int t2 = 2*t*t;
            _xNow = _xPrev + _vxNow * (1 / t) + _axNow * (1 / t2);
            _yNow = _yPrev + _vyNow * (1 / t) + _ayNow * (1 / t2);
            //_xNow = _xPrev;
            //_yNow = _yPrev;


            //max1 = max0 + (x - max0) / dividerA;
            //may1 = may0 + (y - may0) / dividerA;
            //maz1 = maz0 + (z - maz0) / dividerA;

            _mNowX = _xNow;
            _mNowY = _yNow;


            //_mNowX = _mPrevX + (_xNow - _mPrevX) / 2;
            //_mNowY = _mPrevY + (_yNow - _mPrevY) / 2;

            bool isHalt = "HALT".Equals(_handState);
            bool isOpen = "OPEN".Equals(_handState);

            if (isHalt)
            {
                _mNowX = 0;
                _mNowY = 0;
                _mPrevX = 0;
                _mPrevY = 0;

            }

            UpdateLL(ref _listOfDeltasX, _mNowX);
            UpdateLL(ref _listOfDeltasY, _mNowY);
            UpdateLL(ref _listOfRectHeight, _rectFinalTemplate.Height);
            UpdateLL(ref _listOfRectWidth, _rectFinalTemplate.Width);
            
           

            double stdXDel = Math.Round(StandardDeviation(_listOfDeltasX), 5);
            double stdYDel = Math.Round(StandardDeviation(_listOfDeltasY), 5);
            stdState = Math.Round(StandardDeviation(_listOfState), 5);
            double stdWidth = StandardDeviation(_listOfRectWidth);
            double stdHeight = StandardDeviation(_listOfRectHeight);

            if (stdWidth > 20.0) _mNowX = 0;
            if (stdHeight > 20.0) _mNowY = 0;

            string stdXDelStr = stdXDel.ToString();
            string stdYDelStr = stdYDel.ToString();


            if (stdXDel <= 0.8) _mNowX = 0;
            if (stdYDel <= 0.8) _mNowY = 0;

            if (stdState > 0)
            {
                _mNowX = 0;
                _mNowY = 0;
            }



            _s0 = _s1;
            _s1 = _s2;
            _s2 = new Point(_mNowX, _mNowY);

            
            Point labelPtStdX = new Point(9 * _frameRT.Width / 10, 1 * _frameRT.Height / 10);
            Point labelPtStdY = new Point(9 * _frameRT.Width / 10, labelPtStdX.Y + 20);

            _v0X = _v1X;
            _v1X = _v2X;
            _v2X = _vxNow;

            _v0Y = _v1Y;
            _v1Y = _v2Y;
            _v2Y = _vyNow;

            _a0X = _a1X;
            _a1X = _a2X;
            _a2X = _axNow;

            _a0Y = _a1Y;
            _a1Y = _a2Y;
            _a2Y = _ayNow;

            _mPrevX = _mNowX;
            _mPrevY = _mNowY;

        }
      
        private void MoveMouse()
        {
            if (checkBoxMouseMove.Checked)
            {

                _mousePointCircPrev = _mousePointCirc;
                _mousePointCirc = new CircleF(new PointF((float)_roiCentroid.X, (float)_roiCentroid.Y), _pointerRadius);

                if (CircleIntersects(_mousePointCirc, _mousePointCircPrev))
                {
                    _mouseDir = _s2;
                    MoveCursor(_mouseDir, _frameRT);
                }

            }
        }
        
        private void ApproximateRectSize()
        {
            _largestContour = _contour[_indexLargestContour];
            _convexHull = new VectorOfPoint();

            CvInvoke.ConvexHull(_largestContour, _convexHull, false);
            _circleMax = CvInvoke.MinEnclosingCircle(_contour[_indexLargestContour]);

            System.Drawing.Point rectTrackStart = new Point((int)_circleMax.Center.X - (int)_circleMax.Radius - (int)(0.1*_circleMax.Radius),
                (int)_circleMax.Center.Y - (int)_circleMax.Radius - (int)(0.1*_circleMax.Radius));

           
            _rectTracker = new Rectangle(rectTrackStart, new Size(2 *
                ((int)_circleMax.Radius + (int)(0.1 * _circleMax.Radius)), 2 * (int)_circleMax.Radius + (int)(0.1 * _circleMax.Radius)));
        }

        private void LocateLargestContour()
        {
            _contour = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            Image<Gray, Byte> imgFilter = _imgHistFilter.Clone();
            CvInvoke.FindContours(imgFilter, _contour, hierarchy, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            _indexLargestContour = ImgProc.LargestContourIndex(_contour);
            
        }

        private void ApplySkinHistFilter()
        {
            _frameRTClone = _frameRT.Clone();
            _imgHistFilter = ImgProc.SkinDetectHSV(_hsvLookUpTable ,_frameRT,_configImgProc.Frequency,
                _configImgProc.Erosion,
                _configImgProc.Dilation);

            _imgHistFilterClone = _imgHistFilter.Clone();
            
        }


        /// <summary>
        /// Initialize image through acquiring raw image from camera and flipping.
        /// </summary>
        private void ImageInit()
        {
            InitHandParam();

            Framesno = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);

            Image<Bgr, Byte> initImg = _capture.QueryFrame().ToImage<Bgr, Byte>();
            Image<Bgr, Byte> filteredImg = new Image<Bgr, Byte>(initImg.Size);
            _frameRT = new Image<Bgr, Byte>(initImg.Size);
            _frameImg = new Image<Bgr, Byte>(initImg.Size);
                      
            filteredImg =initImg;

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


        private void MoveCursor(System.Drawing.Point offset, Image<Bgr, Byte> image)
        {
            Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            int widthMain = screenSize.Width;
            int heightMain = screenSize.Height;

            int widthImg = image.Width;
            int heightImg = image.Width;

            int finalOffsetX = 3 * offset.X;
            int finalOffsetY = 3 * offset.Y;

            int offsetMainX = widthMain * finalOffsetX / widthImg;
            int offsetMainY = heightMain * finalOffsetY / heightImg;

            this.Cursor = new Cursor(Cursor.Current.Handle);
            int posX = Cursor.Position.X + offsetMainX;
            int posY = Cursor.Position.Y + offsetMainY;

            if (posX < 0) posX = 1;
            if (posX > screenSize.Width) posX = screenSize.Width - 1;
            if (posY < 0) posY = 1;
            if (posY > screenSize.Height) posY = screenSize.Height - 1;

            Cursor.Position = new Point(posX, posY);

        }

        private bool CircleIntersects(CircleF c1, CircleF c2) 
        {
            double val0 = (c1.Radius - c2.Radius) * (c1.Radius - c2.Radius);
            double val1 = (c1.Center.X - c2.Center.X) * (c1.Center.X - c2.Center.X);
            double val2 = (c1.Center.Y - c2.Center.Y) * (c1.Center.Y - c2.Center.Y);
            double val3 =  (c1.Radius + c2.Radius) * (c1.Radius + c2.Radius);

            bool validity0 = val0 <= (val1 + val2);
            bool validity1 = (val1 + val2) <= val3;

            return validity0 && validity1;
           
        }

        private void InitHandParam()
        {
            _roiCentroid = new Point();
        }

        private void LoadSkinTrees()
        {
            _hsvSkinTree = new HSVTree(_configFile.FileHistSkinHSV);
            _hsvNSkinTree = new HSVTree(_configFile.FileHistNonSkinHSV);
        }

        #endregion

        #region EVENTS

        private void ProcessFrame_Idle(object sender, EventArgs arg)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            VideoRun();
            watch.Stop();
            _elapsedTime = watch.ElapsedMilliseconds;
            long fps = 1000L / _elapsedTime;

            labelFPS.Text = fps.ToString();
        }

        private void btnStartCam_Click(object sender, EventArgs e)
        {
            #region camera_start

            


            _mtOPN = new MultiTemplate(_configFile.FilePathOPN);
            _mtLFT = new MultiTemplate(_configFile.FilePathLFT);
            _mtRGT = new MultiTemplate(_configFile.FilePathRGT);
            _mtHLT = new MultiTemplate(_configFile.FilePathHLT);
            _mtSDN = new MultiTemplate(_configFile.FilePathSDN);
            _mtSUP = new MultiTemplate(_configFile.FilePathSUP);
   
            _capture = null;
            _capture = new VideoCapture(0);

            if (_capture.QueryFrame() != null)
            {
                btnStartCam.Enabled = false;
                btnStopCam.Enabled = true;
                int height = pictureBoxCam.Size.Height;
                int width = pictureBoxCam.Size.Width;
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 60);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, height);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, width);
                Application.Idle += ProcessFrame_Idle;
                _mousePointCurr.Changed += mousePoint_Changed;
                lblRemarks.Text = "Image \nProcessing";
            }
            else
            {
                lblRemarks.Text = "Please Attach Camera";
            }


            #endregion
        }

        private void btnStopCam_Click(object sender, EventArgs e)
        {
            #region camera_stop
            btnStartCam.Enabled = true;
            btnStopCam.Enabled = false;

            Application.Idle -= ProcessFrame_Idle;
            _mousePointCurr.Changed -= mousePoint_Changed;
            //   _threadVideo.Abort();
            //    ReleaseData();
            Memory.ReleaseData(ref _capture);

            pictureBoxCam.Image = null;
            pictureBoxHSD.Image = null;
            pictureBoxDefect.Image = null;
            
            lblRemarks.Text = "Press Start";
            #endregion
        }

        
        private void btnTrainHSD_Click(object sender, EventArgs e)
        {
            btnTrainHSD.Enabled = false;

            _threadHSDTrainer = new Thread(new ThreadStart(delegate()
            {
                _hsvStat = new HSVStat(_configFile.FilePathHSV);

                _hsvStat.SaveToFile(_configFile.FileHistSkinHSV,
                    _configFile.FileHistNonSkinHSV);
                LoadSkinTrees();

                ThreadSafeEnableBtn(btnTrainHSD, true);

            }
            ));

            _threadHSDTrainer.Start();
        }


        

        private void rbNormal_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxCam.Image = null;
        }

        private void rbHSD_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxCam.Image = null;
        }

        private void updateColorSkinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_formUpdateSkinColor.Visible)
            {
                _formUpdateSkinColor.Show();
            }
        }

        private void mousePoint_Changed(object sender, EventArgs e)
        {
            MoveCursor();
        }

        private void MoveCursor()
        {
            if (_allowMouseMovement)
            {
                this.Cursor = new Cursor(Cursor.Current.Handle);

                Size screenSize = Screen.PrimaryScreen.Bounds.Size;

                int xMouse = screenSize.Width * (_mousePointCurr.MouseLoc.X) / pictureBoxCam.Size.Width;
                int yMouse = screenSize.Height * (_mousePointCurr.MouseLoc.Y) / pictureBoxCam.Size.Height;

                Point pt = new Point(xMouse, yMouse);
                Cursor.Position = pt;
            }
        }

        
    
        private void FormHVMouse_Load(object sender, EventArgs e)
        {
            LoadSkinTrees();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormHVMouse_FormClosing(object sender, FormClosingEventArgs e)
        {
            Memory.ReleaseData(ref _capture);
        }

        private void pbMouseTest_Click(object sender, EventArgs e)
        {

        }

        private void pbMouseTest_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left) 
            //{
            //    lblClickRemark.Text = "Left";
            //}
            //else if (e.Button == MouseButtons.Right) 
            //{
            //    lblClickRemark.Text = "Right";
            //}
        }

        private void checkBoxMouseMove_CheckedChanged(object sender, EventArgs e)
        {
          

        }

        private void trackBarRad_Scroll(object sender, EventArgs e)
        {
        }

        private void pbMouseTest_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left && !lblClickRemark.Text.Equals("Left"))
            //{

            //}
            //else if (e.Button == MouseButtons.Right && !lblClickRemark.Text.Equals("Right"))
            //{
            //}
        }

        private void pbMouseTest_MouseUp(object sender, MouseEventArgs e)
        {
            //lblClickRemark.Text = "None";
        }

        private void trackBarLF_Scroll(object sender, EventArgs e)
        {
        }

        private void labelHF_Click(object sender, EventArgs e)
        {

        }

        private void trackBarHF_Scroll(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void labelAllowClicks_Click(object sender, EventArgs e)
        {

        }

        
        private void UpdateLL(ref LinkedList<double> ll, double val)
        {
            ll.RemoveFirst();
            ll.AddLast(val);
        }

        private void UpdateLL(ref LinkedList<int> ll, int val) 
        {
            ll.RemoveFirst();
            ll.AddLast(val);
        }

        private void UpdateMX(int val)
        {
            _listOfMX.RemoveFirst();
            _listOfMX.AddLast(val);
        }

        private void UpdateMY(int val)
        {
            _listOfMY.RemoveFirst();
            _listOfMY.AddLast(val);
        }

        private void UpdateDeltasX(int val) 
        {
            _listOfDeltasX.RemoveFirst();
            _listOfDeltasX.AddLast(val);
        }

        private void UpdateDeltasY(int val)
        {
            _listOfDeltasY.RemoveFirst();
            _listOfDeltasY.AddLast(val);
        }

        private void UpdateDefectPtX(int val)
        {
            _listOfStartDefectPtX.RemoveFirst();
            _listOfStartDefectPtX.AddLast(val);
        }

        private void UpdateDefectPtY(int val)
        {
            _listOfStartDefectPtY.RemoveFirst();
            _listOfStartDefectPtY.AddLast(val);
        }


        
        private double StandardDeviation(LinkedList<double> list)
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

            return result;
        }

        private double StandardDeviation(LinkedList<int> list) 
        {
            double result = 0.0;

            double sumOfAll = 0.0;
            double n = list.Count;

            foreach (int val in list) sumOfAll += val;

            double mu = sumOfAll / n;

            double sumOfDiffs = 0.0;

            foreach (int val in list) sumOfDiffs = (val - mu) * (val - mu);

            double sqrd = sumOfDiffs / (n - 1);

            result = Math.Sqrt(sqrd);

            return result;
        }


        private void pictureBoxCam_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
                labelClickRemark.Text = "LEFT";
            if (e.Button == MouseButtons.Right)
                labelClickRemark.Text = "RIGHT";
        }

        private void pictureBoxCam_MouseUp(object sender, MouseEventArgs e)
        {
            labelClickRemark.Text = "OPEN";
            
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxLeftClick_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trainClickGesturesToolStripGesture_Click(object sender, EventArgs e)
        {
            _formTemplateTrain.Show();
        }

        private void pictureBoxCam_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void labelPRight_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void labelPTrack_TextChanged(object sender, EventArgs e)
        {
            
  

        }

        private void labelPTrack_Click(object sender, EventArgs e)
        {

        }

        private void TemplateMouseAction(int prediction, Point labelPointHand) 
        {
            UpdateLL(ref _listOfState, prediction);

            Mat imgCropped = new Mat(_frameRTClone.Mat, _rectFinalTemplate);
            Image<Bgr, Byte> imgCroppedFinal = imgCropped.ToImage<Bgr, Byte>();


            switch (prediction)
            {
                case CLASSIFICATION_HAND_OPEN:

                    
                   
                    
                    
                    CvInvoke.PutText(_frameRT, "Open", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);
                    
                    _handStateInt = CLASSIFICATION_HAND_OPEN;
                   
                    if (checkBoxMouseMove.Checked)
                    {
                        _roiCentroid = Geometry.RectangularCentroid(_rectFinalTemplate);
                        UpdateCoordinatesForMouse();
                        _roiCentroidPrev = _roiCentroid;

                         MoveMouse();
                    }

                    MouseClickDecision();


                    //VirtualMouse.LeftUp();
                    //VirtualMouse.RightUp();

                    break;
                case CLASSIFICATION_HAND_LEFT:
                 
                    CvInvoke.PutText(_frameRT, "Left", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);

                    _handStateInt = CLASSIFICATION_HAND_LEFT;
                    RightClickDecision();        
   
                    UpdateCoordinatesForMouse();
                    //MoveMouse();

                    if (checkBoxLeftClick.Checked && !_leftDown )
                    {
                        VirtualMouse.LeftDown();
                        _leftDown = true;
                    }

                    
                    break;
                case CLASSIFICATION_HAND_RIGHT:
                 
                    CvInvoke.PutText(_frameRT, "Right", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);

                    _handStateInt = CLASSIFICATION_HAND_RIGHT;

                    LeftClickDecision();

                    UpdateCoordinatesForMouse();
                    //MoveMouse();
         

                    if (checkBoxRightClick.Checked && !_rightDown)
                    {
                        VirtualMouse.RightDown();
                        _rightDown = true;
                        //_rightDown = true;
                    }

                    break;
                case CLASSIFICATION_HAND_DOWN:

                    CvInvoke.PutText(_frameRT, "Down", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);
                    
                    UpdateCoordinatesForMouse();
                
                    if(checkBoxScroll.Checked)
                      VirtualMouse.ScrollDown();

                    MouseClickDecision();
                  
                    break;
                case CLASSIFICATION_HAND_UP:

                    CvInvoke.PutText(_frameRT, "Up", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);
                    UpdateCoordinatesForMouse();

                    if (checkBoxScroll.Checked)
                    VirtualMouse.ScrollUp();

                    MouseClickDecision();
                
                    break;
                case CLASSIFICATION_HAND_HALT:
            
                    CvInvoke.PutText(_frameRT, "Halt", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);
                    UpdateCoordinatesForMouse();
                    _isHalt = true;

                    MouseClickDecision();

                    //_roiCentroid = new Point(0, 0);
                    //_roiCentroidPrev = new Point(0, 0);
                    //if (_leftDown)
                    //{
                    //    VirtualMouse.LeftUp();
                    //    _leftDown = false;
                    //}

                    //if (_rightDown)
                    //{
                    //    VirtualMouse.RightUp();
                    //    _rightDown = false;
                    //}

                    break;
                default:
                    CvInvoke.PutText(_frameRT, "None", _rectFinalTemplate.Location, FontFace.HersheyComplex, 0.7, _mcvBlue, 1);
                    break;
            }
        
        }

        private void MouseClickDecision()
        {
            LeftClickDecision();

            RightClickDecision();
        }

        private void RightClickDecision()
        {
            if (_rightDown)
            {
                _rightDown = false;
                VirtualMouse.RightUp();
            }
        }

        private void LeftClickDecision()
        {
            if (_leftDown)
            {
                _leftDown = false;
                VirtualMouse.LeftUp();
            }
        }

    }
}
       