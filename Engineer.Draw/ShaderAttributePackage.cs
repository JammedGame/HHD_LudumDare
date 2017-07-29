using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Draw
{
    public class ShaderAttributePackage
    {
        protected bool _BufferExists;
        protected bool _DataChanged;
        protected bool _AttributesBound;
        protected int _BufferLines;
        protected int _BufferLineLength;
        protected int _VertexArray_Indexer;
        protected int _VertexBuffer_Indexer;
        protected int _ManualBufferLines;
        protected byte[] _ManualDataArray;
        protected List<int> _Size;
        protected List<int> _DataSize;
        protected List<string> _ID;
        protected List<string> _Type;
        protected List<byte[]> _Data;
        protected virtual bool ActivateAttributesWithManualBuffer(int Program_Indexer)
        {
            return false;
        }
        public int BufferLines
        {
            get
            {
                return _BufferLines;
            }

            set
            {
                _BufferLines = value;
            }
        }
        public bool BufferExists
        {
            get
            {
                return _BufferExists;
            }

            set
            {
                _BufferExists = value;
            }
        }
        public ShaderAttributePackage()
        {
            this._DataChanged = true;
            this._Size = new List<int>();
            this._DataSize = new List<int>();
            this._ID = new List<string>();
            this._Type = new List<string>();
            this._Data = new List<byte[]>();
        }
        public ShaderAttributePackage(ShaderAttributePackage ShaderAttributePackage)
        {
            this._DataChanged = true;
            this._AttributesBound = ShaderAttributePackage._AttributesBound;
            this._BufferLines = ShaderAttributePackage._BufferLines;
            this._BufferLineLength = ShaderAttributePackage._BufferLineLength;
            this._VertexArray_Indexer = ShaderAttributePackage._VertexArray_Indexer;
            this._VertexBuffer_Indexer = ShaderAttributePackage._VertexBuffer_Indexer;
            this._ManualBufferLines = ShaderAttributePackage._ManualBufferLines;
            this._ManualDataArray = new byte[ShaderAttributePackage._ManualDataArray.Length];
            for(int i = 0; i < ShaderAttributePackage._ManualDataArray.Length; i++ ) this._ManualDataArray[i] = ShaderAttributePackage._ManualDataArray[i];
            this._Size = new List<int>(ShaderAttributePackage._Size);
            this._DataSize = new List<int>(ShaderAttributePackage._DataSize);
            this._ID = new List<string>(ShaderAttributePackage._ID);
            this._Type = new List<string>(ShaderAttributePackage._Type);
            this._Data = new List<byte[]>(ShaderAttributePackage._Data);
        }
        public virtual bool SetDefinition(string ID, int Size, string Type)
        {
            if (_AttributesBound) return false;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) return false;
            }
            _ID.Add(ID);
            _Type.Add(Type);
            _Size.Add(Size);
            _DataSize.Add(0);
            _Data.Add(null);
            return true;
        }
        public virtual bool SetData(string ID, int DataSize, byte[] Data)
        {
            int Index = -1;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) Index = i;
            }
            if (Index == -1) return false;
            _Data[Index] = Data;
            _DataSize[Index] = DataSize;
            _DataChanged = true;
            return true;
        }
        public virtual bool DeleteDefinition(string ID)
        {
            int Index = -1;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_ID[i] == ID) Index = i;
            }
            if (Index == -1) return false;
            _ID.RemoveAt(Index);
            _Type.RemoveAt(Index);
            _Size.RemoveAt(Index);
            _DataSize.RemoveAt(Index);
            _Data.RemoveAt(Index);
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
        public virtual void Bind(int Program_Indexer)
        {

        }
        public virtual void ClearData()
        {
            _AttributesBound = false;
            for (int i = 0; i < _ID.Count; i++)
            {
                _DataSize[i] = 0;
                _Data[i] = null;
            }
        }
        public virtual bool Activate(int Program_Indexer)
        {
            return false;
        }
        public virtual void SetDataManualy(int BufferLines, byte[] Data)
        {
            _ManualBufferLines = BufferLines;
            _ManualDataArray = Data;
        }
    }
}
