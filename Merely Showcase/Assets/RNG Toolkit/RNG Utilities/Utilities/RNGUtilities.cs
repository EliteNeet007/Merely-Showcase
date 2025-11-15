using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace RightNowGames.Utilities
{
    /// <summary>
    /// A collection of utility functions.
    /// </summary>
    public static class RNGUtilities
    {
		#region Player Loop
		// The player loop is Unity's underlying life-cycle.
		// These methods are used to interact (insert/remove systems, edit, etc...) with the player loop.

		/// <summary>
		/// Inserts a system into Unity's PlayerLoop, as a subsystem of system T.
		/// </summary>
		/// <typeparam name="T">The system under which we want to add our system.</typeparam>
		/// <param name="loop">Reference to the player loop system.</param>
		/// <param name="systemToInsert">Our system.</param>
		/// <param name="index">The index at which we want to insert our system.</param>
		/// <returns></returns>
		public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
		{
			// If the signature loop isn't of type T, recursively check subsystems until you find a type T or check all systems.
			if (loop.type != typeof(T)) return HandleSubSystemLoop<T>(ref loop, systemToInsert, index);

			// If we reach this block, we are now working with a system of type T:
			// Create a list of player loop systems.
			var playerLoopSystemList = new List<PlayerLoopSystem>();
			// If the current system has any subsystems - add them to the list.
			if (loop.subSystemList != null) playerLoopSystemList.AddRange(loop.subSystemList);
			// Insert our system to the list at our chosen index.
			playerLoopSystemList.Insert(index, systemToInsert);
			// assign the system list back to the current system and return true.
			loop.subSystemList = playerLoopSystemList.ToArray();
			return true;
		}

		private static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
		{
			// No subsystems to loop through, return false.
			if (loop.subSystemList == null) return false;

			// Loop through subsystems to try and find one that matches the type T.
			for (int i = 0; i < loop.subSystemList.Length; i++)
			{
				// Doesn't match type T, continue.
				if (!InsertSystem<T>(ref loop.subSystemList[i], in systemToInsert, index)) continue;
				// Matches type T, return true.
				return true;
			}

			// Default return - false.
			return false;
		}

		/// <summary>
		/// Removes a system from the player loop.
		/// </summary>
		/// <typeparam name="T">The main system under which we look for the systemToRemove.</typeparam>
		/// <param name="loop">Reference to the player loop system.</param>
		/// <param name="systemToRemove">The system we want to remove from the player loop.</param>
		public static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove)
		{
			// If the current player loop system has no subsystems to loop through - return.
			if (loop.subSystemList == null) return;

			// create a list of all the current subsystems and loop through them.
			var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);
			for (int i = 0; i < playerLoopSystemList.Count; i++)
			{
				// If the current subsystem matches our signature systemToRemove:
				if (playerLoopSystemList[i].type == systemToRemove.type && playerLoopSystemList[i].updateDelegate == systemToRemove.updateDelegate)
				{
					// Remove the system and reassign the list to the player loop system.
					playerLoopSystemList.RemoveAt(i);
					loop.subSystemList = playerLoopSystemList.ToArray();
				}
			}

			// Perform the above process recursively through the entire player loop.
			HandleSubSystemLoopForRemoval<T>(ref loop, systemToRemove);
		}

		private static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem loop, PlayerLoopSystem systemToRemove)
		{
			// No subsystems to loop through, return false.
			if (loop.subSystemList == null) return;

			// Loop through subsystems to ensure we remove our signature systemToRemove from the player loop (if it exists).
			for (int i = 0; i < loop.subSystemList.Length; i++)
			{
				RemoveSystem<T>(ref loop.subSystemList[i], systemToRemove);
			}
		}

		/// <summary>
		/// Prints the signature PlayerLoopSystem to the console.
		/// </summary>
		/// <param name="loop"></param>
		public static void PrintPlayerLoop(PlayerLoopSystem loop)
		{
			// Create a new string builder and add a title.
			StringBuilder sb = new();
			sb.AppendLine("Unity Player Loop");

			// Iterate through all the signature loop's sub systems using the helper method.
			foreach (PlayerLoopSystem subSystem in loop.subSystemList)
			{
				PrintSubsystem(subSystem, sb, 0);
			}

			// Print the resulting string to the console.
			Debug.Log(sb.ToString());
		}

		private static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level)
		{
			// Add system level to the string builder.
			sb.Append(' ', level * 2).AppendLine(system.type.ToString());

			// Check if reached the end of the system branch, if so - return.
			if (system.subSystemList == null || system.subSystemList.Length == 0) return;

			// Iterate through this level's sub systems and recursively call this method.
			foreach (PlayerLoopSystem subSystem in system.subSystemList)
			{
				PrintSubsystem(subSystem, sb, level + 1);
			}
		}

		#endregion

		#region Names

		private static List<string> firstNameListFemale = new List<string>()
		{
			"Petra", "Iris", "Candice", "Kate", "Keira", "May", "Carla", "Elice", "Aya", "Nina", "Bonnie",
			"Natasha", "Anna", "Courtney", "Lizzy", "Hope", "Rose", "Galina", "Jane", "Sue", "Claire",
			"Sabrina", "Lillian", "Imane", "Lian", "Anitta", "Blaire", "Rebecca", "Kathrine", "Jade",
			"Freiya", "Natalie", "Talia", "Josephine", "Austin",
		};

		private static List<string> firstNameListMale = new List<string>()
		{
			"Peter", "Ezio", "Alex", "Mark", "Leon", "Oliver", "Christopher", "Connor", "Seth", "Steve",
			"Chris", "William", "Bill", "James", "Robert", "Edward", "Ivan", "Lucas", "Richard", "Tom",
			"Nathan", "Harry", "Bruce", "Dylan", "Jason", "Lewis", "Neil", "Adam", "Morgan", "Artem",
			"Ahmed", "Yusuf", "Noah", "Omar", "Tyson", "Austin"
		};
		
		private static List<string> fakeCityNameList = new List<string>()
		{
			"Vee", "Agen", "Agon", "Ardok", "Payn", "Lorr", "Loom", "Plaintown", "Deerstead", "Metropolis",
			"Coast", "Gotham", "Star", "Central", "Paradise", "Midnight", "Noonvile"
		};
		
		private static List<string> realCityNameList = new List<string>()
		{
			"Alabama", "New York", "Bangkok", "Lisbon", "Chicago", "Madrid", "London", "Washington",
			"Salem", "Madison", "Oxford", "Manchester", "Cleveland", "Kingston", "Oakland", "Winchester",
			"Boston", "Greenwich", "Austin"
		};

		/// <summary>
		/// Returns a random female name from the list of female first names.
		/// </summary>
		/// <returns></returns>
		public static string GetRandomFemaleName()
		{
			return firstNameListFemale[Random.Range(0, firstNameListFemale.Count)];
		}

		/// <summary>
		/// Returns a random male name from the list of male first names.
		/// </summary>
		/// <returns></returns>
		public static string GetRandomMaleName()
		{
			return firstNameListMale[Random.Range(0, firstNameListMale.Count)];
		}

		/// <summary>
		/// Returns a random city name out of a list of fictional/made up cities.
		/// </summary>
		/// <returns></returns>
		public static string GetRandomFakeCityName()
		{
			return fakeCityNameList[Random.Range(0, fakeCityNameList.Count)];
		}

		/// <summary>
		/// Returns a random city name out of a list of real cities.
		/// </summary>
		/// <returns></returns>
		public static string GetRandomRealCityName()
		{
			return realCityNameList[Random.Range(0, realCityNameList.Count)];
		}

		/// <summary>
		/// Returns the month name as a string, determined by month int signature.
		/// </summary>
		/// <param name="month">The month to return -> 1 = January, 2 = February, etc...</param>
		/// <returns></returns>
		public static string GetMonthName(int month)
		{
			return month switch
			{
				2 => "February",
				3 => "March",
				4 => "April",
				5 => "May",
				6 => "June",
				7 => "July",
				8 => "August",
				9 => "September",
				10 => "October",
				11 => "November",
				12 => "December",
				_ => "January",
			};
		}

		/// <summary>
		/// Returns the first 3 letters of the month name as a string, determined by the month int input.
		/// </summary>
		/// <param name="month">The month to return -> 1 = January, 2 = February, etc...</param>
		/// <returns></returns>
		public static string GetMonthNameShort(int month)
		{
			return GetMonthName(month).Substring(0, 3);
		}

		#endregion

		#region Color Calculation/Translation

		/// <summary>
		/// Returns color format 00-FF string, from value range 0->255.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Dec_to_Hex(int value)
		{
			return value.ToString("X2");
		}

		/// <summary>
		/// Returns color value range 0->255, from string format.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static int Hex_to_Dec(string hex)
		{
			return System.Convert.ToInt32(hex, 16);
		}

		/// <summary>
		/// Returns color foramt 00-FF string, from value range 0->1.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Dec01_to_Hex(float value)
		{
			return Dec_to_Hex((int)Mathf.Round(value * 255f));
		}

		/// <summary>
		/// Returns color value range 0->1, from string format.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static float Hex_to_Dec01(string hex)
		{
			return Hex_to_Dec(hex) / 255f;
		}

		/// <summary>
		/// Sets out values to hex format FF00FFAA - red, green, blue, alpha.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <param name="alpha"></param>
		public static void GetStringFromColor(Color color, out string red, out string green, out string blue, out string alpha)
		{
			red = Dec01_to_Hex(color.r);
			green = Dec01_to_Hex(color.g);
			blue = Dec01_to_Hex(color.b);
			alpha = Dec01_to_Hex(color.a);
		}

		/// <summary>
		/// Returns hex color format FF00FF, from float values.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static string GetStringFromColor(float r, float g, float b)
		{
			string red = Dec01_to_Hex(r);
			string green = Dec01_to_Hex(g);
			string blue = Dec01_to_Hex(b);
			return red + green + blue;
		}

		/// <summary>
		/// Returns hex color format FF00FF + alpha value - AA, from float values.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string GetStringFromColor(float r, float g, float b, float a)
		{
			string alpha = Dec01_to_Hex(a);
			return GetStringFromColor(r, g, b) + alpha;
		}

		/// <summary>
		/// Returns a Color value on a scale of Red->Yellow->Green, like a heat map.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Color GetRedGreenColor(float value)
		{
			float r = 0f;
			float g = 0f;
			if (value <= .5f)
			{
				r = 1f;
				g = value * 2f;
			}
			else
			{
				g = 1f;
				r = 1f - (value - .5f) * 2f;
			}
			return new Color(r, g, 0f, 1f);
		}

		/// <summary>
		/// Returns a random color with max alpha.
		/// </summary>
		/// <returns></returns>
		public static Color GetRandomColor()
		{
			return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		}

		#endregion

		#region Get World Position

		/// <summary>
		/// Returns a worldPosition based on the mouse's current screen position.<br />
		/// Sets the z value of the returned position to the signature zValue (default 0).<br />
		/// Calculates using Camera.main.
		/// </summary>
		/// <param name="zValue">The Z value of the returned position, default 0.</param>
		/// <returns></returns>
		public static Vector3 GetMouseWorldPosition2D(float zValue = 0)
		{
			return GetMouseWorldPosition2D(Camera.main, zValue);
		}

		/// <summary>
		/// Returns a worldPosition based on the mouse's current screen position.<br />
		/// Sets the z value of the returned position to the signature zValue (default 0).
		/// </summary>
		/// <param name="camera">The camera to be used for the calculation.</param>
		/// <param name="zValue">The Z value of the returned position, default 0.</param>
		/// <returns></returns>
		public static Vector3 GetMouseWorldPosition2D(Camera camera, float zValue = 0)
		{
			Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPosition.z = zValue;
			return mouseWorldPosition;
		}

		/// <summary>
		/// Returns a worldPosition based on the mouse's current position.<br />
		/// Calculates using Camera.main.
		/// </summary>
		/// <returns></returns>
		public static Vector3 GetMouseWorldPosition3D()
		{
			return GetMouseWorldPosition3D(Camera.main);
		}

		/// <summary>
		/// Returns a worldPosition based on the mouse's current position.<br />
		/// </summary>
		/// <param name="camera">The camera to be used for the calculation.</param>
		/// <returns></returns>
		public static Vector3 GetMouseWorldPosition3D(Camera camera)
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				return hit.point;
			}

			return Vector3.zero;
		}

		/// <summary>
		/// Returns a worldPosition based on the mouse's current position.<br />
		/// Calculates using Camera.main.
		/// </summary>
		/// <param name="layerMask">Which layers can the check interact with?</param>
		/// <returns></returns>
		public static Vector3 GetMouseWorldPosition3D(LayerMask layerMask)
		{
			return GetMouseWorldPosition3D(Camera.main, layerMask);
		}

		/// <summary>
		/// Returns a worldPosition based on the mouse's current position.
		/// </summary>
		/// <param name="camera">The camera to be used for the calculation.</param>
		/// <param name="layerMask">Which layers can the check interact with?</param>
		/// <returns></returns>
		public static Vector3 GetMouseWorldPosition3D(Camera camera, LayerMask layerMask)
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
			{
				return hit.point;
			}

			return Vector3.zero;
		}

		#endregion

		#region Rotation

		/// <summary>
		/// Sets the orbiter's position based on the bounds of the signature collider and the given targetPosition.<br />
		/// <br />
		/// distanceModifier augments the orbiter's distance from the center of the collider and is defaulted to 1f.<br />
		/// Default value maintains the orbiter on the collider's edge.
		/// </summary>
		/// <param name="orbiter"></param>
		/// <param name="targetPosition"></param>
		/// <param name="collider"></param>
		/// <param name="distanceModifier"></param>
		/// <returns></returns>
		public static Vector3 OrbitCircleCollider(GameObject orbiter, Vector3 targetPosition, CircleCollider2D collider, float distanceModifier = 1f)
		{
			orbiter.transform.position = collider.ClosestPoint(targetPosition);
			Vector3 offset = orbiter.transform.position - collider.bounds.center;
			offset.Normalize();
			offset *= (collider.radius * distanceModifier);
			return offset;
		}

		/// <summary>
		/// Rotates the camera transform around the target transform toward the given direction at the given rotationSpeed.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="target"></param>
		/// <param name="directionX"></param>
		/// <param name="directionY"></param>
		/// <param name="rotationSpeed"></param>
		public static void CameraOrbitRotation(Transform camera, Transform target, float directionX, float directionY, float rotationSpeed)
		{
			camera.RotateAround(target.position, camera.up, directionX * rotationSpeed);
			camera.RotateAround(target.position, camera.right, directionY * rotationSpeed * -1);
		}

		/// <summary>
		/// Returns a quaternion with the forward vector looking at the given direction.
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Quaternion LookAt2D(Vector2 direction)
		{
			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			return Quaternion.AngleAxis(angle, Vector3.forward);
		}

		#endregion

		#region Random

		/// <summary>
		/// Returns a random Vector2 based on 2 defined Vector2's.
		/// </summary>
		/// <param name="minimum"></param>
		/// <param name="maximum"></param>
		/// <returns></returns>
		public static Vector2 RandomVector2(Vector2 minimum, Vector2 maximum)
		{
			return new Vector2(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y));
		}

		/// <summary>
		/// Returns a random Vector3 based on 2 defined Vector3's.
		/// </summary>
		/// <param name="minimum"></param>
		/// <param name="maximum"></param>
		/// <returns></returns>
		public static Vector3 RandomVector3(Vector3 minimum, Vector3 maximum)
		{
			return new Vector3(Random.Range(minimum.x, maximum.x),
				Random.Range(minimum.y, maximum.y),
				Random.Range(minimum.z, maximum.z));
		}

		/// <summary>
		/// Returns a random number within range, based on the specified number of sides to the dice.
		/// </summary>
		/// <param name="numberOfSides"></param>
		/// <returns></returns>
		public static int RollADice(int numberOfSides) { return GetRandomFromRange(1, numberOfSides); }

		/// <summary>
		/// Returns the sum of all the int(s) passed in parameters.
		/// </summary>
		/// <param name="thingsToAdd"></param>
		/// <returns></returns>
		public static int Sum(params int[] thingsToAdd)
		{
			int result = 0;
			for (int i = 0; i < thingsToAdd.Length; i++)
			{
				result += thingsToAdd[i];
			}
			return result;
		}

		/// <summary>
		/// Returns a random int between min and max, both inclusive.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int GetRandomFromRange(int min, int max) { return Random.Range(min, max + 1); }

		/// <summary>
		/// Returns a random float between min and max, both inclusive.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static float GetRandomFromRange(float min, float max) { return Random.Range(min, max); }

		/// <summary>
		/// Returns a random success based on X% of chance.
		/// </summary>
		/// <param name="percent"></param>
		/// <returns></returns>
		public static bool Chance(int percent) { return Random.Range(1, 101) <= percent; }

		#endregion

		#region Lerp

		/// <summary>
		/// Internal method used to determine the lerp rate.
		/// </summary>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		private static float LerpRate(float rate, float deltaTime)
		{
			rate = Mathf.Clamp01(rate);
			float invRate = -Mathf.Log(1f - rate, 2f) * 60f;
			return Mathf.Pow(2f, -invRate * deltaTime);
		}

		/// <summary>
		/// Lerps a float towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static float Lerp(float value, float target, float rate, float deltaTime)
		{
			if (deltaTime == 0f) return value;
			return Mathf.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		/// <summary>
		/// Lerps a Vector2 towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static Vector2 Lerp(Vector2 value, Vector2 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f) return value;
			return Vector2.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		/// <summary>
		/// Lerps a Vector3 towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static Vector3 Lerp(Vector3 value, Vector3 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f) return value;
			return Vector3.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		/// <summary>
		/// Lerps a Vector4 towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static Vector4 Lerp(Vector4 value, Vector4 target, float rate, float deltaTime)
		{
			return Vector4.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		/// <summary>
		/// Lerps a Quaternion towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static Quaternion Lerp(Quaternion value, Quaternion target, float rate, float deltaTime)
		{
			if (deltaTime == 0f) return value;
			return Quaternion.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		/// <summary>
		/// Lerps a Color towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static Color Lerp(Color value, Color target, float rate, float deltaTime)
		{
			if (deltaTime == 0f) return value;
			return Color.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		/// <summary>
		/// Lerps a Color32 towards target at the specified rate.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="target"></param>
		/// <param name="rate"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public static Color32 Lerp(Color32 value, Color32 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f) return value;
			return Color32.Lerp(target, value, LerpRate(rate, deltaTime));
		}

		#endregion

		/// <summary>
		/// Returns a randomized string, 32 characters long (letters & numbers).<br/>
		/// Perfect for generating unique instance id's (GUID -> globally unique identifier).
		/// </summary>
		/// <returns></returns>
		public static string GenerateGUID()
		{
			return System.Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Simple implementation of XOR encryption.<br/><br/>
		/// Encrypts/Decrypts the signature data using the signature encryptionCodeWord.
		/// </summary>
		/// <param name="data">The data to be modified (encrypted/decrypted).</param>
		/// <param name="encryptionCodeWord">The code word to be used in the encryption/decryption process.</param>
		/// <returns></returns>
		public static string EncryptDecryptData(string data, string encryptionCodeWord)
		{
			string modifiedData = "";

			// Perform XOR operation on the signature data to encrypt.
			for (int i = 0; i < data.Length; i++)
			{
				modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
			}

			return modifiedData;
		}


	}
}