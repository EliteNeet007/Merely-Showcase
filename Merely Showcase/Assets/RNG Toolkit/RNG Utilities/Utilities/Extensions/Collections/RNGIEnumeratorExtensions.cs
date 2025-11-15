using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
	public static class RNGIEnumeratorExtensions
	{
		/// <summary>
		/// Cycles through the extended IEnumerator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerator"></param>
		/// <returns>An IEnumerable instance of the extended IEnumerator.</returns>
		public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator)
		{
			while (enumerator.MoveNext()) yield return enumerator.Current;
		}


	}
}