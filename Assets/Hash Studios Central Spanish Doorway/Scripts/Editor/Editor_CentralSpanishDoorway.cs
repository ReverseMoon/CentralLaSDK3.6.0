using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR

[CustomEditor(typeof(U_HashStudiosCentralSpanishDoor_Main))]
public class Editor_CentralSpanishDoorway : UnityEditor.Editor
{
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
    
    public Texture2D texture; // Replace with your actual texture
    private U_HashStudiosCentralSpanishDoor_Main mainScript;
    bool isFoldoutOpen = false;

    private GUIStyle sectionStyle = new GUIStyle();

    public void OnEnable()
    {
        mainScript = (U_HashStudiosCentralSpanishDoor_Main)target;
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
            alignment = TextAnchor.MiddleCenter,
            fontSize = 10,
            fontStyle = FontStyle.Normal
        };
        SmallRedText.normal.textColor = new Color32(255, 50, 50, 255);
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

        GUILayout.Label("Hash Studios Central Spanish Doorway", Header, GUILayout.Width(EditorGUIUtility.currentViewWidth));
        GUILayout.Label("", GUILayout.Height(5), GUILayout.Width(EditorGUIUtility.currentViewWidth));
        // Draw tool description
        GUILayout.Label("Our customer doorway!", SmallTextCenter, GUILayout.Width(EditorGUIUtility.currentViewWidth));
        GUILayout.Label("La puerta de nuestro cliente!", SmallTextCenter, GUILayout.Width(EditorGUIUtility.currentViewWidth));
        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));
        GUILayout.Label("(DO NOT SHARE, NOT FOR RESALE OR SHARING)", SmallRedText, GUILayout.Width(EditorGUIUtility.currentViewWidth));
        GUILayout.Label("(NO COMPARTIR, NO PARA REVENTA O COMPARTIR)", SmallRedText, GUILayout.Width(EditorGUIUtility.currentViewWidth));
        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.Label("Overall / En general", BoldText, GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Room Name / Nombre de la Sala", SmallTextCenter, GUILayout.Width(300));
        mainScript.roomName = EditorGUILayout.TextField(mainScript.roomName, GUILayout.Width(150));
        GUILayout.EndHorizontal();
        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.Label("Camera 1 - OUTSIDE / Cámara 1 - EXTERIOR", BoldText, GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.BeginHorizontal();
        GUILayout.Label("UI Screen (SELF) / Pantalla de UI (PROPIO)", Text, GUILayout.Width(300));
        mainScript.camera1_uiImage = (Image)EditorGUILayout.ObjectField(mainScript.camera1_uiImage, typeof(Image), true, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("UI Screen (AWAY) / Pantalla de UI (AUSENTE)", Text, GUILayout.Width(300));
        mainScript.camera1_uiImage_outdoor = (Image)EditorGUILayout.ObjectField(mainScript.camera1_uiImage_outdoor, typeof(Image), true, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Camera 1", Text, GUILayout.Width(300));
        mainScript.camera1 = (Camera)EditorGUILayout.ObjectField(mainScript.camera1, typeof(Camera), GUILayout.Width(150));
        GUILayout.EndHorizontal();
        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.Label("Camera 2 - INSIDE / Cámara 2 - INTERIOR", BoldText, GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.BeginHorizontal();
        GUILayout.Label("UI Screen (SELF) / Pantalla de UI (PROPIO)", Text, GUILayout.Width(300));
        mainScript.camera2_uiImage = (Image)EditorGUILayout.ObjectField(mainScript.camera2_uiImage, typeof(Image), true, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("UI Screen (AWAY) / Pantalla de UI (AUSENTE)", Text, GUILayout.Width(300));
        mainScript.camera2_uiImage_outdoor = (Image)EditorGUILayout.ObjectField(mainScript.camera2_uiImage_outdoor, typeof(Image), true, GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Camera 2", Text, GUILayout.Width(300));
        mainScript.camera2 = (Camera)EditorGUILayout.ObjectField(mainScript.camera2, typeof(Camera), GUILayout.Width(150));
        GUILayout.EndHorizontal();
        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.Label("VIP LIST / LISTA VIP", BoldText, GUILayout.Width(EditorGUIUtility.currentViewWidth));

        GUILayout.BeginHorizontal();
        GUILayout.Label("¿Vip Habilitado?", Text, GUILayout.Width(300));
        mainScript.isVIP = EditorGUILayout.Toggle(mainScript.isVIP, GUILayout.Width(150));
        GUILayout.EndHorizontal();
        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        if(mainScript.isVIP == true)
        {
            GUILayout.Label("Vip Members List / Lista de Miembros Vip", BoldText, GUILayout.Width(150));

            string foldoutText = isFoldoutOpen ? "Hide List / Ocultar Lista" : "Show List / Mostrar Lista";

            // Create custom foldout button with text
            if (GUILayout.Button(foldoutText, GUILayout.Width(265)))
            {
                isFoldoutOpen = !isFoldoutOpen;
            }

            // Content displayed only when the foldout is open
            if (isFoldoutOpen)
            {
                // ... add GUI elements here that belong to the group ...
                // Get the minimum length of all arrays
                int minLength = mainScript.vipMembers.Length;
                //GUILayout.Label("", GUILayout.Height(10), GUILayout.Width(EditorGUIUtility.currentViewWidth));

                // Loop through each index
                for (int i = 0; i < minLength; i++)
                {
                    GUILayout.Label("", GUILayout.Height(40), GUILayout.Width(EditorGUIUtility.currentViewWidth));
                    //GUILayout.Label("Object " + (i + 1), SmallText);

                    GUILayout.BeginVertical();

                    EditorGUILayout.LabelField("Player " + i, BoldText, GUILayout.Width(200));

                    // Display and edit elements
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Element Index: ", GUILayout.Width(200));
                    EditorGUILayout.LabelField("Índice de Elemento", SmallText, GUILayout.Width(200));
                    EditorGUILayout.LabelField("" + i, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player Name:", GUILayout.Width(200));
                    EditorGUILayout.LabelField("Nombre del Jugador", SmallText, GUILayout.Width(200));
                    mainScript.vipMembers[i] = EditorGUILayout.TextField(mainScript.vipMembers[i], GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Remove / Eliminar", GUILayout.Width(200)))
                    {
                        RemoveElementAtIndex(i);
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.Label("", GUILayout.Height(50), GUILayout.Width(EditorGUIUtility.currentViewWidth));

                GUILayout.BeginHorizontal();

                // Add button to add a new element
                if (GUILayout.Button("Agregar Nuevo Elemento", GUILayout.Width(150)))
                {
                    AddNewElement();
                }

                // Add button to remove the last element
                if (minLength > 0 && GUILayout.Button("Eliminar Último Elemento", GUILayout.Width(150)))
                {
                    RemoveLastElement();
                }

                GUILayout.EndHorizontal();

                GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

            }

            GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));
        }

        //GUILayout.Label("Setup Render Texture and Material");

        /*if (GUILayout.Button("Generate Render Texture & Apply to Camera 1 and UI"))
        {
            if (mainScript.camera1 == null || mainScript.camera1_uiImage == null || string.IsNullOrEmpty(mainScript.roomName))
            {
                Debug.LogError("Camera 1, Camera 1 UI Image, or Camera 1 Room Name not provided!");
                return;
            }

            string roomName = mainScript.roomName;
            string renderTextureName = "GeneratedRenderTexture_" + roomName;
            string renderTexturePath = "Assets/Hash Studios Central Spanish Doorway/GeneratedTextures/" + renderTextureName + ".renderTexture";

            // Check and delete existing Render Texture
            RenderTexture existingRenderTexture = AssetDatabase.LoadAssetAtPath<RenderTexture>(renderTexturePath);
            if (existingRenderTexture != null)
            {
                AssetDatabase.DeleteAsset(renderTexturePath);
            }

            // Create new Render Texture
            RenderTexture newRenderTexture = new RenderTexture(256, 256, 24);
            newRenderTexture.name = renderTextureName;
            AssetDatabase.CreateAsset(newRenderTexture, renderTexturePath);

            // Assign Render Texture to Camera 1
            mainScript.camera1.targetTexture = newRenderTexture;

            // Check and delete existing UI Material for Camera 1
            string uiMaterialPath = "Assets/Hash Studios Central Spanish Doorway/GeneratedTextures/UIMaterial_" + roomName + ".mat";
            Material existingUIMaterial = AssetDatabase.LoadAssetAtPath<Material>(uiMaterialPath);
            if (existingUIMaterial != null)
            {
                AssetDatabase.DeleteAsset(uiMaterialPath);
            }

            // Create new UI Material for Camera 1
            Material newUIMaterial = new Material(Shader.Find("UI/Default"));
            newUIMaterial.name = "UIMaterial_" + roomName;
            newUIMaterial.SetTexture("_MainTex", newRenderTexture);
            AssetDatabase.CreateAsset(newUIMaterial, uiMaterialPath);

            // Apply the UI Material to Camera 1 UI Image
            mainScript.camera1_uiImage.material = newUIMaterial;

            // Save the modified material
            EditorUtility.SetDirty(newUIMaterial);
            AssetDatabase.SaveAssets();

            // Focus the project window and select the new UI Material for Camera 1
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newUIMaterial;
        }*/

        if (GUILayout.Button("Generar Textura de Renderizado y Aplicar a Cámara 1 & 2 y UI"))
        {
            if (mainScript.camera1 == null || mainScript.camera2 == null ||
    mainScript.camera1_uiImage == null || mainScript.camera1_uiImage_outdoor == null ||
    mainScript.camera2_uiImage == null || mainScript.camera2_uiImage_outdoor == null ||
    string.IsNullOrEmpty(mainScript.roomName))
            {
                Debug.LogError("HSCS: Required components or Room Name not provided!");
                return;
            }

            string roomName = mainScript.roomName.Trim();
            string directoryPath = $"Assets/Hash Studios Central Spanish Doorway/GeneratedTextures/{roomName}";

            // Ensure the directory for the room exists
            if (!AssetDatabase.IsValidFolder(directoryPath))
            {
                string parentPath = "Assets/Hash Studios Central Spanish Doorway/GeneratedTextures";
                AssetDatabase.CreateFolder(parentPath, roomName);
            }

            // Generate and assign Render Textures
            GenerateAndAssignRenderTexture(mainScript.camera1, "camera1", directoryPath, mainScript.camera1_uiImage, mainScript.camera2_uiImage_outdoor);
            GenerateAndAssignRenderTexture(mainScript.camera2, "camera2", directoryPath, mainScript.camera2_uiImage, mainScript.camera1_uiImage_outdoor);

            // Focus the project window on the last generated Render Texture
            EditorUtility.FocusProjectWindow();
        }

        GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        //GUILayout.Label("Other Door", BoldText, GUILayout.Width(EditorGUIUtility.currentViewWidth));

        //GUILayout.Label("", GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));

        //base.DrawDefaultInspector();
    }

    void GenerateAndAssignRenderTexture(Camera camera, string cameraName, string directoryPath, Image selfUIImage, Image outdoorUIImage)
    {
        string roomName = mainScript.roomName.Trim();
        string renderTextureName = $"GeneratedRenderTexture_{cameraName}_{roomName}";
        string renderTexturePath = $"{directoryPath}/{renderTextureName}.renderTexture";

        // Check and delete existing Render Texture
        RenderTexture existingRenderTexture = AssetDatabase.LoadAssetAtPath<RenderTexture>(renderTexturePath);
        if (existingRenderTexture != null)
        {
            AssetDatabase.DeleteAsset(renderTexturePath);
        }

        // Create a new Render Texture
        RenderTexture newRenderTexture = new RenderTexture(256, 256, 24);
        newRenderTexture.name = renderTextureName;
        AssetDatabase.CreateAsset(newRenderTexture, renderTexturePath);
        camera.targetTexture = newRenderTexture;

        // For SELF UI Image
        string selfMaterialName = $"Material_{cameraName}_SELF_{roomName}";
        string selfMaterialPath = $"{directoryPath}/{selfMaterialName}.mat";
        GenerateAndApplyMaterial(selfUIImage, newRenderTexture, selfMaterialName, selfMaterialPath);

        // For OUTDOOR UI Image (using the other camera's Render Texture)
        string outdoorMaterialName = $"Material_{cameraName}_OUTDOOR_{roomName}";
        string outdoorMaterialPath = $"{directoryPath}/{outdoorMaterialName}.mat";
        GenerateAndApplyMaterial(outdoorUIImage, newRenderTexture, outdoorMaterialName, outdoorMaterialPath);
    }

    void GenerateAndApplyMaterial(Image uiImage, RenderTexture renderTexture, string materialName, string materialPath)
    {
        // Check and delete existing Material
        Material existingMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        if (existingMaterial != null)
        {
            AssetDatabase.DeleteAsset(materialPath);
        }

        // Create a new Material
        Material newMaterial = new Material(Shader.Find("UI/Default"));
        newMaterial.name = materialName;
        newMaterial.SetTexture("_MainTex", renderTexture);
        AssetDatabase.CreateAsset(newMaterial, materialPath);

        // Apply the new Material to the UI Image
        uiImage.material = newMaterial;
    }

    private void RemoveElementAtIndex(int index)
    {
        if (index >= 0 && index < mainScript.vipMembers.Length)
        {
            // Shift elements to the left after removing the desired element
            for (int i = index; i < mainScript.vipMembers.Length - 1; i++)
            {
                mainScript.vipMembers[i] = mainScript.vipMembers[i + 1];
            }

            // Decrement the size of all arrays after shifting
            Array.Resize(ref mainScript.vipMembers, mainScript.vipMembers.Length - 1);

            // Mark the changes for serialization
            EditorUtility.SetDirty(mainScript);
        }
    }

    private void AddNewElement()
    {
        // Increase the size of all arrays
        Array.Resize(ref mainScript.vipMembers, mainScript.vipMembers.Length + 1);

        // Assign default values to the newly added element
        mainScript.vipMembers[mainScript.vipMembers.Length - 1] = "";

        // Mark the changes for serialization
        EditorUtility.SetDirty(mainScript);
    }

    private void RemoveLastElement()
    {
        if (mainScript.vipMembers.Length > 0)
        {
            // Decrement the size of all arrays
            Array.Resize(ref mainScript.vipMembers, mainScript.vipMembers.Length - 1);

            // Mark the changes for serialization
            EditorUtility.SetDirty(mainScript);
        }
    }
}

#endif