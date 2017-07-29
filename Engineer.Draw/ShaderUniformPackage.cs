using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Draw
{
    public class ShaderUniformPackage
    {
        protected List<int> _Size;
        protected List<string> _ID;
        protected List<string> _Type;
        protected List<byte[]> _Data;
        public ShaderUniformPackage()
        {
            this._Size = new List<int>();
            this._ID = new List<string>();
            this._Type = new List<string>();
            this._Data = new List<byte[]>();
        }
        public ShaderUniformPackage(ShaderUniformPackage ShaderUniformPackage)
        {
            this._Size = new List<int>(ShaderUniformPackage._Size);
            this._ID = new List<string>(ShaderUniformPackage._ID);
            this._Type = new List<string>(ShaderUniformPackage._Type);
            this._Data = new List<byte[]>(ShaderUniformPackage._Data);
        }
        public virtual bool SetDefinition(string ID, int Size, string Type)
        {
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) return false;
            }
            _ID.Add(ID);
            _Type.Add(Type);
            _Size.Add(Size);
            _Data.Add(null);
            return true;
        }
        public virtual bool SetData(string ID, byte[] Data)
        {
            int Index = -1;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) Index = i;
            }
            if (Index == -1) return false;
            _Data[Index] = Data;
            return true;
        }
        public virtual bool Exists(string ID)
        {
            int Index = -1;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) Index = i;
            }
            if (Index == -1) return false;
            return true;
        }
        public virtual bool Delete(string ID)
        {
            int Index = -1;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) Index = i;
            }
            if (Index == -1) return false;
            this._ID.RemoveAt(Index);
            this._Size.RemoveAt(Index);
            this._Type.RemoveAt(Index);
            this._Data.RemoveAt(Index);
            return true;
        }
        public virtual bool Activate(int Program_Indexer)
        {
            return true;
        }
        public virtual void ClearData()
        {
            for (int i = 0; i < _ID.Count; i++)
            {
                _Data[i] = null;
            }
        }
        public virtual void Update(ShaderUniformPackage ShaderUniformPackage)
        {
            for(int i = 0; i < ShaderUniformPackage._ID.Count; i++)
            {
                if (this.Exists(ShaderUniformPackage._ID[i])) this.SetData(ShaderUniformPackage._ID[i], ShaderUniformPackage._Data[i]);
                else
                {
                    this.SetDefinition(ShaderUniformPackage._ID[i], ShaderUniformPackage._Size[i], ShaderUniformPackage._Type[i]);
                    this.SetData(ShaderUniformPackage._ID[i], ShaderUniformPackage._Data[i]);
                }
            }
        }
    }
}
