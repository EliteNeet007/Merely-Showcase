using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
    public static class RNGListExtensions
    {
		/// <summary>
		/// Swaps two items in a list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			T temporary = list[i];
			list[i] = list[j];
			list[j] = temporary;
		}

		/// <summary>
		/// Shuffles a list randomly.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list.Swap(i, Random.Range(i, list.Count));
			}
		}

		/// <summary>
		/// Defines a random index within the extended list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns>The item matching the defined idex.</returns>
		public static T GetRandomFromList<T>(this IList<T> list)
		{
			return list[Random.Range(0, list.Count)];
		}

		/// <summary>
		/// Defines a random index within the extended list, outputs the defined index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <returns>The item matching the defined index.</returns>
		public static T GetRandomFromListOutIndex<T>(this IList<T> list, out int index)
		{
			index = Random.Range(0, list.Count);
			return list[index];
		}

		/// <summary>
		/// Moves a member at index from listToTakeFrom to this list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listToAddTo"></param>
		/// <param name="listToTakeFrom"></param>
		/// <param name="index"></param>
		public static void TakeFromOtherList<T>(this IList<T> listToAddTo, IList<T> listToTakeFrom, int index)
		{
			T target = listToTakeFrom[index];
			listToTakeFrom.Remove(target);
			listToAddTo.Add(target);
		}

		/// <summary>
        /// Add an entire other list to this list, essentially merging the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToAddTo"></param>
        /// <param name="listToAdd"></param>
		public static void AddWholeList<T>(this IList<T> listToAddTo, IList<T> listToAdd)
        {
            for (int i = 0; i < listToAdd.Count; i++)
			{
				listToAddTo.Add(listToAdd[i]);
			}
        }

		/// <summary>
        /// Removes the full list to bve removed from the main list.<br/>
		/// Ensures we do not try to remove an item that is not present before we remove it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToRemoveFrom"></param>
        /// <param name="listToRemove"></param>
		public static void RemoveWholeList<T>(this IList<T> listToRemoveFrom, IList<T> listToRemove)
        {
            for (int i = 0; i < listToRemove.Count; i++)
			{
				if (listToRemoveFrom.Contains(listToRemove[i])) listToRemoveFrom.Remove(listToRemove[i]);
			}
        }

		/// <summary>
		/// Converts the extended List to a Queue of the same type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns>The generated Queue.</returns>
		public static Queue<T> ToQueue<T>(this IList<T> list)
		{
			Queue<T> ret = new Queue<T>();

			for (int i = 0; i < list.Count; i++)
			{
				ret.Enqueue(list[i]);
			}

			return ret;
		}


	}
}