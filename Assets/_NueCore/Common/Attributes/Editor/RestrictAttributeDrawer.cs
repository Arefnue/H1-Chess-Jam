﻿using UnityEditor;
using UnityEngine;

namespace _NueCore.Common.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(RestrictAttribute))]
    public class RestrictAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var requiredAttribute = this.attribute as RestrictAttribute;
                EditorGUI.BeginProperty(position, label, property);
                if (requiredAttribute != null)
                    property.objectReferenceValue = EditorGUI.ObjectField(position,
                        label,
                        property.objectReferenceValue,
                        requiredAttribute.RequiredType,
                        true);

                EditorGUI.EndProperty();
            }
            else
            {
                var previousColor = GUI.color;
                GUI.color = Color.red;
                EditorGUI.LabelField(position, label, new GUIContent("Property is not a reference type"));
                GUI.color = previousColor;
            }
        }
    }
}