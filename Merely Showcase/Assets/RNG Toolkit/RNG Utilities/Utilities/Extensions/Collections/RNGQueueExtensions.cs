using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
    public static class RNGQueueExtensions
    {
        /// <summary>
        /// Converts the extended Queue to an Array of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns>The generated Array.</returns>
        public static T[] ToArray<T>(this Queue<T> queue)
        {
            T[] ret = new T[queue.Count];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = queue.Dequeue();
            }

            return ret;
        }

        /// <summary>
        /// Converts the extended Queue to a list of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns>The generated List.</returns>
        public static List<T> ToList<T>(this Queue<T> queue)
        {
            List<T> ret = new List<T>();

            while (queue.Count > 0)
            {
                ret.Add(queue.Dequeue());
            }

            return ret;
        }


    }
}