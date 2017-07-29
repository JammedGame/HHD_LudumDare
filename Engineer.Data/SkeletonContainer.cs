using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Data
{
    public class SkeletonContainer : AnimationContainer
    {
        private string _EulerRotationOrder;
        public string EulerRotationOrder
        {
            get { return _EulerRotationOrder; }
            set { _EulerRotationOrder = value; }
        }
        private List<Segment> _Segments;
        public int NumSegments
        {
            get
            {
                return this._Segments.Count;
            }
        }
        public List<Segment> Segments
        {
            get { return _Segments; }
            set { _Segments = value; }
        }
        public SkeletonContainer()
            : base()
        {
            this._Segments = new List<Segment>();
        }
        public SkeletonContainer(SkeletonContainer SC) : base(SC)
        {
            this._EulerRotationOrder = SC._EulerRotationOrder;
            this._Segments = new List<Segment>();
            for (int i = 0; i < SC.Segments.Count; i++) this._Segments.Add(new Segment(SC.Segments[i]));
        }
        public void AddSkeleton(SkeletonContainer SC)
        {
            int NStartFrame = StartFrame;
            if (SC.StartFrame < NStartFrame) NStartFrame = SC.StartFrame;
            int NEndFrame = EndFrame;
            if (SC.EndFrame > NEndFrame) NEndFrame = SC.EndFrame;
            if (StartTime.IsValid()) StartTime += (NStartFrame - StartFrame);
        }
    }
}
