using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Accord.MachineLearning;
using Accord.Statistics;


namespace Hand_Virtual_Mouse
{
    public class KnnIO
    {
        public double[][] Inputs { get; set; }
        public int[] Outputs { get; set; }

        public char[] sep = new char[] { ',' };

        private List<double[]> inputList = new List<double[]>();
        private List<int> outputList = new List<int>();

        public KnnIO(string path, double scale) 
        {
            List<string> dataLines = File.ReadAllLines(path).ToList();

            foreach (string dataLine in dataLines.GetRange(1, dataLines.Count - 1))
            {
                List<string> dataRaw = dataLine.Split(sep).ToList();

                List<double> actualFeatureList = new List<double>();

                dataRaw.GetRange(0, dataRaw.Count - 1).
                    ForEach(s => actualFeatureList.Add(scale*Double.Parse(s)));

                int classification = Int32.Parse(dataRaw[dataRaw.Count - 1]);

                inputList.Add(actualFeatureList.ToArray());
                outputList.Add(classification);
            }

            Inputs = inputList.ToArray();
            Outputs = outputList.ToArray();
        
        }
        public KnnIO(string path)
        {
            List<string> dataLines = File.ReadAllLines(path).ToList();

            foreach (string dataLine in dataLines.GetRange(1, dataLines.Count - 1))
            {
                List<string> dataRaw = dataLine.Split(sep).ToList();

                List<double> actualFeatureList = new List<double>();

                dataRaw.GetRange(0, dataRaw.Count - 1).
                    ForEach(s => actualFeatureList.Add(Double.Parse(s)));

                int classification = Int32.Parse(dataRaw[dataRaw.Count - 1]);

                inputList.Add(actualFeatureList.ToArray());
                outputList.Add(classification);
            }

            Inputs = inputList.ToArray();
            Outputs = outputList.ToArray();
        }
    }

    public class TemplateFeatures 
    {
        public static readonly int CLASSIFICATION_HAND_OPEN = 0;
        public static readonly int CLASSIFICATION_HAND_LEFT = 1;
        public static readonly int CLASSIFICATION_HAND_RIGHT = 2;
        public static readonly int CLASSIFICATION_HAND_DOWN = 3;
        public static readonly int CLASSIFICATION_HAND_UP = 4;
        public static readonly int CLASSIFICATION_HAND_HALT = 5;

        public double[] TemplateFeaturesList { get; set; }
        public TemplateFeatures(List<double> features) 
        {
            TemplateFeaturesList = features.ToArray();
        }
    }

    public class TemplatePaths 
    {
        public static readonly string NODE_MAIN_TEMPLATE = "data\\template_data\\node_main.csv";
        public static readonly string NODE_A_TEMPLATE = "data\\template_data\\node_a.csv";
        public static readonly string NODE_B_TEMPLATE = "data\\template_data\\node_b.csv";
        public static readonly string NODE_CLICK_TEMPLATE = "data\\template_data\\node_click.csv";
        public static readonly string NODE_SCROLL_TEMPLATE = "data\\template_data\\node_scroll.csv";
        
    }
    public class TemplateMatchDecisionTree
    {
        private Image<Gray, Byte> _template;
        private TemplateMatchClassifier _nodeMainTemplate { get; set; }

        private TemplateMatchClassifier _nodeATemplate { get; set; }
        private TemplateMatchClassifier _nodeOpenTemplate { get; set; }
        private TemplateMatchClassifier _nodeClickTemplate { get; set; }
        //public TemplateNode NodeRightTemplate { get; set; }

        private TemplateMatchClassifier _nodeBTemplate { get; set; }
        private TemplateMatchClassifier _nodeHaltTemplate { get; set; }
        private TemplateMatchClassifier _nodeScrollTemplate { get; set; }
        //public TemplateNode NodeScrollUpTemplate { get; set; }

        ////private int TemplateMatch(TemplateFeatures features)
        ////{
        ////    return 
        ////}
        //public TemplateMatchDecisionTree() 
        ////{
        ////    TemplateIO 
        //}

