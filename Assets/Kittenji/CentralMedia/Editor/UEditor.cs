using System;
using System.Collections.Generic;
using System.Reflection;
using UdonSharp;
using UnityEditor;
using UnityEngine;

namespace Kittenji
{
    public static class CustomEditorExtensions
    {
        public static bool ContainsArrayElement(this SerializedProperty property, int value)
        {
            int size = property.arraySize;
            if (size == 0) return false;

            for (int i = 0; i < size; i++)
                if (property.GetArrayElementAtIndex(i).intValue.Equals(value)) return true;
            
            return false;
        }

        public static bool ContainsArrayElement(this SerializedProperty property, string value)
        {
            int size = property.arraySize;
            if (size == 0) return false;

            for (int i = 0; i < size; i++)
                if (property.GetArrayElementAtIndex(i).stringValue.Equals(value)) return true;

            return false;
        }

        public static string[] ToStringArray(this SerializedProperty property, bool isUrl = false)
        {
            string[] array = new string[property.arraySize];

            for (int i = 0; i < array.Length; i++) {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                array[i] = isUrl ? element.FindPropertyRelative("url").stringValue : element.stringValue;
            }

            return array;
        }
    }

    public class UEditor<T> : Editor where T : UdonSharpBehaviour
    {
        public T Script;
        public Transform transform => Script.transform;
        public string ScriptPath;
        public string ScriptFolder;

        private int CachedFoldoutHeaderIndex = -1;
        private Dictionary<int, bool> CachedFoldoutHeaderState = new Dictionary<int, bool>();

        public virtual void OnEnable()
        {
            Script = target as T;
            MonoScript ms = MonoScript.FromMonoBehaviour(Script);
            ScriptPath = AssetDatabase.GetAssetPath(ms);
            /// Debug.Log("Script Path: " + ScriptPath);
            ScriptFolder = System.IO.Path.GetDirectoryName(ScriptPath);

            Type genericType = typeof(T);
            Type type = this.GetType();
            Type serializedPropertyType = typeof(SerializedProperty);
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.Equals(serializedPropertyType)) continue;
                SerializedProperty property = serializedObject.FindProperty(field.Name);
                if (property == null) throw new System.MissingFieldException("Could not find property '" + field.Name + "'");
                field.SetValue(this, property);
                /// Debug.Log($"<color=#0c0c0c>[<color=#4ec9b0>UEditor</color><color=white><</color><color=#96cf9a>{genericType.Name}</color><color=white>></color>]</color> Found property: {field.Name}");
            }

            Debug.Log($"<color=#0c0c0c>[<color=#4ec9b0>UEditor</color><color=white><</color><color=#96cf9a>{genericType.Name}</color><color=white>></color>]</color> Initialized.");
        }

        public override void OnInspectorGUI()
        {
            CachedFoldoutHeaderIndex = 0;
        }

        public bool BeginCachedFoldoutHeaderGroup(string content, string tooltip) => BeginCachedFoldoutHeaderGroup(new GUIContent(content, tooltip));
        public bool BeginCachedFoldoutHeaderGroup(string content) => BeginCachedFoldoutHeaderGroup(new GUIContent(content));
        public bool BeginCachedFoldoutHeaderGroup(GUIContent content)
        {
            int index = CachedFoldoutHeaderIndex++;
            bool state = CachedFoldoutHeaderState.ContainsKey(index) ? CachedFoldoutHeaderState[index] : false;
            state = EditorGUILayout.BeginFoldoutHeaderGroup(state, content);
            CachedFoldoutHeaderState[index] = state;
            EditorGUI.indentLevel++;
            return state;
        }
        public void EndCachedFoldoutHeaderGroup()
        {
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        public static (long, int) UnixTSField(GUIContent label, long timestamp)
        {
            DateTime dateTime = UnixTimeStampToDateTime(timestamp);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MinWidth(10));
            int month = EditorGUILayout.IntField(dateTime.Month, GUILayout.Width(42));
            int day = EditorGUILayout.IntField(dateTime.Day, GUILayout.Width(42));
            day = Mathf.Min(Mathf.Max(1, day), DateTime.DaysInMonth(dateTime.Year, month));
            int year = (short)EditorGUILayout.IntField(dateTime.Year, GUILayout.Width(60));
            GUILayout.EndHorizontal();

            dateTime = new DateTime(year, month, day);

            return (DateTimeToUnixTimeStamp(dateTime), year);
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
        }
        public static long DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            DateTimeOffset timeOffset = new DateTimeOffset(dateTime.ToUniversalTime());
            return timeOffset.ToUnixTimeSeconds();
        }

        [InitializeOnLoadMethod]
        public static void ClearProgressBarOnLoad()
        {
            EditorUtility.ClearProgressBar();
        }

        public static class FlagsHelper
        {
            public static bool IsSet(int flags, int value) => (flags & (1 << value)) != 0;

            public static void Set(ref int flags, int value)
            {
                flags = flags | (1 << value);
            }

            public static void Unset(ref int flags, int value)
            {
                flags = flags & (~(1 << value));
            }
        }
    }
}