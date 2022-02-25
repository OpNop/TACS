using System;
using System.IO;
using System.Text;

namespace TinyTools.TinySocket
{
    public class Packer
    {
        private readonly ASCIIEncoding mASCIIEncoding;
        private readonly BinaryWriter mBinaryWriter;
        private readonly MemoryStream mBuffer = new MemoryStream();
        private readonly UnicodeEncoding mUnicodeEncoding;

        public Packer(byte packetType)
        {
            mBinaryWriter = new BinaryWriter(mBuffer);
            mBinaryWriter.Write(packetType);
            mASCIIEncoding = new ASCIIEncoding();
            mUnicodeEncoding = new UnicodeEncoding();
        }

        public void Add(bool val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(sbyte val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(byte val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(short val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(ushort val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(int val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(uint val)
        {
            mBinaryWriter.Write(val);
        }

        public void Add(byte[] val)
        {
            mBinaryWriter.Write(val);
        }

        public void AddShortString(string val)
        {
            if (val == null)
            {
                Add(0);
                return;
            }
            byte[] bytes = mUnicodeEncoding.GetBytes(val);
            if (bytes.Length > 255)
            {
                throw new TooLongStringException();
            }
            Add((byte)val.Length);
            Add(bytes);
        }

        public void AddString(string val)
        {
            if (val == null)
            {
                Add(0);
                return;
            }
            byte[] bytes = mUnicodeEncoding.GetBytes(val);
            if (bytes.Length > 65535)
            {
                throw new TooLongStringException();
            }
            Add((ushort)val.Length);
            Add(bytes);
        }

        public void AddDateTime(DateTime val)
        {
            if (val == DateTime.MinValue)
            {
                mBinaryWriter.Write(0L);
            }
            else
            {
                mBinaryWriter.Write(val.ToFileTime());
            }
        }

        public byte[] ToArray()
        {
            mBinaryWriter.Close();
            mBuffer.Close();
            return mBuffer.ToArray();
        }
    }
}
