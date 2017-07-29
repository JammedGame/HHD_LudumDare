using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Engineer.Draw.OpenGL.GLSL
{
    public class GLSLShaderAttributePackage : ShaderAttributePackage
    {
        private GCHandle _BufferPointer;
        protected override bool ActivateAttributesWithManualBuffer(int Program_Indexer)
        {
            int CurrentOffset = 0;
            GL.GenVertexArrays(1, out _VertexArray_Indexer);
            GL.BindVertexArray(_VertexArray_Indexer);
            GL.GenBuffers(1, out _VertexBuffer_Indexer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VertexBuffer_Indexer);
            int bufferSize = _ManualBufferLines * _BufferLineLength;
            _BufferPointer = GCHandle.Alloc(_ManualDataArray, GCHandleType.Pinned);
            IntPtr ManualBuffer = _BufferPointer.AddrOfPinnedObject();
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(bufferSize), ManualBuffer, BufferUsageHint.StaticDraw);
            int Attribute = 0;
            int Offset = 0;
            _BufferLines = _ManualBufferLines;
            for (int i = 0; i < _ID.Count; i++)
            {
                Attribute = i;
                int AtributeSize = _Size[i] / sizeof(float);
                int LineLength = _BufferLineLength / _BufferLines;
                GL.EnableVertexAttribArray(Attribute);
                GL.VertexAttribPointer(Attribute, AtributeSize, VertexAttribPointerType.Float, false, _BufferLineLength, Offset);
                Offset += _Size[i];
                CurrentOffset += _Size[i];
            }
            _ManualBufferLines = 0;
	        return true;
        }
        public GLSLShaderAttributePackage() : base ()
        {

        }
        public GLSLShaderAttributePackage(ShaderAttributePackage ShaderAttributePackage) : base(ShaderAttributePackage)
        {
            GLSLShaderAttributePackage Package = ShaderAttributePackage as GLSLShaderAttributePackage;
            if(Package != null) this._BufferPointer = Package._BufferPointer;
        }
        public GLSLShaderAttributePackage(GLSLShaderAttributePackage GLSLShaderAttributePackage) : base (GLSLShaderAttributePackage)
        {
            this._BufferPointer = GLSLShaderAttributePackage._BufferPointer;
        }
        public override void Bind(int Program_Indexer)
        {
            _BufferLineLength = 0;
            for (int i = 0; i < _ID.Count; i++)
            {
                GL.BindAttribLocation(Program_Indexer, i, _ID[i]);
                _BufferLineLength += _Size[i];
            }
        }
        public override void ClearData()
        {
            this._BufferExists = false;
            GL.DeleteBuffers(1, ref _VertexBuffer_Indexer);
            GL.DeleteVertexArrays(1, ref _VertexArray_Indexer);
            base.ClearData();
        }
        public override bool Activate(int Program_Indexer)
        {
            if (!_DataChanged)
            {
                GL.BindVertexArray(_VertexArray_Indexer);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _VertexBuffer_Indexer);
                return true;
            }
            GL.DeleteBuffers(1, ref _VertexBuffer_Indexer);
            GL.DeleteVertexArrays(1, ref _VertexArray_Indexer);
            if (_ManualBufferLines > 0) return ActivateAttributesWithManualBuffer(Program_Indexer);
            int CurrentOffset = 0;
            byte[] Data;
            int ByteTotalLength = 0;
            int ByteLineSize = 0;
            int BiggestRatioIndex = 0;
            int BiggestRatioSize = 0;
            for (int i = 0; i < _ID.Count; i++)
            {
                int DataSizeToAttributeSize = _DataSize[i] / _Size[i];
                if (DataSizeToAttributeSize > BiggestRatioSize)
                {
                    BiggestRatioSize = DataSizeToAttributeSize;
                    BiggestRatioIndex = i;
                }
                ByteLineSize += _Size[i];
            }
            ByteTotalLength = BiggestRatioSize * ByteLineSize;
            Data = new byte[ByteTotalLength];
            int CurrentDataOffset;
            for (int i = 0; i < BiggestRatioSize; i++)
            {
                CurrentDataOffset = 0;
                for (int j = 0; j < _ID.Count; j++)
                {
                    for (int k = 0; k < _Size[j]; k++)
                        Data[i * ByteLineSize + CurrentDataOffset + k] = (_Data[j])[i * _Size[j] + k];
                    CurrentDataOffset += _Size[j];
                }
            }
            _BufferLines = BiggestRatioSize;
            if (_BufferLines == 0) return false;
            GL.GenVertexArrays(1, out _VertexArray_Indexer);
            GL.BindVertexArray(_VertexArray_Indexer);
            GL.GenBuffers(1, out _VertexBuffer_Indexer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VertexBuffer_Indexer);
            int bufferSize = ByteTotalLength;
            _BufferPointer = GCHandle.Alloc(Data, GCHandleType.Pinned);
            IntPtr CollectedBuffer = _BufferPointer.AddrOfPinnedObject();
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(bufferSize), CollectedBuffer, BufferUsageHint.StaticDraw);
            int Attribute = 0;
            int Offset = 0;
            for (int i = 0; i < _ID.Count; i++)
            {
                Attribute = i;
                int AtributeSize = _Size[i] / sizeof(float);
                int LineLength = _BufferLineLength / _BufferLines;
                GL.EnableVertexAttribArray(Attribute);
                GL.VertexAttribPointer(Attribute, AtributeSize, VertexAttribPointerType.Float, false, _BufferLineLength, Offset);
                Offset += _Size[i];
                CurrentOffset += _Size[i];
            }
            _BufferPointer.Free();
            this._BufferExists = true;
            this._DataChanged = false;
	        return true;
        }
    }
}
