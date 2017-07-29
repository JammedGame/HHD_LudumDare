using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engineer.Project
{
    class ResourceManager
    {
        public static Dictionary<string, Bitmap> Images;
        public static ResourceManager RM;
        public ResourceManager()
        {
            XmlDocument Document = new XmlDocument();
            Document.Load("Data/Default.mtx");
            XmlNode Main = Document.FirstChild;
            Engineer.Engine.Material Mat = new Engineer.Engine.Material(Main);
            Engineer.Engine.Material.Default = Mat;
            Init();
        }
        public void Init()
        {
            RM = this;
            Images = new Dictionary<string, Bitmap>();
            ResourceSet resourceSet = global::Engineer.Project.Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;
                Images.Add(resourceKey, (Bitmap)resource);
            }
        }
    }
}
