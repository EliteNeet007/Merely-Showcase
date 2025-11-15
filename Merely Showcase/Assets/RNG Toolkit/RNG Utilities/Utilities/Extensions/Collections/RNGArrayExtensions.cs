using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
    public static class RNGArrayExtensions
    {
		/// <summary>
		/// Define a random index within the extended array's length.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns>A the item at the randomized index from the extended array.</returns>
		public static T GetRandom<T>(this T[] array)
		{
			return array[Random.Range(0, array.Length)];
		}

		/// <summary>
		/// Outputs the index of the returned value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <returns>A random item from the extended array.</returns>
		public static T GetRandomOutIndex<T>(this T[] array, out int index)
		{
			index = Random.Range(0, array.Length);
			return array[index];
		}

		/// <summary>
		/// Shuffles the extended array using the Fisher Yates algorithm.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns>The shuffled array.</returns>
		public static T[] Shuffle<T>(this T[] array)
		{
			// Fisher Yates shuffle algorithm, see https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
			for (int i = 0; i < array.Length; i++)
			{
				T tmp = array[i];
				int randomIndex = Random.Range(i, array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = tmp;
			}
			return array;
		}

		/// <summary>
		/// Adds a single item to the extended array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		/// <returns>The revised array.</returns>
		public static T[] AddSingle<T>(this T[] array, T value)
		{
			// Initialize the return array.
			T[] ret = new T[array.Length + 1];

			// Cycle thorugh the return array and assign the existing values.
			for (int i = 0; i < array.Length; i++)
			{
				ret[i] = array[i];
			}

			// Add the new value to the return array.
			ret[array.Length] = value;

			// Return the array.
			return ret;
		}

		/// <summary>
		/// Adds an array of items to the extended array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="values"></param>
		/// <returns>The revised array.</returns>
		public static T[] AddMultiple<T>(this T[] array, T[] values)
		{
			// Initialize the return array.
			T[] ret = new T[array.Length + values.Length];

			// Cycle through the return array and assign the existing values.
			for (int i = 0; i < array.Length; i++)
			{
				ret[i] = array[i];
			}

			// Add the new values to the return array.
			for (int i = 0; i < values.Length; i++)
			{
				ret[array.Length + i] = values[i];
			}

			// Return the array.
			return ret;
		}

		/// <summary>
		/// Removes the signature value from the extended Array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		/// <returns>The modified extended Array.</returns>
		public static T[] RemoveSingle<T>(this T[] array, T value)
		{
			T[] ret = new T[array.Length - 1];
			int index = 0;

			for (int i = 0; i < array.Length; i++)
			{
				if ((object)array[i] != (object)value)
				{
					ret[index] = array[i];
					index++;
				}
			}

			return ret;
		}

		/// <summary>
		/// Removes the signature values from the extended Array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="values"></param>
		/// <returns>The modified extended Array.</returns>
		public static T[] RemoveMultiple<T>(this T[] array, T[] values)
		{
			T[] ret = new T[array.Length - values.Length];
			int retIndex = 0;
			int valuesIndex = 0;

			for (int i = 0; i < array.Length; i++)
			{
				for (int J = 0 + valuesIndex; J < values.Length; J++)
				{
					if ((object)array[i] != (object)values[J])
					{
						ret[retIndex] = array[i];
						retIndex++;
						valuesIndex++;
						continue;
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// Converts the extended Array into a Queue of the same type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns>The generated Queue.</returns>
		public static Queue<T> ToQueue<T>(this T[] array)
		{
			Queue<T> ret = new Queue<T>();

			for (int i = 0; i < array.Length; i++) ret.Enqueue(array[i]);

			return ret;
		}

		/// <summary>
		/// Used to extend the functionality of an Array, making it a more valid replacement for a List.<br/><br/>
		/// Checks for the number of items within the extended Array that aren't null,<br/>
		/// Which represents the number of items actually contained within the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns>The number of non-null items within the extended array.</returns>
		public static int Count<T>(this T[] array)
		{
			int count = 0;

			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null) count++;
			}

			return count;
		}

		/// <summary>
		/// Cycles through the extended Array in order to find the first null item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns>The index of the first null item within the extended Array.<br/>
		/// If no null item is found, returns -1.</returns>
		public static int FirstEmptyIndex<T>(this T[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null) return i;
			}

			return -1;
		}


	}
}