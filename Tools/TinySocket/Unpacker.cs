using System;
using System.IO;
using System.Text;

namespace TinyTools.TinySocket
{
    public class Unpacker
    {
        private readonly ASCIIEncoding mASCIIEncoding;
        private readonly BinaryReader mBinaryReader;
        private readonly MemoryStream mBuffer;
        private readonly UnicodeEncoding mUnicodeEncoding;

        public Unpacker(byte[] b)
        {
            mBuffer = new MemoryStream(b);
            mBinaryReader = new BinaryReader(mBuffer);
            mASCIIEncoding = new ASCIIEncoding();
            mUnicodeEncoding = new UnicodeEncoding();
        }

        private static int ASCIILengthWithNull(byte[] data)
        {
            int result = data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    return i;
                }
            }
            return result;
        }

        public string GetASCIIString()
        {
            ushort num = mBinaryReader.ReadUInt16();
            if (num == 0)
            {
                return "";
            }
            byte[] bytes = mBinaryReader.ReadBytes(num);
            return mASCIIEncoding.GetString(bytes);
        }

        public string GetASCIIString(int length)
        {
            if (length == 0)
            {
                return "";
            }
            byte[] array = mBinaryReader.ReadBytes(length);
            return mASCIIEncoding.GetString(array, 0, ASCIILengthWithNull(array));
        }

        public string GetASCIIStringMax(int maxLength)
        {
            string aSCIIString = GetASCIIString();
            if (aSCIIString.Length > maxLength)
            {
                throw new TooLongStringException();
            }
            return aSCIIString;
        }

        public byte[] GetBytes(int count)
        {
            return mBinaryReader.ReadBytes(count);
        }

        public DateTime GetDateTime()
        {
            long num = mBinaryReader.ReadInt64();
            if (num == 0)
            {
                return DateTime.MinValue;
            }
            return DateTime.FromFileTime(num);
        }

        public short GetInt16()
        {
            return mBinaryReader.ReadInt16();
        }

        public int GetInt32()
        {
            return mBinaryReader.ReadInt32();
        }

        public sbyte GetInt8()
        {
            return mBinaryReader.ReadSByte();
        }

        public bool GetBool()
        {
            return mBinaryReader.ReadBoolean();
        }

        public string GetShortASCIIString()
        {
            byte b = mBinaryReader.ReadByte();
            if (b == 0)
            {
                return "";
            }
            byte[] bytes = mBinaryReader.ReadBytes(b);
            return mASCIIEncoding.GetString(bytes);
        }

        public string GetShortASCIIStringMax(int maxLength)
        {
            string shortASCIIString = GetShortASCIIString();
            if (shortASCIIString.Length > maxLength)
            {
                throw new TooLongStringException();
            }
            return shortASCIIString;
        }

        public string GetShortString()
        {
            byte b = mBinaryReader.ReadByte();
            if (b == 0)
            {
                return "";
            }
            byte[] bytes = mBinaryReader.ReadBytes(b * 2);
            return mUnicodeEncoding.GetString(bytes);
        }

        public string GetShortStringMax(int maxLength)
        {
            string shortString = GetShortString();
            if (shortString.Length > maxLength)
            {
                throw new TooLongStringException();
            }
            return shortString;
        }

        public string GetString()
        {
            ushort num = mBinaryReader.ReadUInt16();
            if (num == 0)
            {
                return "";
            }
            byte[] bytes = mBinaryReader.ReadBytes(num * 2);
            return mUnicodeEncoding.GetString(bytes);
        }

        public string GetString(int length)
        {
            if (length == 0)
            {
                return "";
            }
            byte[] array = mBinaryReader.ReadBytes(length * 2);
            return mUnicodeEncoding.GetString(array, 0, LengthWithNull(array) * 2);
        }

        public string GetStringMax(int maxLength)
        {
            string @string = GetString();
            if (@string.Length > maxLength)
            {
                throw new TooLongStringException();
            }
            return @string;
        }

        public ushort GetUInt16()
        {
            return mBinaryReader.ReadUInt16();
        }

        public uint GetUInt32()
        {
            return mBinaryReader.ReadUInt32();
        }

        public byte GetUInt8()
        {
            return mBinaryReader.ReadByte();
        }

        private static int LengthWithNull(byte[] data)
        {
            int result = data.Length;
            for (int i = 0; i < data.Length; i += 2)
            {
                if (data[i] == 0 && data[i + 1] == 0)
                {
                    return i / 2;
                }
            }
            return result;
        }
    }

}
