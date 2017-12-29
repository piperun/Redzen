﻿/* ***************************************************************************
 * This file is part of the Redzen code library.
 * 
 * Copyright 2015-2017 Colin Green (colin.green1@gmail.com)
 *
 * Redzen is free software; you can redistribute it and/or modify
 * it under the terms of The MIT License (MIT).
 *
 * You should have received a copy of the MIT License
 * along with Redzen; if not, see https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using Redzen.Random;

namespace Redzen.Sorting
{
    /// <summary>
    /// Helper methods related to sorting.
    /// </summary>
    public static class SortUtils
    {
        #region Public Static Methods

        /// <summary>
        /// Indicates if a list of doubles is sorted into ascending order.
        /// </summary>
        public static bool IsSorted<T>(IList<T> valueList)
            where T : IComparable<T>
        {
            if (0 == valueList.Count) {
                return true;
            }

            T prev = valueList[0];
            int count = valueList.Count;
            for (int i=1; i < count; i++)
            {
                if(null != prev && prev.CompareTo(valueList[i]) > 0) {
                    return false;
                }
                prev = valueList[i];
            }
            return true;
        }

        /// <summary>
        /// Randomly shuffles items within a list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="rng">Random number generator.</param>
        public static void Shuffle<T>(IList<T> list, IRandomSource rng)
        {
            // Fisher–Yates shuffle.
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

            for(int i = list.Count-1; i > 0; i--)
            {
                int swapIdx = rng.Next(i + 1);
                T tmp = list[swapIdx];
                list[swapIdx] = list[i];
                list[i] = tmp;
            }
        }

        /// <summary>
        /// Randomly shuffles a sub-span of items within a list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="rng">Random number generator.</param>
        /// <param name="startIdx">The index of the first item in the segment.</param>
        /// <param name="endIdx">The index of the last item in the segment, i.e. endIdx is inclusive; the item at endIdx will participate in the shuffle.</param>
        public static void Shuffle<T>(IList<T> list, IRandomSource rng, int startIdx, int endIdx)
        {
            // Fisher–Yates shuffle.
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

            // Determine how many items in the list will be being shuffled
            int itemCount = (endIdx - startIdx);

            for(int i=endIdx; i > startIdx; i--)
            {
                int swapIdx = startIdx + rng.Next((i-startIdx) + 1);
                T tmp = list[swapIdx];
                list[swapIdx] = list[i];
                list[i] = tmp;
            }
        }

        /// <summary>
        /// Sort the items in the provided list. In addition we ensure that items that are defined as equal by the IComparer
        /// are arranged randomly.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list of items to sort.</param>
        /// <param name="comparer">The IComparer[T] implementation to use when comparing elements.</param>
        /// <param name="rng">Random number generator.</param>
        public static void SortUnstable<T>(List<T> list, IComparer<T> comparer, IRandomSource rng)
        {
            // Notes.
            // The naive approach is to shuffle the list items and then call Sort(). Regardless of whether the sort is stable or not,
            // the equal items would be arranged randomly within their sorted sub-segments.
            // However, typically lists are already partially sorted and that fact improves the performance of the sort. To try and
            // keep some of that benefit we call sort first, and then call shuffle on sub-segments of items identified as equal.
            if(list.Count < 10)
            {
                Shuffle(list, rng);
                list.Sort(comparer);
                return;
            }

            // Sort the list.
            list.Sort(comparer);

            // Scan for segments of items that are equal.
	        int startIdx = 0;
            int count = list.Count;

            while(TryFindSegment(list, comparer, ref startIdx, out int endIdx))
            {
                // Shuffle the segment of equal items.
                Shuffle(list, rng, startIdx, endIdx);

                // Test for the end of the list.
                // N.B. If endIdx points to one of the last two items then there can be no more segments (segments are made of at least two items).
                if(endIdx > count-3) {
                    break;
                }

                // Set the startIdx of the next candidate segment.
                startIdx = endIdx + 1;
            }
        }

        #endregion

        #region Private Static Methods

        private static bool TryFindSegment<T>(List<T> list, IComparer<T> comparer, ref int startIdx, out int endIdx)
        {
            int count = list.Count;
            for(; startIdx < count-1; startIdx++)
            {
                // Get a ref to the candidate segment start item.
                T startItem = list[startIdx];

                // Find the end of the segment of equal items.
                for(endIdx = startIdx+1; endIdx < count && 0 == comparer.Compare(startItem, list[endIdx]); endIdx++);
                
                // Test if a segment was found.
                if(endIdx > startIdx+1)
                {
                    // Segment found. Here the endIdx will always be pointing to the item after the segment end, so we decrement.
                    endIdx--;
                    return true;
                }
            }
			endIdx = 0;
            return false;
        }

        #endregion
    }
}
