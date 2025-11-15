using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RightNowGames.Utilities
{
	/// <summary>
	/// An attribute to conditionally hide fields in the inspector, based on a boolean value.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public class BoolConditionAttribute : PropertyAttribute
	{
		// Syntax Example:
		/*
			boolTest - the specific boolean variable, controlled through the inspector, it's value is the one being checked.
		
			Input the name of the bool variable as a string:

			[RNGBoolCondition("boolTest")]
            public string boolTestString = "Visible";
		*/

		public string conditionBool = "";
		public bool conditionState;

		public bool GetFlag(bool value)
		{
			return value == conditionState;
		}

		public BoolConditionAttribute(string conditionBoolean, bool conditionState = true)
		{
			this.conditionBool = conditionBoolean;
			this.conditionState = conditionState;
		}
	}
}