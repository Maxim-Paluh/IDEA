using System;

using System.Text;


namespace IdeaClass
{
    public class Data
    {
        public static UInt16[] GetData(byte[] DataByte)
        {
            var uint16Array = new ushort[4];
            var bytearray = DataByte;

            if (BitConverter.IsLittleEndian)
            Array.Reverse(bytearray);

            for (int i = 0; i < 4; i++)
            {
                uint16Array[i] = BitConverter.ToUInt16(bytearray, i*2);
            }
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(uint16Array);

            return uint16Array;
        }
    }
}
