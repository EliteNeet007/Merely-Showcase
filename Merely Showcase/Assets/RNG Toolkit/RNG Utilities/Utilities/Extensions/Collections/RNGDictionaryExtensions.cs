using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RightNowGames.Utilities
{
	public static class RNGDictionaryExtensions
	{
		/// <summary>
		/// Finds a key (if one exists) that matches the signature value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="W"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="value"></param>
		/// <returns>The found key (if it exists).</returns>
		public static T GetKeyByValue<T, W>(this Dictionary<T, W> dictionary, W value)
		{
			T key = default;
			foreach (KeyValuePair<T, W> pair in dictionary)
			{
				if (pair.Value.Equals(value))
				{
					key = pair.Key;
					break;
				}
			}
			return key;
		}

		/// <summary>
		/// Defines a random index within the extended dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary"></param>
		/// <returns>The KeyValuePair matching the defined index.</returns>
		public static KeyValuePair<TKey, TValue> GetRandomizedKeyAndValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return dictionary.ElementAt(Random.Range(0, dictionary.Count));
		}

		/// <summary>
		/// Defines a random index within the extended dictionary and outputs it.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="index"></param>
		/// <returns>The KeyValuePair matching the defined index.</returns>
		public static KeyValuePair<TKey, TValue> GetRandomKeyValuePairOutIndex<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, out int index)
		{
			index = Random.Range(0, dictionary.Count);
			return dictionary.ElementAt(index);
		}


	}
}