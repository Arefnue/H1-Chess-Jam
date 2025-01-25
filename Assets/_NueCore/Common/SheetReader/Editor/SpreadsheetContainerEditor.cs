using System;
using System.IO;
using System.Linq;
using System.Reflection;
using _NueCore.Common.SheetReader.Serializers;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _NueCore.Common.SheetReader.Editor
{
    [CustomEditor(typeof(SheetDataBase), true)]
    public class SpreadsheetContainerEditor : OdinEditor
    {
        private const string RelativePathNotation = "..";

        private SheetDataBase _container;
        private SheetImporter _importer;
        private string[] _possibleTogglesIds;

        public override void OnInspectorGUI()
        {
            _container = (SheetDataBase)target;
            var contentFieldBinding = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var contentField = _container.GetType().GetFields(contentFieldBinding).Where(fi => Attribute.IsDefined(fi, typeof(SheetContentAttribute))).FirstOrDefault();
            if (contentField == null)
            {
                EditorGUILayout.HelpBox("Error: Missing field marked with [SheetContentAttribute] attribute!",
                    MessageType.Error);
                base.OnInspectorGUI();
                return;
            }

            var content = contentField.GetValue(_container);

            _container.foldoutImportGUI =
                EditorGUILayout.BeginFoldoutHeaderGroup(_container.foldoutImportGUI, "Import");
            if (_container.foldoutImportGUI)
                DrawImportGUI(content);
            EditorGUILayout.EndFoldoutHeaderGroup();

            _container.foldoutSerializationGUI =
                EditorGUILayout.BeginFoldoutHeaderGroup(_container.foldoutSerializationGUI, "Serialization");
            if (_container.foldoutSerializationGUI)
                DrawSerializationGUI(content);
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space(16);

            base.OnInspectorGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawImportGUI(object content)
        {
            var listsFieldBinding = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var contentFields = content.GetType().GetFields(listsFieldBinding);
            var listsFields = contentFields
                .Where(fi => Attribute.IsDefined(fi, typeof(SheetPageAttribute)))
                .OrderBy(fi => fi.Name)
                .ToArray();

            _possibleTogglesIds ??= listsFields
                .Select(fi => fi.Name)
                .ToArray();

            var anySelected = _container.selectedTogglesIds.Any();
            var allSelected = !_possibleTogglesIds.Any(toggleId => !_container.selectedTogglesIds.Contains(toggleId));

            #region Document Id field

            _container.documentId = EditorGUILayout.TextField(
                new GUIContent(
                    "Document Id",
                    "The XXXX part in 'https://docs.google.com/spreadsheets/d/XXXX/edit' URL.\n\nNOTE: The document must be accessable by link."),
                _container.documentId);

            #endregion

            #region Control buttons

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(allSelected);
            if (GUILayout.Button("All"))
                SelectAll(true);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!anySelected);
            if (GUILayout.Button("None"))
                SelectAll(false);

            if (GUILayout.Button("Import"))
            {
                if (!_container.selectedTogglesIds.Any())
                {
                    Debug.LogWarning("Nothing is selected to import");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_container.documentId))
                    throw new Exception($"Document ID is not specified!");


                EditorUtility.DisplayProgressBar("Downloading definitions", "Initializing...", 0);

                var targetListsFields =
                    listsFields.Where(fi => _container.selectedTogglesIds.Contains(fi.Name)).ToArray();
                _importer = new SheetImporter(content, targetListsFields, _container.documentId);

                _importer.OnComplete += OnImportQueueComplete;
                _importer.OnOutputChanged += OnOutputChanged;
                _importer.OnProgressChanged += OnProgressChanged;

                _importer.Run();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            #endregion

            #region Pages toggles

            EditorGUILayout.LabelField("Pages to import:");

            EditorGUI.indentLevel += 1;

            foreach (var toggleId in _possibleTogglesIds)
            {
                var crntValue = _container.selectedTogglesIds.Contains(toggleId);
                var newValue = EditorGUILayout.Toggle($"{toggleId}", crntValue);
                if (newValue != crntValue)
                    SelectToggle(toggleId, newValue);
            }

            EditorGUI.indentLevel -= 1;

            #endregion
        }

        private void DrawSerializationGUI(object content)
        {
            #region Settings fields

            _container.serializationOutputPath = EditorGUILayout.TextField(
                new GUIContent(
                    "Output path",
                    "HINT: use '../' notation to specify path, relative to your project 'Assets' directory."),
                _container.serializationOutputPath);

            _container.serializationFileName = EditorGUILayout.TextField(
                "File Name",
                _container.serializationFileName);

            _container.serializationTypes =
                (SheetDataBase.SheetSerializationTypes)EditorGUILayout.EnumPopup("Format", _container.serializationTypes);

            #endregion

            #region Control buttons

            EditorGUILayout.BeginHorizontal();

            var disableSerializeButton = string.IsNullOrWhiteSpace(_container.serializationOutputPath) ||
                                         string.IsNullOrWhiteSpace(_container.serializationFileName);
            EditorGUI.BeginDisabledGroup(disableSerializeButton);
            if (GUILayout.Button("Serialize"))
            {
                var serializer = default(SheetSerializerBase);
                var outputPath = _container.serializationOutputPath.StartsWith(RelativePathNotation)
                    ? Path.GetFullPath(Path.Combine(Application.dataPath, _container.serializationOutputPath))
                    : _container.serializationOutputPath;
                if (!Directory.Exists(outputPath))
                    throw new Exception($"Missing directory '{outputPath}'!");

                switch (_container.serializationTypes)
                {
                    default:
                    case SheetDataBase.SheetSerializationTypes.JSON:

                        outputPath = Path.Combine(outputPath, _container.serializationFileName + ".json");
                        serializer = new SheetSerializerJson(content, outputPath);
                        serializer.Run();
                        break;

                    case SheetDataBase.SheetSerializationTypes.Binary:
                        outputPath = Path.Combine(outputPath, _container.serializationFileName + ".bin");
                        serializer = new SheetSerializerBinary(content, outputPath);
                        serializer.Run();
                        break;
                }
            }

            EditorGUI.EndDisabledGroup();


            EditorGUILayout.EndHorizontal();

            #endregion
        }

        private void SelectAll(bool mode)
        {
            foreach (var toggleId in _possibleTogglesIds)
                SelectToggle(toggleId, mode);
        }

        private void SelectToggle(string toggleId, bool mode)
        {
            if (mode && !_container.selectedTogglesIds.Contains(toggleId))
                _container.selectedTogglesIds.Add(toggleId);
            else if (!mode)
                _container.selectedTogglesIds.Remove(toggleId);
        }

        private void OnProgressChanged()
        {
            EditorUtility.DisplayProgressBar("Downloading definitions", _importer.Output, _importer.Progress);
        }

        private void OnOutputChanged()
        {
            EditorUtility.DisplayProgressBar("Downloading definitions", _importer.Output, _importer.Progress);
        }

        private void OnImportQueueComplete()
        {
            EditorUtility.SetDirty(target);

            EditorUtility.ClearProgressBar();

            _importer.OnComplete -= OnImportQueueComplete;
            _importer.OnOutputChanged -= OnOutputChanged;
            _importer.OnProgressChanged -= OnProgressChanged;
            _importer = null;
        }
    }
}