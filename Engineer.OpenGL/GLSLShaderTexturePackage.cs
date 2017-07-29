using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Engineer.Draw.OpenGL
{
    public class GLSLShaderTexturePackage : ShaderTexturePackage
    {
        private uint TexturesPointer;
        private GCHandle _BufferPointer;
        public GLSLShaderTexturePackage() : base()
        {

        }
        public GLSLShaderTexturePackage(ShaderTexturePackage Package) : base(Package)
        {

        }
        public override bool Activate()
        {
            if (GLSLShaderTexturePackage._Binded && GLSLShaderTexturePackage._Index == TexturesPointer) return true;
            //if (GLSLShaderTexturePackage._Binded)GL.DeleteTexture(GLSLShaderTexturePackage._Index);
            if (this._TexturesNumber == 0) return true;
            if (!this._Loaded)
            {
                GL.GenTextures(1, out TexturesPointer);
                GLSLShaderTexturePackage._Index = TexturesPointer;
                GL.BindTexture(TextureTarget.Texture2DArray, TexturesPointer);
                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (float)All.Clamp);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (float)All.Clamp);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (float)All.Linear);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (float)All.Linear);
                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                _BufferPointer = GCHandle.Alloc(this._Textures, GCHandleType.Pinned);
                IntPtr TextureBuffer = _BufferPointer.AddrOfPinnedObject();
                GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, (int)this._Resolution.X, (int)this._Resolution.Y, this._TexturesNumber, 0, PixelFormat.Bgra, PixelType.UnsignedByte, TextureBuffer);
                this._Loaded = true;
            }
            else GL.BindTexture(TextureTarget.Texture2DArray, TexturesPointer);
            this._Active = true;
            GLSLShaderTexturePackage._Binded = true;
            ShaderTexturePackage._Index = TexturesPointer;
            return true;
        }
        public override void ClearData()
        {
            this._Active = false;
            GL.DeleteTexture(TexturesPointer);
            this._TexturesNumber = 0;
            this._Textures = null;
        }
    }
}
