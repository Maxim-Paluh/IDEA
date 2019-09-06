
using System;
using System.IO;
using System.Text;

namespace IdeaClass
{
    public class Idea
    {
        public Key Keyclass { get; set; }
        public UInt16[] Data { get; set; }
        public Idea(string key)
        {
            Keyclass = new Key(key);
        }
        public void SetData(byte[] data)
        {
            if (data.Length != 8)
            {
                throw new ArgumentException("length");
            }
            Data = IdeaClass.Data.GetData(data);
        }
        public byte[] RetBin()
        {
            byte[] ret = new byte[8];
            for (int i = 0; i < 4; i++)
            {
                byte[] f = BitConverter.GetBytes(Data[i]);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(f); 
                Array.Copy(f,0,ret,i*2,2);
            }
            return ret;

        }
        public void Encryption()
        {
            for (int i = 0; i < 9; i++)
            {
                if (i != 8)
                {
                    var tempA = And(Data[0], Keyclass.EncryptionKey[i * 6]);
                    var tempB = (ushort)(((Data[1] + Keyclass.EncryptionKey[(i * 6) + 1]) % 65536));
                    var tempC = (ushort)(((Data[2] + Keyclass.EncryptionKey[(i * 6) + 2]) % 65536));
                    var tempD = And(Data[3], Keyclass.EncryptionKey[(i * 6) + 3]);

                    var tempE = (ushort)(tempA ^ tempC);
                    var tempF = (ushort)(tempB ^ tempD);


                    var t1 = And(tempE, Keyclass.EncryptionKey[(i * 6) + 4]);
                    var t2 = (ushort)((t1 + tempF) % 65536);
                    var t3 = And(t2, Keyclass.EncryptionKey[(i * 6) + 5]);
                    var t4 = And(tempE, Keyclass.EncryptionKey[(i * 6) + 4]);
                    var t5 = (ushort)((t4 + t3) % 65536);
                    Data[0] = (ushort)(tempA ^ t3);
                    Data[1] = (ushort)(tempC ^ t3);
                    Data[2] = (ushort)(tempB ^ t5);
                    Data[3] = (ushort)(tempD ^ t5);
                }
                else
                {
                    UInt16 temp = Data[1];
                    Data[0] = (ushort)And(Data[0], Keyclass.EncryptionKey[i * 6]);
                    Data[1] = (ushort)(((Data[2] + Keyclass.EncryptionKey[(i * 6) + 1]) % 65536));
                    Data[2] = (ushort)(((temp + Keyclass.EncryptionKey[(i * 6) + 2]) % 65536));
                    Data[3] = (ushort)And(Data[3], Keyclass.EncryptionKey[(i * 6) + 3]);
                   
                }

            }
           
        
        }
        public void Decryption()
        {
            for (int i = 0; i < 9; i++)
            {
                if (i != 8)
                {
                    var tempA = And(Data[0], Keyclass.DecryptionKey[i * 6]);
                    var tempB = (ushort)(((Data[1] + Keyclass.DecryptionKey[(i * 6) + 1]) % 65536));
                    var tempC = (ushort)(((Data[2] + Keyclass.DecryptionKey[(i * 6) + 2]) % 65536));
                    var tempD = And(Data[3], Keyclass.DecryptionKey[(i * 6) + 3]);

                    var tempE = (ushort)(tempA ^ tempC);
                    var tempF = (ushort)(tempB ^ tempD);


                    var t1 = And(tempE, Keyclass.DecryptionKey[(i * 6) + 4]);
                    var t2 = (ushort)((t1 + tempF) % 65536);
                    var t3 = And(t2, Keyclass.DecryptionKey[(i * 6) + 5]);
                    var t4 = And(tempE, Keyclass.DecryptionKey[(i * 6) + 4]);
                    var t5 = (ushort)((t4 + t3) % 65536);


                    Data[0] = (ushort)(tempA ^ t3);
                    Data[1] = (ushort)(tempC ^ t3);
                    Data[2] = (ushort)(tempB ^ t5);
                    Data[3] = (ushort)(tempD ^ t5);

                   
                }
                else
                {
                    UInt16 temp = Data[1];
                    Data[0] = (ushort)And(Data[0], Keyclass.DecryptionKey[i * 6]);
                    Data[1] = (ushort)(((Data[2] + Keyclass.DecryptionKey[(i * 6) + 1]) % 65536));
                    Data[2] = (ushort)(((temp + Keyclass.DecryptionKey[(i * 6) + 2]) % 65536));
                    Data[3] = (ushort)And(Data[3], Keyclass.DecryptionKey[(i * 6) + 3]);
                    
                }

            }

        }
        private UInt32 And(UInt16 data, UInt16 key)
        {
            UInt32 t1, t2;
            if (key==0)
            {
                t2 = (UInt32)(Math.Pow(2, 16));
            }
            else
            {
                t2 = key;
            }
            if (data == 0)
            {
                t1 = (UInt32)(Math.Pow(2, 16));
            }
            else
            {
                t1 = data;
            }
            return  (ushort)((t1*t2)%65537);
        }
    }
}
