using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.IO
{
    public enum Architecture
    {
        IEEELittleEndian, IEEEBigEndian, VAXPDP
    }
    public class BruteStream
    {
        protected int _Index;
        private Architecture _CurrentArchitecture;
        public Architecture CurrentArchitecture { get => _CurrentArchitecture; set => _CurrentArchitecture = value; }
        private byte[] Bytes;
        public virtual bool EOF
        {
            get { return _Index >= Bytes.Length; }
        }
        public virtual int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }
        public BruteStream()
        {
            this._CurrentArchitecture = Architecture.IEEELittleEndian;
        }
        public BruteStream(string FileRoot)
        {
            this._CurrentArchitecture = Architecture.IEEELittleEndian;
            this.Bytes = File.ReadAllBytes(FileRoot);
            this._Index = 0;
        }
        public virtual void SetArchitecture(Architecture NewArchitecture)
        {
            this.CurrentArchitecture = NewArchitecture;
        }
        public virtual void JumpTo(int _Index)
        {
            this._Index = _Index;
        }
        public virtual void JumpFor(int _IndexIncrement)
        {
            this._Index += _IndexIncrement;
        }
        public virtual byte PeekByte()
        {
            return Bytes[_Index];
        }
        public virtual byte ReadByte()
        {
            _Index++;
            return Bytes[_Index - 1];
        }
        public virtual byte[] ReadBytes(int Length)
        {
            byte[] ByteArray = new byte[Length];
            for (int i = 0; i < Length; i++) ByteArray[i] = Bytes[_Index + i];
            _Index += Length;
            return ByteArray;
        }
        public virtual sbyte PeekSByte()
        {
            return (sbyte)Bytes[_Index];
        }
        public virtual sbyte ReadSByte()
        {
            _Index++;
            return (sbyte)Bytes[_Index - 1];
        }
        public virtual sbyte[] ReadSBytes(int Length)
        {
            sbyte[] ByteArray = new sbyte[Length];
            for (int i = 0; i < Length; i++) ByteArray[i] = (sbyte)Bytes[_Index + i];
            _Index += Length;
            return ByteArray;
        }
        public virtual char PeekChar()
        {
            return (char)Bytes[_Index - 1];
        }
        public virtual char ReadChar()
        {
            _Index++;
            return (char)Bytes[_Index - 1];
        }
        public virtual char[] ReadChars(int Length)
        {
            char[] CharArray = new char[Length];
            for (int i = 0; i < Length; i++) CharArray[i] = (char)Bytes[_Index + i];
            _Index += Length;
            return CharArray;
        }
        public virtual short ReadInt16()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_16();
            else return ReadLEInt_16();
        }
        public virtual ushort ReadUInt16()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (ushort)ReadBEInt_16();
            else return (ushort)ReadLEInt_16();
        }
        public virtual int ReadInt32()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_32();
            else return ReadLEInt_32();
        }
        public virtual uint ReadUInt32()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (uint)ReadBEInt_32();
            else return (uint)ReadLEInt_32();
        }
        public virtual long ReadInt64()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_64();
            else return ReadLEInt_64();
        }
        public virtual ulong ReadUInt64()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (ulong)ReadBEInt_64();
            else return (ulong)ReadLEInt_64();
        }
        public virtual float ReadSingle()
        {
            if (CurrentArchitecture == Architecture.IEEELittleEndian) return ReadIEEELESingle();
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadIEEEBESingle();
            else return ReadVAXPDPSingle();
        }
        public virtual double ReadDouble()
        {
            if (CurrentArchitecture == Architecture.IEEELittleEndian) return ReadIEEELEDouble();
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadIEEEBEDouble();
            else return Double.NaN;
        }
        public virtual unsafe short ReadLEInt_16()
        {
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                _Index += 2;
                if (_Index % 2 == 0)
                {
                    return *((short*)PointerByte);
                }
                else
                {
                    return (short)((*PointerByte) | (*(PointerByte + 1) << 8));
                }
            }
        }
        public virtual unsafe short ReadBEInt_16()
        {
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                _Index += 2;
                return (short)((*PointerByte << 8) | (*(PointerByte + 1)));
            }
        }
        public virtual unsafe int ReadLEInt_32()
        {
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                _Index += 4;
                if (_Index % 4 == 0)
                {
                    return *((int*)PointerByte);
                }
                else
                {
                    return (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                }
            }
        }
        public virtual unsafe int ReadBEInt_32()
        {
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                _Index += 4;
                return (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
            }
        }
        public virtual unsafe long ReadLEInt_64()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                if (_Index % 8 == 0)
                {
                    Value = *((long*)PointerByte);
                }
                else
                {
                    int i1 = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                    int i2 = (*(PointerByte + 4)) | (*(PointerByte + 5) << 8) | (*(PointerByte + 6) << 16) | (*(PointerByte + 7) << 24);
                    Value = (uint)i1 | ((long)i2 << 32);
                }
            }
            _Index += 8;
            return Value;
        }
        public virtual unsafe long ReadBEInt_64()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                int i1 = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
                int i2 = (*(PointerByte + 4) << 24) | (*(PointerByte + 5) << 16) | (*(PointerByte + 6) << 8) | (*(PointerByte + 7));
                Value = (uint)i2 | ((long)i1 << 32);
                return Value;
            }
        }
        public virtual unsafe float ReadIEEELESingle()
        {
            int Value;
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                if (_Index % 4 == 0)
                {
                    Value = *((int*)PointerByte);
                }
                else
                {
                    Value = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                }
            }
            _Index += 4;
            return *(float*)&Value;
        }
        public virtual unsafe float ReadIEEEBESingle()
        {
            int Value;
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                Value = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
            }
            _Index += 4;
            return *(float*)&Value;
        }
        public virtual unsafe float ReadVAXPDPSingle()
        {
            int Value = 0;
            Value = (Bytes[_Index + 1] - 1 * (Bytes[_Index + 1] == 0 ? 0 : 1)) << 24 | (Bytes[_Index] << 16) | (Bytes[_Index + 3] << 8) | Bytes[_Index + 2];
            _Index += 4;
            return *(float*)&Value;
        }
        public virtual unsafe double ReadIEEELEDouble()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                if (_Index % 8 == 0)
                {
                    Value = *((long*)PointerByte);
                }
                else
                {
                    int i1 = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                    int i2 = (*(PointerByte + 4)) | (*(PointerByte + 5) << 8) | (*(PointerByte + 6) << 16) | (*(PointerByte + 7) << 24);
                    Value = (uint)i1 | ((long)i2 << 32);
                }
            }
            _Index += 8;
            return *(double*)&Value;
        }
        public virtual unsafe double ReadIEEEBEDouble()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[_Index])
            {
                int i1 = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
                int i2 = (*(PointerByte + 4) << 24) | (*(PointerByte + 5) << 16) | (*(PointerByte + 6) << 8) | (*(PointerByte + 7));
                Value = (uint)i2 | ((long)i1 << 32);
            }
            _Index += 8;
            return *(double*)&Value;
        }
        public virtual void Close()
        {
            this.Bytes = null;
            this._Index = -1;
            System.GC.Collect();
        }
    }
    public class SmartStream : BruteStream
    {
        private bool _EOF;
        private bool _MainEOF;
        public override bool EOF
        {
            get { return (_EOF && Index >= BufferSize); }
        }
        private int BufferSize;
        private FileStream Stream;
        private byte[] BytesBuffer;
        private byte[] BytesBufferBackBuffer;
        public override int Index
        {
            get { return _Index; }
            set { SetIndex(value); }
        }
        public SmartStream(string FileRoot, int BufferSize)
        {
            this.BufferSize = (BufferSize * 1024 * 1024) / 2;
            Stream = new FileStream(FileRoot, FileMode.Open, FileAccess.Read);
            this.BytesBuffer = new byte[this.BufferSize];
            this.BytesBufferBackBuffer = new byte[this.BufferSize];
            Stream.Read(this.BytesBuffer, 0, this.BufferSize);
            Stream.Seek(-16, SeekOrigin.Current);
            Stream.Read(this.BytesBufferBackBuffer, 0, this.BufferSize);
            this._Index = 0;
        }
        private void SwitchBuffer()
        {
            if (_EOF) return;
            if (_MainEOF) _EOF = true;
            BytesBuffer = BytesBufferBackBuffer;
            if (!_MainEOF)
            {
                BytesBufferBackBuffer = new byte[BufferSize];
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                System.GC.Collect();
                if (BufferSize > (Stream.Length - Stream.Position + 16))
                {
                    Stream.Seek(-16, SeekOrigin.Current);
                    Stream.Read(this.BytesBufferBackBuffer, 0, (int)(Stream.Length - Stream.Position));
                    _MainEOF = true;
                }
                else
                {
                    Stream.Seek(-16, SeekOrigin.Current);
                    Stream.Read(this.BytesBufferBackBuffer, 0, BufferSize);
                }
            }
            _Index -= (BufferSize - 16);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            System.GC.Collect();
        }
        private void SetIndex(byte Value)
        {
            _Index = Value;
            while (!_EOF && _Index >= (BufferSize - 16)) SwitchBuffer();
        }
        private void SetIndex(int Value)
        {
            _Index = Value;
            while (!_EOF && _Index >= (BufferSize - 16)) SwitchBuffer();
        }
        private void IncreaseIndex(byte Value)
        {
            _Index += Value;
            while (!_EOF && _Index >= (BufferSize - 16)) SwitchBuffer();
        }
        private void IncreaseIndex(int Value)
        {
            _Index += Value;
            while (!_EOF && _Index >= (BufferSize - 16)) SwitchBuffer();
        }
        public override void JumpTo(int Index)
        {
            this.BytesBuffer = new byte[BufferSize];
            this.BytesBufferBackBuffer = new byte[BufferSize];
            Stream.Seek(Index, SeekOrigin.Begin);
            Stream.Read(this.BytesBuffer, 0, BufferSize);
            Stream.Seek(-16, SeekOrigin.Current);
            Stream.Read(this.BytesBufferBackBuffer, 0, BufferSize);
            this._Index = 0;
        }
        public override void JumpFor(int IndexIncrement)
        {
            while (IndexIncrement / 250 > 0)
            {
                IncreaseIndex(250);
                IndexIncrement -= 250;
            }
            IncreaseIndex((byte)IndexIncrement);
        }
        public override byte PeekByte()
        {
            return BytesBuffer[Index];
        }
        public override byte ReadByte()
        {
            byte b = BytesBuffer[Index];
            Index = Index + 1;
            return b;
        }
        public override byte[] ReadBytes(int Length)
        {
            byte[] ByteArray = new byte[Length];
            for (int i = 0; i < Length; i++) ByteArray[i] = BytesBuffer[Index + i];
            IncreaseIndex(Length);
            return ByteArray;
        }
        public override sbyte PeekSByte()
        {
            return (sbyte)BytesBuffer[Index];
        }
        public override sbyte ReadSByte()
        {
            IncreaseIndex(1);
            return (sbyte)BytesBuffer[Index - 1];
        }
        public override sbyte[] ReadSBytes(int Length)
        {
            sbyte[] ByteArray = new sbyte[Length];
            for (int i = 0; i < Length; i++) ByteArray[i] = (sbyte)BytesBuffer[Index + i];
            IncreaseIndex(Length);
            return ByteArray;
        }
        public override char PeekChar()
        {
            return (char)BytesBuffer[Index - 1];
        }
        public override char ReadChar()
        {
            IncreaseIndex(1);
            return (char)BytesBuffer[Index - 1];
        }
        public override char[] ReadChars(int Length)
        {
            char[] CharArray = new char[Length];
            for (int i = 0; i < Length; i++) CharArray[i] = (char)BytesBuffer[Index + i];
            IncreaseIndex(Length);
            return CharArray;
        }
        public override short ReadInt16()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_16();
            else return ReadLEInt_16();
        }
        public override ushort ReadUInt16()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (ushort)ReadBEInt_16();
            else return (ushort)ReadLEInt_16();
        }
        public override int ReadInt32()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_32();
            else return ReadLEInt_32();
        }
        public override uint ReadUInt32()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (uint)ReadBEInt_32();
            else return (uint)ReadLEInt_32();
        }
        public override long ReadInt64()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_64();
            else return ReadLEInt_64();
        }
        public override ulong ReadUInt64()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (ulong)ReadBEInt_64();
            else return (ulong)ReadLEInt_64();
        }
        public override float ReadSingle()
        {
            if (CurrentArchitecture == Architecture.IEEELittleEndian) return ReadIEEELESingle();
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadIEEEBESingle();
            else return ReadVAXPDPSingle();
        }
        public override double ReadDouble()
        {
            if (CurrentArchitecture == Architecture.IEEELittleEndian) return ReadIEEELEDouble();
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadIEEEBEDouble();
            else return Double.NaN;
        }
        public override unsafe short ReadLEInt_16()
        {
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                IncreaseIndex(2);
                if (Index % 2 == 0)
                {
                    return *((short*)PointerByte);
                }
                else
                {
                    return (short)((*PointerByte) | (*(PointerByte + 1) << 8));
                }
            }
        }
        public override unsafe short ReadBEInt_16()
        {
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                IncreaseIndex(2);
                return (short)((*PointerByte << 8) | (*(PointerByte + 1)));
            }
        }
        public override unsafe int ReadLEInt_32()
        {
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                IncreaseIndex(4);
                if (Index % 4 == 0)
                {
                    return *((int*)PointerByte);
                }
                else
                {
                    return (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                }
            }
        }
        public override unsafe int ReadBEInt_32()
        {
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                IncreaseIndex(4);
                return (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
            }
        }
        public override unsafe long ReadLEInt_64()
        {
            long Value;
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                if (Index % 8 == 0)
                {
                    Value = *((long*)PointerByte);
                }
                else
                {
                    int i1 = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                    int i2 = (*(PointerByte + 4)) | (*(PointerByte + 5) << 8) | (*(PointerByte + 6) << 16) | (*(PointerByte + 7) << 24);
                    Value = (uint)i1 | ((long)i2 << 32);
                }
            }
            IncreaseIndex(8);
            return Value;
        }
        public override unsafe long ReadBEInt_64()
        {
            long Value;
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                int i1 = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
                int i2 = (*(PointerByte + 4) << 24) | (*(PointerByte + 5) << 16) | (*(PointerByte + 6) << 8) | (*(PointerByte + 7));
                Value = (uint)i2 | ((long)i1 << 32);
                return Value;
            }
        }
        public override unsafe float ReadIEEELESingle()
        {
            int Value;
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                if (Index % 4 == 0)
                {
                    Value = *((int*)PointerByte);
                }
                else
                {
                    Value = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                }
            }
            IncreaseIndex(4);
            return *(float*)&Value;
        }
        public override unsafe float ReadIEEEBESingle()
        {
            int Value;
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                Value = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
            }
            IncreaseIndex(4);
            return *(float*)&Value;
        }
        public override unsafe float ReadVAXPDPSingle()
        {
            int Value = 0;
            Value = (BytesBuffer[Index + 1] - 1 * (BytesBuffer[Index + 1] == 0 ? 0 : 1)) << 24 | (BytesBuffer[Index] << 16) | (BytesBuffer[Index + 3] << 8) | BytesBuffer[Index + 2];
            IncreaseIndex(4);
            return *(float*)&Value;
        }
        public override unsafe double ReadIEEELEDouble()
        {
            long Value;
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                if (Index % 8 == 0)
                {
                    Value = *((long*)PointerByte);
                }
                else
                {
                    int i1 = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                    int i2 = (*(PointerByte + 4)) | (*(PointerByte + 5) << 8) | (*(PointerByte + 6) << 16) | (*(PointerByte + 7) << 24);
                    Value = (uint)i1 | ((long)i2 << 32);
                }
            }
            IncreaseIndex(8);
            return *(double*)&Value;
        }
        public override unsafe double ReadIEEEBEDouble()
        {
            long Value;
            fixed (byte* PointerByte = &BytesBuffer[Index])
            {
                int i1 = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
                int i2 = (*(PointerByte + 4) << 24) | (*(PointerByte + 5) << 16) | (*(PointerByte + 6) << 8) | (*(PointerByte + 7));
                Value = (uint)i2 | ((long)i1 << 32);
            }
            IncreaseIndex(8);
            return *(double*)&Value;
        }
        public override void Close()
        {
            this.BytesBuffer = null;
            this.BytesBufferBackBuffer = null;
            this.Stream.Close();
            this._Index = -1;
            System.GC.Collect();
        }
    }
    public class SwitchSourceStream
    {
        private int Index;
        private Architecture CurrentArchitecture;
        private byte[] Bytes;
        public bool EOF
        {
            get { return Index >= Bytes.Length; }
        }
        public SwitchSourceStream(byte[] Source)
        {
            this.Bytes = Source;
            this.Index = 0;
        }
        public void SetArchitecture(Architecture NewArchitecture)
        {
            this.CurrentArchitecture = NewArchitecture;
        }
        public void JumpTo(int Index)
        {
            this.Index = Index;
        }
        public void JumpFor(int IndexIncrement)
        {
            this.Index += IndexIncrement;
        }
        public byte PeekByte()
        {
            return Bytes[Index];
        }
        public byte ReadByte()
        {
            Index++;
            return Bytes[Index - 1];
        }
        public byte[] ReadBytes(int Length)
        {
            byte[] ByteArray = new byte[Length];
            for (int i = 0; i < Length; i++) ByteArray[i] = Bytes[Index + i];
            Index += Length;
            return ByteArray;
        }
        public sbyte PeekSByte()
        {
            return (sbyte)Bytes[Index];
        }
        public sbyte ReadSByte()
        {
            Index++;
            return (sbyte)Bytes[Index - 1];
        }
        public sbyte[] ReadSBytes(int Length)
        {
            sbyte[] ByteArray = new sbyte[Length];
            for (int i = 0; i < Length; i++) ByteArray[i] = (sbyte)Bytes[Index + i];
            Index += Length;
            return ByteArray;
        }
        public char PeekChar()
        {
            return (char)Bytes[Index - 1];
        }
        public char ReadChar()
        {
            Index++;
            return (char)Bytes[Index - 1];
        }
        public char[] ReadChars(int Length)
        {
            char[] CharArray = new char[Length];
            for (int i = 0; i < Length; i++) CharArray[i] = (char)Bytes[Index + i];
            Index += Length;
            return CharArray;
        }
        public short ReadInt16()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_16();
            else return ReadLEInt_16();
        }
        public ushort ReadUInt16()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (ushort)ReadBEInt_16();
            else return (ushort)ReadLEInt_16();
        }
        public int ReadInt32()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_32();
            else return ReadLEInt_32();
        }
        public uint ReadUInt32()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (uint)ReadBEInt_32();
            else return (uint)ReadLEInt_32();
        }
        public long ReadInt64()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadBEInt_64();
            else return ReadLEInt_64();
        }
        public ulong ReadUInt64()
        {
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return (ulong)ReadBEInt_64();
            else return (ulong)ReadLEInt_64();
        }
        public float ReadSingle()
        {
            if (CurrentArchitecture == Architecture.IEEELittleEndian) return ReadIEEELESingle();
            if (CurrentArchitecture == Architecture.IEEEBigEndian) return ReadIEEEBESingle();
            else return ReadVAXPDPSingle();
        }
        public double ReadDouble()
        {
            if (CurrentArchitecture == Architecture.IEEELittleEndian) return ReadIEEELEDouble();
            else return ReadIEEEBEDouble();
        }
        public unsafe short ReadLEInt_16()
        {
            fixed (byte* PointerByte = &Bytes[Index])
            {
                Index += 2;
                if (Index % 2 == 0)
                {
                    return *((short*)PointerByte);
                }
                else
                {
                    return (short)((*PointerByte) | (*(PointerByte + 1) << 8));
                }
            }
        }
        public unsafe short ReadBEInt_16()
        {
            fixed (byte* PointerByte = &Bytes[Index])
            {
                Index += 2;
                return (short)((*PointerByte << 8) | (*(PointerByte + 1)));
            }
        }
        public unsafe int ReadLEInt_32()
        {
            fixed (byte* PointerByte = &Bytes[Index])
            {
                Index += 4;
                if (Index % 4 == 0)
                {
                    return *((int*)PointerByte);
                }
                else
                {
                    return (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                }
            }
        }
        public unsafe int ReadBEInt_32()
        {
            fixed (byte* PointerByte = &Bytes[Index])
            {
                Index += 4;
                return (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
            }
        }
        public unsafe long ReadLEInt_64()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[Index])
            {
                if (Index % 8 == 0)
                {
                    Value = *((long*)PointerByte);
                }
                else
                {
                    int i1 = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                    int i2 = (*(PointerByte + 4)) | (*(PointerByte + 5) << 8) | (*(PointerByte + 6) << 16) | (*(PointerByte + 7) << 24);
                    Value = (uint)i1 | ((long)i2 << 32);
                }
            }
            Index += 8;
            return Value;
        }
        public unsafe long ReadBEInt_64()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[Index])
            {
                int i1 = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
                int i2 = (*(PointerByte + 4) << 24) | (*(PointerByte + 5) << 16) | (*(PointerByte + 6) << 8) | (*(PointerByte + 7));
                Value = (uint)i2 | ((long)i1 << 32);
                return Value;
            }
        }
        public unsafe float ReadIEEELESingle()
        {
            int Value;
            fixed (byte* PointerByte = &Bytes[Index])
            {
                if (Index % 4 == 0)
                {
                    Value = *((int*)PointerByte);
                }
                else
                {
                    Value = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                }
            }
            //Value = (Bytes[Index + 1] - 1 * (Bytes[Index + 1] == 0 ? 0 : 1)) << 24 | (Bytes[Index] << 16) | (Bytes[Index + 3] << 8) | Bytes[Index + 2];
            Index += 4;
            return *(float*)&Value;
        }
        public unsafe float ReadIEEEBESingle()
        {
            int Value;
            fixed (byte* PointerByte = &Bytes[Index])
            {
                Value = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
            }
            //Value = (Bytes[Index + 1] - 1 * (Bytes[Index + 1] == 0 ? 0 : 1)) << 24 | (Bytes[Index] << 16) | (Bytes[Index + 3] << 8) | Bytes[Index + 2];
            Index += 4;
            return *(float*)&Value;
        }
        public unsafe float ReadVAXPDPSingle()
        {
            int Value = 0;
            /*fixed (byte* PointerByte = &Bytes[Index])
            {
                Value = ((*(PointerByte + 1) - 1 * (*(PointerByte + 1) == 0 ? 0 : 1))<<24) | (*(PointerByte) << 16) | (*(PointerByte + 3) << 8) | (*(PointerByte + 2));
            }*/
            Value = (Bytes[Index + 1] - 1 * (Bytes[Index + 1] == 0 ? 0 : 1)) << 24 | (Bytes[Index] << 16) | (Bytes[Index + 3] << 8) | Bytes[Index + 2];
            Index += 4;
            return *(float*)&Value;
        }
        public unsafe double ReadIEEELEDouble()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[Index])
            {
                if (Index % 8 == 0)
                {
                    Value = *((long*)PointerByte);
                }
                else
                {
                    int i1 = (*PointerByte) | (*(PointerByte + 1) << 8) | (*(PointerByte + 2) << 16) | (*(PointerByte + 3) << 24);
                    int i2 = (*(PointerByte + 4)) | (*(PointerByte + 5) << 8) | (*(PointerByte + 6) << 16) | (*(PointerByte + 7) << 24);
                    Value = (uint)i1 | ((long)i2 << 32);
                }
            }
            Index += 8;
            return *(double*)&Value;
        }
        public unsafe double ReadIEEEBEDouble()
        {
            long Value;
            fixed (byte* PointerByte = &Bytes[Index])
            {
                int i1 = (*PointerByte << 24) | (*(PointerByte + 1) << 16) | (*(PointerByte + 2) << 8) | (*(PointerByte + 3));
                int i2 = (*(PointerByte + 4) << 24) | (*(PointerByte + 5) << 16) | (*(PointerByte + 6) << 8) | (*(PointerByte + 7));
                Value = (uint)i2 | ((long)i1 << 32);
            }
            Index += 8;
            return *(double*)&Value;
        }
        public void Close()
        {
            this.Bytes = null;
            this.Index = -1;
            System.GC.Collect();
        }
    }
    public static class IOMath
    {
        public static int LongestByDoubles(List<double[]> Items)
        {
            int Max = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Max < Items[i].Length) Max = Items[i].Length;
            }
            return Max;
        }
        public static int LongestBySingles(List<float[]> Items)
        {
            int Max = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Max < Items[i].Length) Max = Items[i].Length;
            }
            return Max;
        }
    }
    public static class Parser
    {
        public static string Parse(StreamReader Stream)
        {
            if (Stream.EndOfStream) return "";
            string NewString = "";
            while (true)
            {
                char NC;
                NC = Convert.ToChar(Stream.Read());
                if (NC == ' ' || NC == '\t' || NC == '\n' || NC == '\r') break;
                else NewString += NC;
            }
            return NewString;
        }
        public static string ForceParse(StreamReader Stream)
        {
            string Parsed = "";
            while (Parsed == "") Parsed = Parse(Stream);
            return Parsed;
        }
        public static float ParseSingle(StreamReader Stream)
        {
            if (Stream.EndOfStream) return Single.NaN;
            string NewString = "";
            while (true)
            {
                char NC;
                NC = Convert.ToChar(Stream.Read());
                if ((NewString == "" || NewString.Contains("\t")) && NC == '\t')
                {
                    return Single.NaN;
                }
                if (NC == ' ' || NC == '\t' || NC == '\n' || NC == '\r')
                {
                    return Converter.ToSingle(NewString);
                }
                else NewString += NC;
            }
        }
        public static double ParseDouble(StreamReader Stream)
        {
            if (Stream.EndOfStream) return Double.NaN;
            string NewString = "";
            while (true)
            {
                char NC;
                NC = Convert.ToChar(Stream.Read());
                if ((NewString == "" || NewString.Contains("\t")) && NC == '\t')
                {
                    return Double.NaN;
                }
                if (NC == ' ' || NC == '\t' || NC == '\n' || NC == '\r')
                {
                    return Converter.ToDouble(NewString);
                }
                else NewString += NC;
            }
        }   
        public static string ParseDoubleAsString(StreamReader Stream)
        {
            if (Stream.EndOfStream) return "";
            string NewString = "";
            while (true)
            {
                char NC;
                NC = Convert.ToChar(Stream.Read());
                if ((NewString == "" || NewString.Contains("\t")) && NC == '\t')
                {
                    return "";
                }
                if (NC == ' ' || NC == '\t' || NC == '\n' || NC == '\r')
                {
                    return NewString;
                }
                else NewString += NC;
            }
        }
        public static float[] ParseSingles(StreamReader Reader)
        {
            List<float> Values = new List<float>();
            string Current;
            while (true)
            {
                Current = ParseDoubleAsString(Reader);
                if (Current != "" && !Current.Contains(".")) break;
                if (Current == "") Values.Add(Single.NaN);
                else Values.Add(Single.Parse(Current));
                if (Reader.EndOfStream) break;
            }
            return Values.ToArray();
        }
        public static double[] ParseDoubles(StreamReader Reader)
        {
            List<double> Values = new List<double>();
            string Current;
            while (true)
            {
                Current = ParseDoubleAsString(Reader);
                if (Current != "" && !Current.Contains(".")) break;
                if (Current == "") Values.Add(Double.NaN);
                else Values.Add(Double.Parse(Current));
                if (Reader.EndOfStream) break;
            }
            return Values.ToArray();
        }
        public static double[] ParseArray(string[] Array)
        {
            double[] NewParsed = new double[Array.Length];
            for (int i = 0; i < Array.Length; i++)
            {
                if (!Double.TryParse(Array[i], out NewParsed[i])) NewParsed[i] = Double.NaN;
            }
            return NewParsed;
        }
        public static double[] ParseArray(string Input, char Splitter)
        {
            string[] InputSeparated = Input.Split(Splitter);
            for (int i = 0; i < InputSeparated.Length; i++) InputSeparated[i].Trim();
            return ParseArray(InputSeparated);
        }
        //Old
        private static string TryParse(ref string Stream)
        {
            if (Stream == "") return "";
            string NewString = "";
            while (true)
            {
                if (Stream == "") return NewString;
                char NC;
                NC = Convert.ToChar(Stream.Substring(0, 1));
                Stream = Stream.Remove(0, 1);
                if (NC == ' ' || NC == '\t' || NC == '\n' || NC == '\r') break;
                else NewString += NC;
            }
            return NewString;
        }
        public static int ReadInt(StreamReader Reader)
        {
            string Parsed = "";
            while (Parsed == "") Parsed = Parse(Reader);
            int Value;
            Int32.TryParse(Parsed, out Value);
            return Value;
        }
        public static float ReadSingle(StreamReader Reader)
        {
            string Parsed = "";
            while (Parsed == "") Parsed = Parse(Reader);
            return Converter.ToSingle(Parsed);
        }
        public static double ReadDouble(StreamReader Reader)
        {
            string Parsed = "";
            while (Parsed == "") Parsed = Parse(Reader);
            return Converter.ToDouble(Parsed);
        }
        public static string ReadString(ref string Stream)
        {
            string Parsed = "";
            while (Parsed == "")
            {
                if (Stream == "") return "0";
                Parsed = TryParse(ref Stream);
            }
            return Parsed;
        }
        public static List<string> ReadToEnd(ref string Stream)
        {
            List<string> Read = new List<string>();
            while (Stream != "") Read.Add(ReadString(ref Stream));
            return Read;
        }
    }
    public static class Converter
    {     
        public static float ToSingle(string Input)
        {
            if (Input.Length == 0) return Single.NaN;
            int Sign = 1;
            int DecPoint = Input.Length;
            long Value = 0;
            float dValue = 1;
            int i = 0;
            if (Input[0] == '-')
            {
                Sign = -1;
                i++;
            }
            for (; i < Input.Length; i++)
            {
                char C = Input[i];
                if (C == '.' || C == ',')
                    DecPoint = i + 1;
                else
                    Value = (Value * 10) + (int)(C - '0');
            }
            DecPoint = Input.Length - DecPoint;
            for (i = 0; i < DecPoint; i++) dValue /= 10;
            return Sign * Value * dValue;
        }
        public static float ToSingle_DEC(byte[] Read)
        {
            if (Read.Length == 0) return Single.NaN;
            int Sign = Read[1] & 0x80;
            int Exp = Read[1] & 0x7F;
            Exp <<= 1;
            int v = Read[0] >> 7;
            Exp += v;
            Exp -= 128;
            if ((Read[0] & 128) == 0) Read[0] += 128;
            int v0 = Read[0] << 16;
            int v1 = Read[3] << 8;
            int v2 = Read[2];
            float Fraction = v0 + v1 + v2;
            Fraction /= 0x1000000;
            if (Sign == 128) Sign = -1;
            else Sign = 1;
            float Result = ((Sign * Fraction * (float)Math.Pow(2, Exp)));
            return Result;
        }
        public static double ToDouble(string Input)
        {
            if (Input.Length == 0) return Single.NaN;
            int Sign = 1;
            int DecPoint = Input.Length;
            long Value = 0;
            double dValue = 1;
            int i = 0;
            if (Input[0] == '-')
            {
                Sign = -1;
                i++;
            }
            for (; i < Input.Length; i++)
            {
                char C = Input[i];
                if (C == '.' || C == ',')
                    DecPoint = i + 1;
                else
                    Value = (Value * 10) + (int)(C - '0');
            }
            DecPoint = Input.Length - DecPoint;
            for (i = 0; i < DecPoint; i++) dValue /= 10;
            return Sign * Value * dValue;
        }
        public static double ToRoundNumber(double num, int place)
        {
            double n;
            n = num * Math.Pow(10, place);
            n = Math.Sign(n) * Math.Abs(Math.Floor(n + .5));
            return n / Math.Pow(10, place);
        }
        public static bool IsIntConvertible(string Value)
        {
            bool Is = false;
            try
            {
                int IntValue = Convert.ToInt32(Value);
                Is = true;
            }
            catch { }
            return Is;
        }
        public static bool IsFloatConvertible(string Value)
        {
            bool Is = false;
            try
            {
                float IntValue = Convert.ToSingle(Value);
                Is = true;
            }
            catch { }
            return Is;
        }
        public static bool IsTimeCodeConvertible(string Value)
        {
            bool Is = false;
            try
            {
                int IntValue = Convert.ToInt32(Value.Substring(0, 2));
                IntValue = Convert.ToInt32(Value.Substring(3, 2));
                IntValue = Convert.ToInt32(Value.Substring(6, 2));
                IntValue = Convert.ToInt32(Value.Substring(9, 2));
                Is = true;
            }
            catch { }
            return Is;
        }
    }
}
