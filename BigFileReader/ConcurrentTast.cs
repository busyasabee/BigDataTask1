using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Numerics;

namespace BigFileReader
{
    class ConcurrentTast
    {
        private uint min;
        private uint max;
        private BigInteger sum;
        private ManualResetEvent doneEvent;
        private MemoryMappedViewAccessor accessor;
        private long startPos;
        private long endPos;
        private long length;

        public long StartPos
        {
            set
            {
                startPos = value;
            }
        }

        public long EndPos
        {
            set
            {
                endPos = value;
            }
        }

        public ConcurrentTast(MemoryMappedViewAccessor accessor, ManualResetEvent doneEvent, long length)
        {
            min = UInt32.MaxValue;
            max = UInt32.MinValue;
            sum = BigInteger.Zero;
            this.accessor = accessor;
            this.doneEvent = doneEvent;
            this.length = length;

        }

        public void ThreadPoolCallback(Object threadContext)
        {
            computeMin();
            computeMax();
            computeSum();
            doneEvent.Set();
        }

        private void computeMin()
        {
            for (long i = 0; i < length; i += 4)
            {
                uint value = accessor.ReadUInt32(i);

                if (value < min)
                {
                    min = value;
                }
            }
        }

        public void computeSum()
        {
            for (long i = 0; i < length; i += 4)
            {
                uint value = accessor.ReadUInt32(i);
                sum = BigInteger.Add(sum, value);
            } 
        }

        private void computeMax()
        {
            for (long i = 0; i < length; i += 4)
            {
                uint value = accessor.ReadUInt32(i);

                if (value > max)
                {
                    max = value;
                }
            }
        }

        public uint getMax()
        {
            return max;
        }

        public uint getMin()
        {
            return min;
        }

        public BigInteger getSum()
        {
            return sum;
        }

    }
}