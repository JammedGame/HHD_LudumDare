using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;
using Engineer.Mathematics;

namespace Engineer.Data
{
    public class BVHContainer : SkeletonContainer
    {
        private char[] _WhiteSpaces = { ' ', '\t' };
        public BVHContainer() : base()
        {

        }
        public BVHContainer(SkeletonContainer SC) : base(SC)
        {

        }
        private string[] CleanStringArray(string[] Array)
        {
            List<string> NewArray = new List<string>();
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] != "") NewArray.Add(Array[i]);
            }
            return NewArray.ToArray();
        }
        private void ReadSegment(string Name, Segment Parent, StreamReader Reader)
        {
            Segment Current = new Segment(Name, Parent);
            if (Parent != null) Parent.Children.Add(Current);
            Current.Index = this.Segments.Count;
            this.Segments.Add(Current);
            Reader.ReadLine(); // Opening Bracket
            object[] Tags = new object[2];
            string CurrentLine;
            while (true)
            {
                CurrentLine = Reader.ReadLine();
                if (CurrentLine.Contains("OFFSET"))
                {
                    string[] Parts = CleanStringArray(CurrentLine.Split(_WhiteSpaces));
                    Vertex Offset = new Vertex(Convert.ToSingle(Parts[1]) * 100, Convert.ToSingle(Parts[2]) * 100, Convert.ToSingle(Parts[3]) * 100);
                    if (Parent != null) Parent.BoneLength = Operations.GetDistance(new Vertex(0, 0, 0), Offset);
                    Tags[0] = Offset;
                }
                else if (CurrentLine.Contains("CHANNELS"))
                {
                    string[] Parts = CleanStringArray(CurrentLine.Split(_WhiteSpaces));
                    List<string> Channels = new List<string>();
                    for (int i = 2; i < Parts.Length; i++)
                    {
                        Channels.Add(Parts[i]);
                    }
                    Tags[1] = Channels;
                }
                else if (CurrentLine.Contains("JOINT"))
                {
                    string[] Parts = CleanStringArray(CurrentLine.Split(_WhiteSpaces));
                    ReadSegment(Parts[1], Current, Reader);
                }
                else if (CurrentLine.Contains("End Site"))
                {
                    Reader.ReadLine();
                    CurrentLine = Reader.ReadLine();
                    string[] Parts = CleanStringArray(CurrentLine.Split(_WhiteSpaces));
                    Vertex Offset = new Vertex(Convert.ToSingle(Parts[1]), Convert.ToSingle(Parts[2]), Convert.ToSingle(Parts[3]));
                    Current.BoneLength = Operations.GetDistance(new Vertex(0, 0, 0), Offset);
                    Reader.ReadLine();
                }
                else if (CurrentLine.Contains("}")) break;
            }
            Current.Tag = Tags;
        }
        public override void Load(string FilePath, BackgroundWorker Worker)
        {
            FileStream Stream = new FileStream(FilePath, FileMode.Open);
            StreamReader Reader = new StreamReader(Stream);
            Reader.ReadLine();
            string[] Parts = CleanStringArray(Reader.ReadLine().Split(_WhiteSpaces));
            ReadSegment(Parts[1], null, Reader);
            Reader.ReadLine();
            Parts = CleanStringArray(Reader.ReadLine().Split(_WhiteSpaces));
            int NumFrames = Convert.ToInt32(Parts[1]);
            this.StartFrame = 0;
            this.EndFrame = NumFrames - 1;
            Parts = CleanStringArray(Reader.ReadLine().Split(_WhiteSpaces));
            float FrameTime = Convert.ToSingle(Parts[2]);
            this.DataRate = (float)Math.Round(1.0 / FrameTime);
            for (int i = 0; i < NumFrames; i++)
            {
                Parts = CleanStringArray(Reader.ReadLine().Split(_WhiteSpaces));
                int index = 0;
                for (int j = 0; j < this.Segments.Count; j++)
                {
                    object[] Tags = this.Segments[j].Tag as object[];
                    List<string> Channels = Tags[1] as List<string>;
                    Vertex Offset = (Vertex)Tags[0];
                    Vertex Translation = Offset;
                    Vertex Rotation = new Vertex();
                    Vertex Scale = new Vertex(1, 1, 1);
                    for (int k = 0; k < Channels.Count; k++)
                    {
                        if (Channels[k] == "Xposition") Translation.X += Convert.ToSingle(Parts[index + k]) * 100;
                        else if (Channels[k] == "Yposition") Translation.Y += Convert.ToSingle(Parts[index + k]) * 100;
                        else if (Channels[k] == "Zposition") Translation.Z += Convert.ToSingle(Parts[index + k]) * 100;
                        else if (Channels[k] == "Xrotation") Rotation.X = Convert.ToSingle(Parts[index + k]);
                        else if (Channels[k] == "Yrotation") Rotation.Y = Convert.ToSingle(Parts[index + k]);
                        else if (Channels[k] == "Zrotation") Rotation.Z = Convert.ToSingle(Parts[index + k]);
                        else if (Channels[k] == "Xscale") Scale.X = Convert.ToSingle(Parts[index + k]);
                        else if (Channels[k] == "Yscale") Scale.Y = Convert.ToSingle(Parts[index + k]);
                        else if (Channels[k] == "Zscale") Scale.Z = Convert.ToSingle(Parts[index + k]);
                    }
                    this.Segments[j].Translation.Add(Translation);
                    this.Segments[j].Rotation.Add(Rotation);
                    this.Segments[j].Scale.Add(Scale);
                    index += Channels.Count;
                }
            }
            this.EulerRotationOrder = "ZYX";
            Reader.Close();
        }
    }
}
