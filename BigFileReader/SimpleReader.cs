using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace BigFileReader
{
    class SimpleReader
    {
        BigInteger sum;
        uint min;
        uint max;
        String path;

        public SimpleReader(String path)
        {
            sum = BigInteger.Zero;
            min = UInt32.MaxValue;
            max = 0;
            this.path = path;
        }

        public void readFile(int numbersCount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                for (int i = 0; i < numbersCount; i++)
                {
                    uint value = reader.ReadUInt32();
                    sum = BigInteger.Add(sum, value);
                    if (value > max)
                    {
                        max = value;
                    }
                    if (value < min)
                    {
                        min = value;
                    }
                }
            }

            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine();
            Console.WriteLine("Time of sequential reading=" + time);
        }

        public String getSum()
        {
            return sum.ToString();
        }

        public uint getMin()
        {
            return min;
        }

        public uint getMax()
        {
            return max;
        }
    }
}
