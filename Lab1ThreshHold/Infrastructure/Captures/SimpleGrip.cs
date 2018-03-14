using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Lab1ThreshHold.Infrastructure.FrameCapture;

namespace Lab1ThreshHold.Infrastructure.Captures
{
    public class SimpleGrip :Grip
    {
        protected override void Process(Image<Gray, byte> queryImage)
        {
            
        }
    }
}
