using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace HashStudiosDarkSlider.Scripts{

    /// <summary>
    /// Copyright (c) 2024 Hash Studios LLC. All rights reserved.
    /// This code is the sole property of Hash Studios LLC and is protected by copyright laws.
    /// It may not be used, reproduced, or distributed without the express written permission of Hash Studios LLC.
    ///
    /// Any unauthorized use of this code is strictly prohibited and may result in legal action.
    ///
    /// This copyright notice must be included in any copies or reproductions of this code.
    ///
    /// NOTE: This copyright notice does not apply to any dependencies or
    /// external libraries used in this code, such as UdonSharp, VRC SDK, or VRChat.
    /// The copyright and licensing terms of these dependencies apply to their respective code.
    /// Hash Studios LLC is not claiming any rights to these dependencies.
    /// </summary>

    [CustomEditor(typeof(AlphamultSlider))]
    public class Editor_HashStudiosMusicPlayer : UnityEditor.Editor
    {
        public Texture2D texture; // Replace with your actual texture
        //private HashStudiosMusicPlayer_Main mainScript;

        private GUIStyle sectionStyle = new GUIStyle();

        public void OnEnable()
        {
            
        }

        override public void OnInspectorGUI()
        {
            Color32 hashGrey = new Color32(190, 190, 190, 255);
            Color32 hashBlue = new Color32(21, 146, 163, 255);
            GUI.color = Color.white;
            // Define GUI styles
            GUIStyle Title = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold
            };
            //Title.normal.textColor = hashBlue;
            GUIStyle Header = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                fontStyle = FontStyle.Bold
            };
            GUIStyle Header_It = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                fontStyle = FontStyle.Italic
            };
            GUIStyle LeftHeader = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };
            //LeftHeader.normal.textColor = Color.white;
            GUIStyle BoldText = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11,
                fontStyle = FontStyle.Bold
            };
            BoldText.normal.textColor = hashBlue;
            GUIStyle Text = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11,
                fontStyle = FontStyle.Normal
            };
            GUIStyle SmallText = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 10,
                fontStyle = FontStyle.Normal
            };
            SmallText.normal.textColor = Color.gray;
            GUIStyle SmallRedText = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 10,
                fontStyle = FontStyle.Normal
            };
            SmallRedText.normal.textColor = new Color32(200, 120, 120, 255);
            GUIStyle SmallTextCenter = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                fontStyle = FontStyle.Normal
            };
            SmallTextCenter.normal.textColor = hashGrey;

            // Draw preview texture
            EditorGUI.DrawPreviewTexture(new Rect(2, 2, EditorGUIUtility.currentViewWidth, EditorGUIUtility.currentViewWidth / 5), texture);
            GUILayout.Label("", GUILayout.Height(EditorGUIUtility.currentViewWidth / 5), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            // Draw title
            GUILayout.Label("", GUILayout.Height(15), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Label("Modified Darkness Slider", Header, GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("", GUILayout.Height(5), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            // Draw tool description
            GUILayout.Label("By Hash Studios", SmallTextCenter, GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            base.DrawDefaultInspector();
        }
    }
}

#endif