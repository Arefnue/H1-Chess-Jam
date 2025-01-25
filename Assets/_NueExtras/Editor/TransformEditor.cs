using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace _NueExtras.Editor
{
    [CustomEditor(typeof(Transform), true)]
    [CanEditMultipleObjects]
    public class TransformEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor _defaultEditor;
        private Transform _transform;

        private void OnEnable()
        {
            _transform = target as Transform;
            _defaultEditor = UnityEditor.Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        }

        private void OnDisable()
        {
            if (_defaultEditor == null)
            {
                return;
            }
            MethodInfo disableMethod = _defaultEditor.GetType()
                .GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(_defaultEditor, null);
            DestroyImmediate(_defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            _defaultEditor.OnInspectorGUI();
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy"))
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(_transform);
            }

            if (GUILayout.Button("Paste"))
            {
                UnityEditorInternal.ComponentUtility.PasteComponentValues(_transform);
            }
        
            EditorGUILayout.EndHorizontal();
        
            if (GUILayout.Button("Reset"))
            {
                _transform.localPosition = Vector3.zero;
                _transform.localRotation = Quaternion.Euler(Vector3.zero);
                _transform.localScale = Vector3.one;
            }
        }
    }
}