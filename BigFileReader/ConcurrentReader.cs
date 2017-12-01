using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Numerics;

namespace BigFileReader
{
    class ConcurrentReader
    {
        long length;
        String path;
        int threadCount;
        BigInteger sum;
        uint min;
        uint max;

        public ConcurrentReader(String path, int threadNumber, long length)
        {
            this.length = length;
            this.path = path;
            threadCount = threadNumber;
            sum = BigInteger.Zero;
            min = UInt32.MaxValue;
            max = UInt32.MinValue;
        }

        public void readFile()
        {
            ManualResetEvent[] doneEvents = new ManualResetEvent[threadCount];
            MemoryMappedViewAccessor[] viewAccessors = new MemoryMappedViewAccessor[threadCount];
            ConcurrentTast[] crArr = new ConcurrentTast[threadCount];

            using (var mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open, "BigFile"))
            {
                long offset = 0L;

                for (int i = 0; i < threadCount; i++)
                {
                    viewAccessors[i] = mmf.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read);
                    doneEvents[i] = new ManualResetEvent(false);
                    crArr[i] = new ConcurrentTast(viewAccessors[i], doneEvents[i], length);
                    ThreadPool.QueueUserWorkItem(crArr[i].ThreadPoolCallback, viewAccessors[i]);
                    offset += length;
                }

            }

            // waits
            WaitHandle.WaitAll(doneEvents);

            for (int i = 0; i < threadCount; i++)
            {
                viewAccessors[i].Dispose();
            }

            for (int i = 0; i < threadCount; i++)
            {
                sum = BigInteger.Add(sum, crArr[i].getSum());
                uint minOfTask = crArr[i].getMin();
                uint maxOfTask = crArr[i].getMax();

                if (minOfTask < min)
                {
                    min = minOfTask;
                }

                if (maxOfTask > max)
                {
                    max = maxOfTask;
                }

            }
        }

        public BigInteger getSum()
        {
            return sum;
        }

        public uint getMax()
        {
            return max;
        }

        public uint getMin()
        {
            return min;
        }
    }
}
