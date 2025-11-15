using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Enums
{
    public static class RNGEnums
    {
        
    }

	#region general Enums

	/// <summary>
	/// Defines an axis to be used for the logic.
	/// </summary>
	public enum Axis
	{
		XAxis,
		YAxis,
		ZAxis,
	}

	/// <summary>
	/// Defines a direction to be used for the logic.
	/// </summary>
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		Forward,
		Backward,
	}

	#endregion

	#region Grids

	/// <summary>
	/// Indicates the type of the grid and it's layout.
	/// </summary>
	public enum GridLayoutType2D
	{
		/// <summary>
		/// Default grid layout.<br/>
		/// Grid width corresponds to the X axis and height corresponds to the Y axis in world space.
		/// </summary>
		Vertical,
		/// <summary>
		/// "Map" layout.<br/>
		/// Grid width corresponds to the X axis and height corresponds to the Z axis in world space.
		/// </summary>
		Horizontal,
		/// <summary>
		/// Alternative vertical layout.<br/>
		/// Grid width corresponds to the Z axis and height corresponds to the Y axis in world space.
		/// </summary>
		VerticalDepth,
	}

	#endregion

	#region Rotations

	/// <summary>
	/// Defines different modes of rotation.
	/// </summary>
	public enum RotationMode
	{
		/// <summary>
		/// Defines applying a continuous rotation to the specified object.
		/// </summary>
		Continuous,
		/// <summary>
		/// Defines applying a periodic rotation to the specified object.
		/// </summary>
		Interval,
		/// <summary>
		/// Defines applying a rotation toward pre-defined values to the specified object.
		/// </summary>
		ToTargetRotation,
	}

	/// <summary>
	/// Defines different modes of object tracking via facing direction.
	/// </summary>
	public enum LookAtMode
	{
		/// <summary>
		/// Defines rotating the object to force it to look at another object.<br/>
		/// Can tilt the object in order to facilitate the behaviour.
		/// </summary>
		LookAtObject,
		/// <summary>
		/// Defines rotating the object to force it to look away from another object.<br/>
		/// Can tilt the object to facilitate the behaviour.
		/// </summary>
		LookAtObjectInverted,
		/// <summary>
		/// Defines rotating the object to force it to look at another object in 2D space.<br/>
		/// Can tilt the object in order to facilitate the behaviour.
		/// </summary>
		LookAtObject2D,
		/// <summary>
		/// Defines rotating the object to force it to look away from another object in 2D space.<br/>
		/// Can tilt the object to facilitate the behaviour.
		/// </summary>
		LookAtObjectInverted2D,
		/// <summary>
		/// Defines rotating the object to share the forward direction of another object.<br/>
		/// Only tilts the object if the other one is also tilted.
		/// </summary>
		LookAtObjectsForward,
		/// <summary>
		/// Defines rotating the object to the opposite of another object's forward direction.<br/>
		/// Only tilts the object if the other one is also tilted.
		/// </summary>
		LookAtObjectsForwardInverted,
	}

	#endregion

	#region String

	public enum eHashType
	{
		HMAC,
		HMACMD5,
		HMACSHA1,
		HMACSHA256,
		HMACSHA384,
		HMACSHA512,
		MD5,
		SHA1,
		SHA256,
		SHA384,
		SHA512
	}

	#endregion

	#region Transitions

	/// <summary>
	/// Defines the type of transition to be performed.
	/// </summary>
	public enum TransitionType
	{
		Fade,
		FlatSwipe,
		SquareRotationShrink90Degrees,
	}

	public enum TransitionDirection
	{
		UpToDown,
		DownToUp,
		RightToLeft,
		LeftToRight,
	}

	#endregion


}