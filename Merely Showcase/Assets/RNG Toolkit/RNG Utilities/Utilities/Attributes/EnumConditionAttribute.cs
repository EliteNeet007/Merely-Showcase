using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RightNowGames.Utilities
{
	/// <summary>
	/// An attribute to conditionally hide fields in the inspector, based on the current selection in an enum.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public class EnumConditionAttribute : PropertyAttribute
	{
		// Syntax Example:
		/*
			ConditionTest - the enum used to limit visibility of the "enumTestString" field.
			conditionTest - the specific instance of the enum, controlled through the inspector, it's value is the one being checked.
		
			Input the name of the enum variable as a string, then specify the int value of the desired state of the enum:

			[RNGEnumCondition("conditionTest", (int)ConditionTest.Visible)]
			public string enumTestString = "Visible";
		*/

		public string conditionEnum = "";
		public bool hidden = false;

		BitArray bitArray = new BitArray(32);
		public bool ContainsBitFlag(int enumValue)
		{
			return bitArray.Get(enumValue);
		}

		public EnumConditionAttribute(string conditionBoolean, params int[] enumValues)
		{
			this.conditionEnum = conditionBoolean;
			this.hidden = true;

			for (int i = 0; i < enumValues.Length; i++)
			{
				bitArray.Set(enumValues[i], true);
			}
		}
	}
}
