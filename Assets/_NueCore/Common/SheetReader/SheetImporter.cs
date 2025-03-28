﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace _NueCore.Common.SheetReader
{
    public class SheetImporter
    {
        public const string URLFormat = @"https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&sheet={1}";
        private readonly CultureInfo _dotCulture = new CultureInfo("en-US");
        private readonly CultureInfo _commaCulture = new CultureInfo("fr-FR");

        private readonly object _targetObject;
        private readonly FieldInfo[] _targetListsFields;
        private readonly string _documentID;

        public event Action OnComplete;

        public bool abort;

        private string _output;
        public string Output
        {
            get => _output;

            private set
            {
                _output = value;
                OnOutputChanged?.Invoke();
            }
        }
        public event Action OnOutputChanged;

        public float progress;
        public float Progress
        {
            get => progress;

            private set
            {
                progress = Mathf.Clamp01(value);
                OnProgressChanged?.Invoke();
            }
        }
        public event Action OnProgressChanged;

        private float ProgressElementDelta
            => 1f / _targetListsFields.Length;

        public SheetImporter(object targetObject, FieldInfo[] targetListsFields, string documentID)
        {
            this._targetObject = targetObject;
            this._targetListsFields = targetListsFields;
            this._documentID = documentID;
        }

        public async void Run()
        {
            abort = false;
            var webClient = new WebClient();

            for (int i = 0; i < _targetListsFields.Length && !abort; i++)
                await PopulateList(_targetObject, _targetListsFields[i], webClient);

            webClient.Dispose();

            OnComplete?.Invoke();
        }

        private async Task PopulateList(object targetObject, FieldInfo targetListField, WebClient webClient)
        {
            var contentType = targetListField.FieldType.GetGenericArguments().SingleOrDefault();
            if (contentType is null)
                throw new Exception($"Could not identify type of defs stored in {targetListField.Name}");

            #region Downloading page

            var pageAttribute = (SheetPageAttribute)Attribute.GetCustomAttribute(targetListField, typeof(SheetPageAttribute));
            var pageName = pageAttribute.name;

            Output = $"Downloading page '{pageName}'...";

            var url = string.Format(URLFormat, _documentID, pageName);
            var request = default(Task<string>);

            try
            {
                request = webClient.DownloadStringTaskAsync(url);
            }
            catch (WebException)
            {
                abort = true;
                throw new Exception($"Bad URL '{url}'");
            }

            while (!request.IsCompleted)
                await Task.Delay(100);

            var rawTable = Regex.Split(request.Result, "\r\n|\r|\n");
            request.Dispose();

            Progress += 1 / 3f * ProgressElementDelta;

            #endregion

            #region Analyzing and splitting raw text

            Output = $"Analysing headers...";

            var headersRaw = Split(rawTable[0]);
            var idHeaderIdx = -1;
            var headers = new List<string>();
            var emptyHeadersIdxs = new List<int>();
            for (int i = 0; i < headersRaw.Length; i++)
            {
                if (string.IsNullOrEmpty(headersRaw[i]))
                {
                    emptyHeadersIdxs.Add(i);
                    continue;
                }

                if (idHeaderIdx == -1 && headersRaw[i].ToLower() == "id")
                    idHeaderIdx = i;

                headers.Add(headersRaw[i]);
            }

            var rows = new List<string[]>();
            for (int i = 1; i < rawTable.Length; i++)
            {
                var substrings = Split(rawTable[i]);
                if (idHeaderIdx != -1 && string.IsNullOrEmpty(substrings[idHeaderIdx]))
                    continue;

                rows.Add(substrings.Where((val, index) => !emptyHeadersIdxs.Contains(index)).ToArray());
            }

            Progress += 1 / 3f * ProgressElementDelta;

            #endregion

            #region Parsing and populating list of defs 

            Output = $"Populating list of defs '{targetListField.Name}'<{contentType.Name}>...";

            var headersToFields = new Dictionary<string, FieldInfo>();
            foreach (var header in headers)
            {
                // TO DO:
                // Add support of fields with names other than the header names via an attribute
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var fieldInfo = contentType.GetField(header, bindingFlags);
                if (fieldInfo is null)
                {
                    Debug.LogWarning($"Header '{header}' match no field in {contentType.Name} type");
                    continue;
                }
                headersToFields.Add(header, fieldInfo);
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(contentType));
            foreach (var row in rows)
            {
                var item = Activator.CreateInstance(contentType);

                for (int i = 0; i < headers.Count; i++)
                    if (headersToFields.TryGetValue(headers[i], out var field))
                    {
                        if (i>=row.Length)
                        {
                            $"Has Empty Field {row}_{i}".NLog(Color.yellow);
                            continue;
                        }
                        field.SetValue(item, Parse(row[i], field.FieldType));
                    }

                list.Add(item);
            }

            targetListField.SetValue(targetObject, list);

            Progress += 1 / 3f * ProgressElementDelta;

            #endregion
        }

        public string[] Split(string line)
        {
            bool isInsideQuotes = false;
            List<string> result = new List<string>();

            string temp = string.Empty;
            for (int i = 0; i < line.Length; i++)
                if (line[i] == '"')
                {
                    isInsideQuotes = !isInsideQuotes;

                    if (i == line.Length - 1)
                        result.Add(temp);
                }
                else
                {
                    if (!isInsideQuotes && line[i] == ',')
                    {
                        result.Add(temp);
                        temp = string.Empty;
                    }
                    else
                        temp += line[i];
                }

            return result.ToArray();
        }

        public static object Parse(string s, Type type)
        {
            var result = default(object);

            if (type == typeof(string))
                return s;
            else if (type == typeof(int))
            {
                if (int.TryParse(s, out var resultInt))
                    return resultInt;
            }
            else if(type == typeof(bool))
            {
                if (bool.TryParse(s, out var resultBool))
                    return resultBool;
            }
            else if (type == typeof(float))
            {
                if (float.TryParse(s.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var resultFloat))
                    return resultFloat;
            }
            else if (type == typeof(double))
            {
                if (double.TryParse(s.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var resultFloat))
                    return resultFloat;
            }
            else if (type.IsEnum)
            {
                try
                {
                    result = Enum.Parse(type, s, true);
                }
                catch (ArgumentException)
                {
                    result = default(object);
                }
                return result;
            }

            return result;
        }
    }
}
