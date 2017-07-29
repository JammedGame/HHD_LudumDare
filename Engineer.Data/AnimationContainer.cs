using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.Mathematics;

namespace Engineer.Data
{
    public class AnimationContainer : FileContainer
    {
        private Axis _AxisUp;
        private int _StartFrame;
        private int _EndFrame;
        private float _DataRate;
        private TimeCode _StartTime;
        public Axis AxisUp
        {
            get { return this._AxisUp; }
            set { this._AxisUp = value; }
        }
        public int StartFrame
        {
            get { return this._StartFrame; }
            set { this._StartFrame = value; }
        }
        public int EndFrame
        {
            get { return this._EndFrame; }
            set { this._EndFrame = value; }
        }
        public int NumFrames
        {
            get { return this._EndFrame - this._StartFrame + 1; }
        }
        public float DataRate
        {
            get { return _DataRate; }
            set { _DataRate = value; }
        }
        public TimeCode StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        public AnimationContainer()
        {
            this._AxisUp = Axis.Y;
            this._StartFrame = 0;
            this._EndFrame = 0;
            this._DataRate = 0;
            this._StartTime = new TimeCode();
        }
        public AnimationContainer(AnimationContainer AC)
        {
            this._AxisUp = AC._AxisUp;
            this._StartFrame = AC._StartFrame;
            this._EndFrame = AC._EndFrame;
            this._DataRate = AC._DataRate;
            this._StartTime = new TimeCode(AC._StartTime);
        }
    }
}
