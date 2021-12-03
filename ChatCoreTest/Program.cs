using System;
using System.Text;

namespace ChatCoreTest
{
    internal class Program
    {
        private static byte[] m_PacketData;//unPacket this, Add Length to package
        private static uint m_Pos;//it maens Length

        public static void Main()
        {
            m_PacketData = new byte[1024];
            m_Pos = 0;

            Write(109);
            Write(109.99f);
            Write("Hello!");

            Console.Write($"Output Byte array(length:{m_Pos}): ");
            for (var i = 0; i < m_Pos; i++)
            {
                Console.Write(m_PacketData[i] + ", ");
            }

            byte[] intread = new byte[4];
            byte[] floatread = new byte[4];
            byte[] stringread = new byte[12];

            int j = 0;
            while (j < m_Pos)
            {
                if (j < 4)//0123
                {
                    intread[j] = m_PacketData[j];
                }
                else if (j >= 4 && j < 8)//4567//8 9 10 11
                {
                    floatread[j - 4] = m_PacketData[j];
                }
                else if (j >= 12)//1213141516........
                {
                    stringread[j - 12] = m_PacketData[j];
                }
                j++;
            }

            Console.WriteLine();
            Console.WriteLine($"Output what we read:");
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

            byteData.CopyTo(m_PacketData, m_Pos);
            m_Pos += (uint)byteData.Length;
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
