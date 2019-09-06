using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEA
{
    class Helper
    {
        public static int[] GetLengs(string sLocalFile)
        {
            int[] temp = new int[2];
            try
            {
                using (FileStream oFS = new FileStream(sLocalFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader oBR = new BinaryReader(oFS))
                    {
                        temp[0] = (int)oFS.Length;
                        if (temp[0] % 8 != 0)
                        {
                            for (int i = 1; i < 8; i++)
                            {
                                temp[0]++;
                                temp[1]++;
                                if (temp[0] % 8 == 0)
                                {
                                    break;
                                }
                            }
                        }
                        return temp;
                    }
                }
            }
            catch
            {
                throw new System.ArgumentException("Невірна адреса файла для шифрування");
            }
        }
        public static byte[] ReadLocalFile(string sLocalFile)
        {
            try
            {
                using (FileStream oFS = new FileStream(sLocalFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader oBR = new BinaryReader(oFS))
                    {
                        return oBR.ReadBytes((int)oFS.Length);
                    }
                }
            }
            catch
            {
                throw new System.ArgumentException("Невірна адреса файла для шифрування");
            }
        }

        public static void retFile(byte[] file, string path)
        {
            try
            {
                using (FileStream oFS = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    oFS.Write(file, 0, file.Length);
                }
            }
            catch (Exception)
            {
                throw new System.ArgumentException("Невірна адреса файла для збереження");
            }
        }

        public static int GetLengsText(string text)
        {
            int temp = 0;
            var mybyte = Encoding.UTF8.GetBytes(text);
            temp = (int)mybyte.Length;
            if (temp % 8 != 0)
            {
                for (int i = 1; i < 8; i++)
                {
                    temp++;
                    if (temp % 8 == 0)
                    {
                        break;
                    }
                }
            }
            return temp;
        }
    }
}
