using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime;
using Engineer.Mathematics;

namespace Engineer.Data
{
    public class FileContainer
    {
        public FileContainer()
        {
        }
        public virtual void Load(string FilePath, BackgroundWorker Worker)
        {
            GCSettings.LargeObjectHeapCompactionMode
                = GCLargeObjectHeapCompactionMode.CompactOnce;
            System.GC.Collect();
        }
        public virtual void Save(string FilePath, BackgroundWorker Worker)
        {
        }
    }
}
