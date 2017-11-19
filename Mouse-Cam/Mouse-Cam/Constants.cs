using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hand_Virtual_Mouse
{
    public class UpperLowerLimit 
    {
        public double Limit { get; set; }
        public bool IsLower { get; set; }

        public bool Evaluation { get; set; }

        public UpperLowerLimit() 
        {

        }

        public void Evaluate(double input) 
        {
            if (IsLower)
            {
                Evaluation = Limit <= input;
            }
            else 
            {
                Evaluation = Limit >= input;
            }
                
        }
    }

    public class ULConstants
    {
        public UpperLowerLimit LeftDiffStartX { get;set; }
        public UpperLowerLimit LeftDiffStartY { get; set; }
        public UpperLowerLimit LeftDiffEndX { get; set; }
        public UpperLowerLimit LeftDiffEndY { get; set; }

        public ULConstants() 
        {
            LeftDiffStartX = new UpperLowerLimit();
            LeftDiffStartY = new UpperLowerLimit();
            LeftDiffEndX = new UpperLowerLimit();
            LeftDiffEndY = new UpperLowerLimit();
        }

    }
}
