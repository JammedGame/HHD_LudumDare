using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine.IO
{
    public class Interface
    {
        protected virtual void WriteConfig(Type ObjectType, string FilePath)
        {
            StreamWriter Stream = new StreamWriter(FilePath);
            Stream.WriteLine("Engineer File Config:");
            Stream.WriteLine("");
            Stream.WriteLine("Version:");
            Stream.WriteLine(Settings.Version);
            Stream.WriteLine("FileType:");
            Stream.WriteLine(ObjectType.ToString());
            Stream.Close();
        }
        protected virtual object[] ReadConfig(string FilePath)
        {
            object[] Data = new object[2];
            StreamReader Reader = new StreamReader(FilePath);
            Reader.ReadLine();
            Reader.ReadLine();
            Reader.ReadLine();
            string Version = Reader.ReadLine();
            Data[0] = Version;
            Reader.ReadLine();
            string FileType = Reader.ReadLine();
            Data[1] = FileType;
            Reader.Close();
            return Data;
        }
        protected virtual void SaveObject(object ObjectToSave, string FilePath, bool Head)
        {

        }
        protected virtual object LoadObject(string DirPath, Type FileType)
        {
            return null;
        }
        public virtual bool SaveData(object ObjectToSave, string FilePath, ref string Log)
        {
            try
            {
                string DirPath = Path.GetDirectoryName(FilePath) + "/" + Path.GetFileNameWithoutExtension(FilePath) + "/";
                if (Directory.Exists(DirPath)) Directory.Delete(DirPath, true);
                SaveObject(ObjectToSave, FilePath, true);
                if (File.Exists(FilePath)) File.Delete(FilePath);
                ZipFile.CreateFromDirectory(DirPath, FilePath);
                Directory.Delete(DirPath, true);
                Log = "Success";
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Log = ex.InnerException.ToString();
                }
                else Log = ex.Message;
                return false;
            }
        }
        public virtual bool LoadData(ref object LoadedObject, string FilePath, ref string Log)
        {
            try
            {
                string DirPath = Path.GetDirectoryName(FilePath) + "/" + Path.GetFileNameWithoutExtension(FilePath) + "/";
                if (Directory.Exists(DirPath)) Directory.Delete(DirPath, true);
                ZipFile.ExtractToDirectory(FilePath, DirPath);
                object[] Config = ReadConfig(DirPath + "data.conf");
                string FileType = Config[1] as string;
                Assembly ASM = typeof(Game).Assembly;
                LoadedObject = LoadObject(DirPath, ASM.GetType(FileType));
                Directory.Delete(DirPath, true);
                Log = "Success";
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Log = ex.InnerException.ToString();
                }
                else Log = ex.Message;
                return false;
            }
        }
        public void Save(object ObjectToSave, string FilePath)
        {
            string Log = "";
            if(!SaveData(ObjectToSave, FilePath, ref Log))
            {
                Console.WriteLine(Log);
            }
        }
        public object Load(string FilePath)
        {
            string Log = "";
            object Loaded = null;
            if(!LoadData(ref Loaded, FilePath, ref Log))
            {
                Console.WriteLine(Log);
            }
            return Loaded;
        }
    }
}
