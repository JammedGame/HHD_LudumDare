using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engineer.Data
{
    public class DAESkeletonContainer : SkeletonContainer
    {
        public override void Load(string FilePath, BackgroundWorker Worker)
        {
            XmlDocument Document = new XmlDocument();
            Document.Load(FilePath);
            XmlNode Main = Document.FirstChild;
            XmlNode Skeleton = null;
            for(int i = 0; i < Main.ChildNodes.Count; i++)
            {
                if (Main.ChildNodes[i].Name == "library_visual_scenes")
                {
                    Skeleton = Main.ChildNodes[i];
                    break;
                }
            }
            if(Skeleton != null)
            {
                XmlNode Root = Skeleton.FirstChild;

            }
        }
    }
    public class DAEMeshContainer : MeshContainer
    {
        public override void Load(string FilePath, BackgroundWorker Worker)
        {

        }
    }
}
