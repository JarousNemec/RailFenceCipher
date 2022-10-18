using System;

namespace RailFenceCipher
{
    public class Interval
    {
        public int[] Intervals { get; set; }

        public int ActualIntervalIndex { get; set; }

        public Interval()
        {
            ActualIntervalIndex = 0;
        }

        public int GetInterval()
        {
            if (ActualIntervalIndex < Intervals.Length)
            {
                return Intervals[ActualIntervalIndex++];
            }

            ActualIntervalIndex = 0;
            return Intervals[ActualIntervalIndex++];
        }

        public void SetInterval(int[] intervals)
        {
            if (intervals[0]<0)
            {
                intervals[0] = 0;
            }
            if (intervals[1]<0)
            {
                intervals[1] = 0;
            }
            Intervals = intervals;
        }

        public override string ToString()
        {
            // foreach (var interval in Intervals)
            // {
            //     Console.WriteLine(interval);
            // }
            //
            // Console.WriteLine("---------------");
            return $"{Intervals[0]} {Intervals[1]}";
        }

        public int GetIntervalOnLine(int line)
        {
            int interval = -1;

            for (int i = 0; i < line; i++)
            {
                interval += 2;
            }

            return interval;
        }
    }
}