using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RightNowGames.Utilities.Editor
{
	// original implementation by http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
	[CustomPropertyDrawer(typeof(EnumConditionAttribute))]
	public class EnumConditionAttributeDrawer : PropertyDrawer
	{
		#if UNITY_EDITOR
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EnumConditionAttribute enumConditionAttribute = (EnumConditionAttribute)attribute;
			bool enabled = GetConditionAttributeResult(enumConditionAttribute, property);
			bool previouslyEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (!enumConditionAttribute.hidden || enabled) EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = previouslyEnabled;
		}
		#endif

		private static Dictionary<string, string> cachedPaths = new Dictionary<string, string>();

		private bool GetConditionAttributeResult(EnumConditionAttribute enumConditionAttribute, SerializedProperty property)
		{
			bool enabled = true;

			SerializedProperty enumProp;
			string enumPropPath = string.Empty;
			string propertyPath = property.propertyPath;

			if (!cachedPaths.TryGetValue(propertyPath, out enumPropPath))
			{
				enumPropPath = propertyPath.Replace(property.name, enumConditionAttribute.conditionEnum);
				cachedPaths.Add(propertyPath, enumPropPath);
			}

			enumProp = property.serializedObject.FindProperty(enumPropPath);

			if (enumProp != null)
			{
				int currentEnum = enumProp.enumValueIndex;
				enabled = enumConditionAttribute.ContainsBitFlag(currentEnum);
			}
			else Debug.LogWarning("No matching boolean found for ConditionAttribute in object: " + enumConditionAttribute.conditionEnum);

			return enabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			EnumConditionAttribute enumConditionAttribute = (EnumConditionAttribute)attribute;
			bool enabled = GetConditionAttributeResult(enumConditionAttribute, property);

			if (!enumConditionAttribute.hidden || enabled) return EditorGUI.GetPropertyHeight(property, label);
			else return -EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
