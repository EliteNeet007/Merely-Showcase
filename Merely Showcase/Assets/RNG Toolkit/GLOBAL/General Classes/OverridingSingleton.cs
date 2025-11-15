using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Basic
{
    /// <summary>
    /// An alternative to the basic PersistentSingleton, essentially the inverse.<br/>
    /// Will override any older components of the same type on Awake(),<br/>
    /// set itself to DontDestroyOnLoan and assign itself as the instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OverridingSingleton<T> : Singleton<T> where T : MonoBehaviour
	{
		/// <summary>
		/// The time at which this instance was initialized.
		/// </summary>
		public float InitializationTime {  get; private set; }

		/// <summary>
		/// Make sure to call base.Awake() in override - BEFORE ANY new code in Awake().
		/// </summary>
		protected override void Awake()
		{
			InitializeInstance();
		}

		/// <summary>
		/// Further extends the basic implementation of assigning Instance as itself:<br/><br/>
		/// 1. Sets it's own InitializationTime.
		/// 2. Sets itself to DontDestroyOnLoad.
		/// 3. Sets hideFlags to HideAndDontSave to ensure this object isn't saved with the scene,<br/>
		/// (since it overrode the instance that belongs in the scene).
		/// 4. Finds all instances of this object that are older than the current one and destroys them.
		/// 5. Finally, sets itself as the static Instance.
		/// </summary>
		protected override void InitializeInstance()
		{
			// Sometimes Awake() can be called while in edite mode,
			// meaning the game isn't running but Awake() is still called.
			// Check if the game is running, if it isn't - return.
			if (!CanInitialize()) return;

			// Set InitializationTime.
			InitializationTime = Time.time;

			// Sets itself as DontDestroyOnLoad -> to persist through scene changes.
			DontDestroyOnLoad(gameObject);

			// Hide flags to ensure this object is never saved with the scene.
			gameObject.hideFlags = HideFlags.HideAndDontSave;

			// Find older instances of this object and destroy them:
			// Gather all instances of this type.
			T[] instances = FindObjectsByType<T>(FindObjectsSortMode.None);
			// Loop through instances array.
			foreach (T instance in instances)
			{
				// If the instance currently looped over is older than our new instance - destroy it.
				if (instance.GetComponent<OverridingSingleton<T>>().InitializationTime < InitializationTime)
				{
					Destroy(instance.gameObject);
				}
			}

			// Set Instance as this.
			Instance = this as T;
		}
	}
}