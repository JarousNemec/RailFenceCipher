using System;

namespace RailFenceCipher
{
    public class OrderedPair
    {
        public int[] Intervals { get; set; }

        public int ActualIntervalIndex { get; set; }

        public OrderedPair()
        {
            ActualIntervalIndex = 0;
        }

        public int GetOrderedPair()
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
            ActualIntervalIndex = 0;
            if (intervals[0] < 0)
            {
                intervals[0] = 0;
            }

            if (intervals[1] < 0)
            {
                intervals[1] = 0;
            }

            Intervals = intervals;
        }

        public override string ToString()
        {
            return $"{Intervals[0]} {Intervals[1]}";
        }

        public int GetSpace(int key, int index, int order)
        {
            int x = 2 * key - 2;
            if (index == 0 || index == key - 1)
            {
                if (order == 0)
                {
                    return x;
                }

                return 0;
            }

            if (order == 0)
            {
                return x - index * 2;
            }

            return index * 2;
        }
    }
}