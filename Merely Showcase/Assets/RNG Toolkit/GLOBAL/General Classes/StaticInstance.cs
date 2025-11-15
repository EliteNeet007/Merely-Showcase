using UnityEngine;

namespace RightNowGames.Basic
{
	/// <summary>
	/// Basic static instance functionality to inherit.<br/><br/>
	/// Similar to singleton in the sense that it re-assigns the reference of instance to itself on Awake(),<br/>
	/// but without destroying the previous instance(s).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
	{
		/// <summary>
		/// Static instance of this object.<br/>
		/// Publicly accessible, privately set.
		/// </summary>
		 public static T Instance { get; protected set; }

		/// <summary>
		/// If you don't override and call InitializeInstance() in Awake(),<br/>
		/// make sure to call base.Awake() in override - BEFORE ANY new code in Awake().
		/// </summary>
		protected virtual void Awake()
		{
			InitializeInstance();
		}

		/// <summary>
		/// Base implementation sets instance to null and destroys the object before quiting.
		/// </summary>
		protected virtual void OnApplicationQuit()
		{
			// Sets the instance to null and destroys the attached object.
			Instance = null;
			Destroy(gameObject);
		}

		/// <summary>
		/// Handles all the necessary steps of initialization for this object.<br/><br/>
		/// Make sure to call base.InitializeInstnace() in override if you need to override.
		/// </summary>
		protected virtual void InitializeInstance()
		{
			// Set instance as this.
			Instance = this as T;
		}

		/// <summary>
		/// Returns true if the static instance isn't null.
		/// </summary>
		/// <returns></returns>
		public static bool HasInstance() => Instance != null;


	}
}