        public TemplateMatchDecisionTree() 
        {
            KnnIO tmpIOMain = new KnnIO(TemplatePaths.NODE_MAIN_TEMPLATE);
            KnnIO tmpIOA = new KnnIO(TemplatePaths.NODE_A_TEMPLATE);
            KnnIO tmpIOB = new KnnIO(TemplatePaths.NODE_B_TEMPLATE);
            KnnIO tmpIOClick = new KnnIO(TemplatePaths.NODE_CLICK_TEMPLATE);
            KnnIO tmpIOScroll = new KnnIO(TemplatePaths.NODE_SCROLL_TEMPLATE);

            _nodeMainTemplate = new TemplateMatchClassifier(2, tmpIOMain.Inputs, tmpIOMain.Outputs);
            _nodeATemplate = new TemplateMatchClassifier(2, tmpIOA.Inputs, tmpIOA.Outputs);
            _nodeBTemplate = new TemplateMatchClassifier(2, tmpIOB.Inputs, tmpIOB.Outputs);
            _nodeClickTemplate = new TemplateMatchClassifier(2, tmpIOClick.Inputs, tmpIOClick.Outputs);
            _nodeScrollTemplate = new TemplateMatchClassifier(4, tmpIOScroll.Inputs, tmpIOScroll.Outputs);
   
        }
        public int TemplateMatch(Image<Gray, Byte> template, TemplateFeatures features) 
        {
            _template = template;

            int firstNode = _nodeMainTemplate.TemplateMatch(features);

            switch (firstNode) 
            {
                case 0:    // a chosen
                    // second stage
                    int secondNodeA = 
                        _nodeATemplate.TemplateMatch(features.TemplateFeaturesList.ToList().GetRange(0,3).ToArray());

                    switch (secondNodeA) 
                    {
                        case 0:  // open chosen
                            return TemplateFeatures.CLASSIFICATION_HAND_OPEN;
                        case 1: // right or left chosen
                            int thirdNode = _nodeClickTemplate.TemplateMatch(features.TemplateFeaturesList.ToList().GetRange(0,2).ToArray());
                            
                            switch (thirdNode) 
                            {
                                case 0: // left click        
                                    return TemplateFeatures.CLASSIFICATION_HAND_LEFT;
                                case 1: // right click
                                    return TemplateFeatures.CLASSIFICATION_HAND_RIGHT;
                                default:
                                    return -1;
                            }
                    
                        default:
                            return -1;
                    }

                case 1:     // B chosen
                    int secondNodeB = _nodeBTemplate.TemplateMatch(features);

                    switch (secondNodeB) 
                    {
                        case 0:
                            return TemplateFeatures.CLASSIFICATION_HAND_HALT;
                        case 1:
                            int thirdNode = _nodeScrollTemplate.TemplateMatch(features);

                            switch (thirdNode)
                            {
                                case 0: // left click        
                                    return TemplateFeatures.CLASSIFICATION_HAND_DOWN;
                                case 1: // right click
                                    return TemplateFeatures.CLASSIFICATION_HAND_UP;
                                default:
                                    return -1;
                            }

                        default:
                            return -1;
                    
                    }

                default:
                    return -1;
            }


        } 

    }
    public class TemplateMatchClassifier : KNearestNeighbors 
    {
        //public TemplateNode(int K, double[][] input, int[] output) 
        //{
            
        //}
        public TemplateMatchClassifier(int k, double[][] inputs, int[] outputs) : base(k, inputs, outputs) { }
        private int TemplateCompute(TemplateFeatures tf) 
        {
            //KNearestNeighbors knn = new KNearestNeighbors()
            return Compute(tf.TemplateFeaturesList);
        }

        private int TemplateCompute(double[] list)
        {
            //KNearestNeighbors knn = new KNearestNeighbors()
            return Compute(list);
        }
        public int TemplateMatch(TemplateFeatures tf) 
        {
            return TemplateCompute(tf);
        }

        public int TemplateMatch(double[] list)
        {
            return TemplateCompute(list);
        }
    }

    //public class TemplateMatcher
    //{
    //}
}
