using System;
using System.Collections;
using System.IO;
using System.Text;


namespace IdeaClass
{
    public class Key
    {
        private string KeyString { get; set; }
        public UInt16[] EncryptionKey { get; set; }
        public UInt16[] DecryptionKey { get; set; }
        public Key(string key)
        {
            if (Encoding.UTF8.GetBytes(key).Length!=16)
            {
                throw new ArgumentException("length");
            }
            KeyString = key;
            GetEncryptionKey();
            GetDecryptionKey();
        }

        private void GetEncryptionKey()
        {
            EncryptionKey = new ushort[56]; 
            var bytearray = Encoding.UTF8.GetBytes(KeyString);
            //bytearray[0] = 0;
            //bytearray[1] = 1;
            //bytearray[2] = 0;
            //bytearray[3] = 2;
            //bytearray[4] = 0;
            //bytearray[5] = 3;
            //bytearray[6] = 0;
            //bytearray[7] = 4;
            //bytearray[8] = 0;
            //bytearray[9] = 5;
            //bytearray[10] = 0;
            //bytearray[11] = 6;
            //bytearray[12] = 0;
            //bytearray[13] = 7;
            //bytearray[14] = 0;
            //bytearray[15] = 8;
            var bitarray = new BitArray(bytearray);
            for (int i = 0; i < 7; i++)
            {
                byte[] tempbyte = BitArrayToByte(bitarray);

                if (BitConverter.IsLittleEndian)
                   tempbyte = ReversByte(tempbyte);
                
                for (int j = 0; j < 8; j++)
                {
                    EncryptionKey[(8 * i) + j] = BitConverter.ToUInt16(tempbyte, j * 2);
                }
                ReversBitArraty(bitarray);
                ShiftLeft(bitarray);
                ReversBitArraty(bitarray);
            }

        }

        private void GetDecryptionKey()
        {
            DecryptionKey = new ushort[52];
            for (int i = 8; i >= 0; i--)
            {
                if (i==8)
                {
                    DecryptionKey[48-(i*6)] = MultiplicativeInverse(EncryptionKey[i*6]);
                    DecryptionKey[48 - (i * 6)+1] = AdditiveInverse(EncryptionKey[(i * 6)+1]);
                    DecryptionKey[48 - (i * 6) + 2] = AdditiveInverse(EncryptionKey[(i * 6) + 2]);
                    DecryptionKey[48 - (i*6) + 3] = MultiplicativeInverse(EncryptionKey[((i * 6)+3)]);

                    DecryptionKey[48 - (i*6) + 4] = EncryptionKey[((i - 1)*6) + 4];
                    DecryptionKey[48 - (i * 6) + 5] = EncryptionKey[((i - 1) * 6) + 5];
                }
                else if (i==0)
                {
                    DecryptionKey[48 - (i * 6)] = MultiplicativeInverse(EncryptionKey[i * 6]);
                    DecryptionKey[48 - (i * 6) + 1] = AdditiveInverse(EncryptionKey[(i * 6) + 1]);
                    DecryptionKey[48 - (i * 6) + 2] = AdditiveInverse(EncryptionKey[(i * 6) + 2]);
                    DecryptionKey[48 - (i * 6) + 3] = MultiplicativeInverse(EncryptionKey[((i * 6) + 3)]);
                }
                else
                {
                    DecryptionKey[48 - (i * 6)] = MultiplicativeInverse(EncryptionKey[i * 6]);
                    DecryptionKey[48 - (i * 6) + 1] = AdditiveInverse(EncryptionKey[(i * 6) + 2]);
                    DecryptionKey[48 - (i * 6) + 2] = AdditiveInverse(EncryptionKey[(i * 6) + 1]);
                    DecryptionKey[48 - (i * 6) + 3] = MultiplicativeInverse(EncryptionKey[(i * 6) + 3]);

                    DecryptionKey[48 - (i * 6) + 4] = EncryptionKey[((i - 1) * 6) + 4];
                    DecryptionKey[48 - (i * 6) + 5] = EncryptionKey[((i - 1) * 6) + 5];
                }

                
            }
           
        }

        void ShiftLeft(BitArray bits)
        {
            for (int j = 0; j < 25; j++)
            {
                bool temp1 = bits[0];
                for (int k = 1; k < bits.Count; k++)
                {
                    bits.Set(k - 1, bits[k]);
                }
                bits.Set(127, temp1);
            }
        }

        void ReversBitArraty(BitArray bits)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bool temp1 = bits[(((i+1)*8)-1)-j];
                    bool temp2 = bits[(i*8)+j];
                    bits.Set((i * 8) + j, temp1);
                    bits.Set((((i + 1) * 8) - 1) - j, temp2);
                }
            }
        }

        byte[] BitArrayToByte(BitArray bits)
        {
            if (bits.Count != 128)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[16];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        byte[] ReversByte(byte[] bytes)
        {
            for (int i = 0; i < 16; i=i+2)
            {
                var tempbyte = bytes[i];
                bytes[i] = bytes[i + 1];
                bytes[i + 1] = tempbyte;
            }
            return bytes;
        }


        UInt16 AdditiveInverse(UInt16 key)
        {
            return (ushort)(65536 - key);
        }

        UInt16 MultiplicativeInverse(UInt16 key)
        {
            Euclid.ExtendedEuclideanResult temp = Euclid.ExtendedEuclide(key, 65537);
            if (temp.X>0)
            {
                return (ushort) (temp.X%65537);
            }
            else
            {
                return (ushort) (65537 + temp.X);
            }

        }
        
       
    }
}
