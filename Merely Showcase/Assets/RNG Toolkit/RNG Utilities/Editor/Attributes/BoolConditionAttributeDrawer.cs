using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RightNowGames.Utilities.Editor
{
	// original implementation by http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
	[CustomPropertyDrawer(typeof(BoolConditionAttribute))]
	public class BoolConditionAttributeDrawer : PropertyDrawer
	{
		#if UNITY_EDITOR
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			BoolConditionAttribute boolConditionAttribute = (BoolConditionAttribute)attribute;
			bool enabled = GetConditionAttributeResult(boolConditionAttribute, property);
			bool previouslyEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (enabled) EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = previouslyEnabled;
		}
		#endif

		private static Dictionary<string, string> cachedPathes = new Dictionary<string, string>();

		private bool GetConditionAttributeResult(BoolConditionAttribute boolConditionAttribute, SerializedProperty property)
		{
			bool enabled = true;

			SerializedProperty boolProp;
			string boolPropPath = string.Empty;
			string propertyPath = property.propertyPath;

			if (!cachedPathes.TryGetValue(propertyPath, out boolPropPath))
			{
				boolPropPath = propertyPath.Replace(property.name, boolConditionAttribute.conditionBool);
				cachedPathes.Add(propertyPath, boolPropPath);
			}

			boolProp = property.serializedObject.FindProperty(boolPropPath);

			if (boolProp != null) enabled = boolConditionAttribute.GetFlag(boolProp.boolValue);
			else Debug.LogWarning("No matching boolean found for conditionAttribute found in object: " + boolConditionAttribute.conditionBool);

			return enabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			BoolConditionAttribute boolConditionAttribute = (BoolConditionAttribute)attribute;
			bool enabled = GetConditionAttributeResult(boolConditionAttribute, property);

			if (enabled) return EditorGUI.GetPropertyHeight(property, label);
			else return -EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
