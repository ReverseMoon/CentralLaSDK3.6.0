using System.Collections;
using System.Collections.Generic;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Kittenji
{
    [CustomEditor(typeof(CentralMediaSensor))]
    public class CentralMediaSensorEditor : UEditor<CentralMediaSensor>
    {
        SerializedProperty MediaLibrary;
        SerializedProperty TargetVideoPlayer;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool doRepaint = false;
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            serializedObject.Update();

            EditorGUILayout.PropertyField(MediaLibrary);
            EditorGUILayout.PropertyField(TargetVideoPlayer);

            serializedObject.ApplyModifiedProperties();
            if (doRepaint) Repaint();
        }

        private void OnSceneGUI()
        {
            if (MediaLibrary.objectReferenceValue != null)
            {
                CentralMediaLibrary mediaLibrary = MediaLibrary.objectReferenceValue as CentralMediaLibrary;

                var matrix = Handles.matrix;
                Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                //Handles.color = Color.cyan;
                //Handles.DrawWireCube(Vector3.zero, rect.sizeDelta);
                Handles.color = Color.blue;
                Handles.DrawSolidRectangleWithOutline(new Rect(-2.4895f, -1.4625f, 4.979f, 2.925f), Color.blue / 4, Color.cyan);
                Handles.color = Color.cyan;
                Handles.DrawSolidRectangleWithOutline(new Rect(-2.4895f, -2.2423f, 4.979f, 0.7423f), Color.blue / 4, Color.cyan);
                //Vector2 size = rect.sizeDelta;
                //Debug.DrawLine(Vector3.zero, Vector3.up);
                Handles.matrix = matrix;
            }
        }
    }
}
