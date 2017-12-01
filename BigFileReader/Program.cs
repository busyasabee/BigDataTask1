using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Threading;
using System.Diagnostics;

namespace BigFileReader
{
    class Program
    {

        static void Main(string[] args)
        {

            long fileSize = 2 * 1024 * 1024 * 1024L; // size in bytes
            int numbersCount = (int)(fileSize / 4);
            String path;
            int threadNum = 4;
            long length = fileSize / threadNum;
            BigInteger sum = BigInteger.Zero;
            uint min;
            uint max;

            Console.Write("Enter file directory path (example: C:\\Files):");
            String enteredPath = Console.ReadLine();
            Console.WriteLine(enteredPath);
            Console.Write("Enter file name (example: bigFile):");
            String enteredFileName = Console.ReadLine();
            Console.WriteLine(enteredFileName);
            path = enteredPath + "\\" + enteredFileName;

            try
            {
                // Sequential reading
                SimpleReader sr = new SimpleReader(path);
                sr.readFile(numbersCount);
                Console.WriteLine("Sequential reading");
                Console.WriteLine("Max=" + sr.getMax());
                Console.WriteLine("Min=" + sr.getMin());
                Console.WriteLine("Sum=" + sr.getSum());

                // Concurrent reading
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ConcurrentReader cr = new ConcurrentReader(path, threadNum, length);
                cr.readFile();
                sw.Stop();
                long time = sw.ElapsedMilliseconds;
                sum = cr.getSum();
                min = cr.getMin();
                max = cr.getMax();

                Console.WriteLine();
                Console.WriteLine("Time of concurrent reading=" + time);
                Console.WriteLine("Max=" + max);
                Console.WriteLine("Min=" + min);
                Console.WriteLine("Sum=" + sum.ToString());
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Not found file " + enteredFileName);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Not found directory " + enteredPath);
                
            }

            Console.WriteLine("Press any button to exit");
            Console.ReadKey();
        }
    }
}
