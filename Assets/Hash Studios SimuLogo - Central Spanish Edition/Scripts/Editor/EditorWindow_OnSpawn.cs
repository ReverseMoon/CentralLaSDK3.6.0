using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR

namespace HashStudiosSimuLogo.Scripts
{

    /// <summary>
    /// Copyright (c) 2023 Hash Studios LLC. All rights reserved.
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
    /// 

    [CustomEditor(typeof(U_ShowWorldLogo_OnSpawn_Main))]
    public class EditorWindow_OnSpawn : UnityEditor.Editor
    {
        public Texture2D texture;
        public U_ShowWorldLogo_OnSpawn_Main mainScript;
        private GameObject[] allObjects;
        //public Texture2D hashstudiosllcbanner;
        //public Texture2D backgroundImage;
        public override void OnInspectorGUI()
        {
            mainScript = (U_ShowWorldLogo_OnSpawn_Main)target;

            Color32 hashBlue = new Color32(19, 149, 185, 255);
            Color32 hashBlueBright = new Color32(0, 255, 255, 255);
            Color32 hashBlueFaded = new Color32(209, 19, 13, 30);
            GUI.color = Color.white;

            GUIStyle Title = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 20,
                fontStyle = FontStyle.Bold
            };
            Title.normal.textColor = hashBlue;

            GUIStyle Header = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 16,
                fontStyle = FontStyle.Bold
            };
            Header.normal.textColor = Color.red;

            GUIStyle Header_It = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 16,
                fontStyle = FontStyle.Italic
            };

            GUIStyle LeftHeader = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUIStyle BoldText = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 13,
                fontStyle = FontStyle.Bold
            };
            BoldText.normal.textColor = hashBlue;

            GUIStyle Text = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 12,
                fontStyle = FontStyle.Normal
            };

            GUIStyle SmallText = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 10,
                fontStyle = FontStyle.Normal
            };
            SmallText.normal.textColor = Color.gray;

            GUIStyle SmallTextCenter = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontSize = 10,
                fontStyle = FontStyle.Normal
            };
            SmallTextCenter.normal.textColor = Color.gray;

            //Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 200);

            EditorGUI.DrawPreviewTexture(new Rect(2, 2, EditorGUIUtility.currentViewWidth, EditorGUIUtility.currentViewWidth / 5), texture);
            GUILayout.Label("", GUILayout.Height(EditorGUIUtility.currentViewWidth / 4), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            
            GUILayout.Label("", GUILayout.Height(25), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            
            GUILayout.Label("Hash Studios Simu Logo", Title);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("Central Spanish Edition", Header);
            GUILayout.Label("(On Spawn)", Header_It);
            //GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            /*GUILayout.Label("This prefab presents your world logo before you,", SmallTextCenter);
            GUILayout.Label("either a few seconds after your initial spawn,", SmallTextCenter);
            GUILayout.Label("or when you step into a trigger collider.", SmallTextCenter);
            GUILayout.Label("(Please note, the logo displays only once)", SmallTextCenter);*/
            
            GUILayout.Label("", GUILayout.Height(25), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            
            /*GUILayout.Label("Your Main Logo", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("Make sure to include information in your image", SmallText);
            GUILayout.Label("like the world name and logo.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            mainScript.WorldLogo = (Sprite)EditorGUILayout.ObjectField("Sprite to Use", mainScript.WorldLogo, typeof(Sprite), false, GUILayout.Width(270));

            GUILayout.Label("", GUILayout.Height(75), GUILayout.Width(EditorGUIUtility.currentViewWidth));*/


            /*GUILayout.Label("Your Video To Display", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("Make sure to make this image pretty", SmallText);
            GUILayout.Label("and blurring or fading effects.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            mainScript.Background = (Sprite)EditorGUILayout.ObjectField("Sprite to Use", mainScript.Background, typeof(Sprite), false, GUILayout.Width(270));

            GUILayout.Label("", GUILayout.Height(75), GUILayout.Width(EditorGUIUtility.currentViewWidth));*/


            /*GUILayout.Label("Your Background Logo", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("Make sure to make this image pretty", SmallText);
            GUILayout.Label("and blurring or fading effects.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            mainScript.Background = (Sprite)EditorGUILayout.ObjectField("Sprite to Use", mainScript.Background, typeof(Sprite), false, GUILayout.Width(270));

            GUILayout.Label("", GUILayout.Height(75), GUILayout.Width(EditorGUIUtility.currentViewWidth));*/

            //GUILayout.Label("Time to Display (After Player Spawn)", LeftHeader);
            //GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            //GUILayout.Label("This is the amount of time it will take for the", SmallText);
            //GUILayout.Label("world logo to appear after spawning in-game.", SmallText);
            //GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            //GUILayout.Label("Time to Display After Spawn", GUILayout.Width(200)); 
            ///mainScript.timeToDisplayAfterSpawn = EditorGUILayout.IntField(mainScript.timeToDisplayAfterSpawn, GUILayout.Width(50));

            /*GUILayout.Label("Category 1", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("This is the text that will display on the top left.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));*/

            //GUILayout.Label("Category 1 Label", GUILayout.Width(500));
            // mainScript.Category1_Label = EditorGUILayout.TextField(mainScript.Category1_Label, GUILayout.Width(200));

            //GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            //GUILayout.Label("Category 1 Text", GUILayout.Width(500));
            //mainScript.Category1_Text = EditorGUILayout.TextField(mainScript.Category1_Text, GUILayout.Width(200));


            //GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(EditorGUIUtility.currentViewWidth));


            /*GUILayout.Label("Category 2", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("This is the text that will display on the bottom left.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Label("Category 2 Label", GUILayout.Width(500));
            mainScript.Category2_Label = EditorGUILayout.TextField(mainScript.Category2_Label, GUILayout.Width(200));

            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Label("Category 2 Text", GUILayout.Width(500));
            mainScript.Category2_Text = EditorGUILayout.TextField(mainScript.Category2_Text, GUILayout.Width(200));


            GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(EditorGUIUtility.currentViewWidth));


            GUILayout.Label("Category 3", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("This is the text that will display on the top right.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Label("Category 3 Label", GUILayout.Width(500));
            mainScript.Category3_Label = EditorGUILayout.TextField(mainScript.Category3_Label, GUILayout.Width(200));

            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Label("Category 3 Text", GUILayout.Width(500));
            mainScript.Category3_Text = EditorGUILayout.TextField(mainScript.Category3_Text, GUILayout.Width(200));


            GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(EditorGUIUtility.currentViewWidth));*/


            GUILayout.Label("Display Time", LeftHeader);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));
            GUILayout.Label("This is how long the logo will stay after it appears.", SmallText);
            GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Label("Time to Display Logo", GUILayout.Width(200));
            mainScript.timeToDisplayLogo = EditorGUILayout.FloatField(mainScript.timeToDisplayLogo, GUILayout.Width(50));

            GUILayout.Label("", GUILayout.Height(75), GUILayout.Width(EditorGUIUtility.currentViewWidth));
        }
    }
}

#endif
