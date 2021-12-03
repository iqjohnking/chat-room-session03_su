using System;
using System.Text;

namespace ChatCoreTest
{
    internal class Program
    {
        private static byte[] m_PacketData;//unPack this, Add Length to package
        private static uint m_Pos;//it maens Length
        private static byte[] l_PacketData;//this package store data
        //use the same method as  20_曾鐁賢
        public static void Main()
        {
            m_PacketData = new byte[1024];
            l_PacketData = new byte[1024];
            m_Pos = 0;

            Write(109); //write int as byte in l_PacketData
            Write(109.99f); //write float as byte in l_PacketData
            Write("哈囉你好嗎!"); //write string as unicode as byte in l_PacketData
            byte[] length = Getlength(m_Pos); //this package store length



            int l = 0;
            while (l < m_Pos + 4)
            {
                if (l < 4)
                {
                    m_PacketData[l] = length[l]; //put length into m_PacketData
                }
                else
                {
                    m_PacketData[l] = l_PacketData[l - 4];//put data into m_PacketData
                }
                l++;
            }

            Console.WriteLine($"Output Byte array(length:{m_Pos}): ");
            for (var i = 0; i < m_Pos + 4; i++)
            {
                Console.Write(m_PacketData[i] + ", ");
            }

            byte[] lengthread = new byte[4];
            byte[] intread = new byte[4];
            byte[] floatread = new byte[4];
            byte[] stringread = new byte[12];

            int j = 0;
            while (j < m_Pos + 4)
            {
                if (j < 4)//0 1 2 3
                {
                    lengthread[j] = m_PacketData[j];
                }
                else if (j >= 4 && j < 8)//4 5 6 7
                {
                    intread[j - 4] = m_PacketData[j];
                }
                else if (j >= 8 && j < 12)//8 9 10 11
                {
                    floatread[j - 8] = m_PacketData[j];
                }
                else if (j >= 16)//12 13 14 15//16........
                {
                    stringread[j - 16] = m_PacketData[j];
                }
                j++;
            }

            Console.WriteLine();
            Console.WriteLine($"Output what we read:");
            Console.WriteLine(ReadLenght(lengthread));
            Console.WriteLine(ReadInt(intread));
            Console.WriteLine(ReadFloat(floatread));
            Console.WriteLine(ReadString(stringread));
            Console.ReadLine();
        }

        // write an integer into a byte array
        private static bool Write(int i)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i);
            Write2(bytes);
            return true;
        }

        // write a float into a byte array
        private static bool Write(float f)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f);
            Write2(bytes);
            return true;
        }

        // write a string into a byte array
        private static bool Write(string s)
        {
            // convert string to byte array
            var bytes = Encoding.Unicode.GetBytes(s);

            // write byte array length to packet's byte array
            if (Write(bytes.Length) == false)
            {
                return false;
            }

            Write2(bytes);
            return true;
        }

        // write a byte array into packet's byte array
        private static void Write2(byte[] byteData)
        {
            // converter little-endian to network's big-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }
            // Write2:first write into l_P, and let m_P = l_P.
            byteData.CopyTo(l_PacketData, m_Pos);
            m_Pos += (uint)byteData.Length;
        }

        private static byte[] Getlength(uint length)
        {
            byte[] length2Bytes = BitConverter.GetBytes(length);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(length2Bytes);
            }

            return length2Bytes;
        }

        private static uint ReadLenght(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            uint byte2length = BitConverter.ToUInt32(byteData, 0);
            return byte2length;
        }

        private static int ReadInt(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            var byte2int = BitConverter.ToInt32(byteData, 0);
            return byte2int;
        }

        private static float ReadFloat(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            var byte2float = BitConverter.ToSingle(byteData, 0);
            return byte2float;
        }

        private static string ReadString(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            var byte2string = Encoding.Unicode.GetString(byteData);
            return byte2string;
        }

    }

}
