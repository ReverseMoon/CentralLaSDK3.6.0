using Kittenji;
using System.Collections;
using System.Collections.Generic;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace Kittenji
{
    [CustomEditor(typeof(CentralAreaTransport))]
    public class CentralAreaTransportEditor : UEditor<CentralAreaTransport>
    {
        SerializedProperty Origin;
        SerializedProperty Target;
        SerializedProperty ToggleActive;
        SerializedProperty DefaultArea;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool doRepaint = false;
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            serializedObject.Update();

            EditorGUILayout.PropertyField(ToggleActive);
            EditorGUILayout.PropertyField(DefaultArea);
            EditorGUILayout.PropertyField(Origin);
            EditorGUILayout.PropertyField(Target);

            serializedObject.ApplyModifiedProperties();

            if (Target.objectReferenceValue != null)
            {
                EditorGUILayout.Space(4);

                EditorGUILayout.BeginVertical("box");
                EditorGUI.BeginChangeCheck();
                Vector3 value = EditorGUILayout.Vector3Field("Position", Script.Target.localPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(Script.Target, "Target Position");
                    Script.Target.localPosition = value;
                }

                EditorGUI.BeginChangeCheck();
                value = EditorGUILayout.Vector3Field("Rotation", Script.Target.localEulerAngles);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(Script.Target, "Target Rotation");
                    Script.Target.localEulerAngles = value;
                }
                EditorGUILayout.EndVertical();
            }

            if (doRepaint) Repaint();
        }
    }
}