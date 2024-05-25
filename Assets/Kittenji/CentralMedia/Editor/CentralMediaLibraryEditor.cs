using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UdonSharpEditor;
using VRC.SDKBase.Editor.BuildPipeline;
using System.Linq;
using System.Reflection;
using TMPro;
using UdonSharp.Video;

namespace Kittenji
{
    [CustomEditor(typeof(CentralMediaLibrary))]
    public class CentralMediaLibraryEditor : UEditor<CentralMediaLibrary>, IVRCSDKBuildRequestedCallback
    {
        SerializedProperty EnableDebug;
        SerializedProperty Animator;
        SerializedProperty Categories;
        // Settings
        SerializedProperty MediaRowCount;
        SerializedProperty ChapterCount;
        SerializedProperty PlayerCooldown;
        // Containers
        SerializedProperty CategoryContainer;
        SerializedProperty MediaContainer;
        SerializedProperty SortingContainer;
        SerializedProperty RatingContainer;
        SerializedProperty GenreContainer;
        SerializedProperty YearContainer;
        SerializedProperty SeasonContainer;
        SerializedProperty ChapterContainer;
        // UI Elements
        SerializedProperty SearchField;
        SerializedProperty PageButtonPrev;
        SerializedProperty PageButtonNext;
        SerializedProperty PageTextLabel;
        SerializedProperty SearchFailed;

        SerializedProperty ChapterPageButtonPrev;
        SerializedProperty ChapterPageButtonNext;
        SerializedProperty ChapterPageLabel;
        // Info Panel
        SerializedProperty InfoPanel;
        SerializedProperty DataPanel; // For Seasonal
        SerializedProperty TitleLabel;
        SerializedProperty DescriptionLabel;
        SerializedProperty RatingLabel;
        SerializedProperty RatingRadial;
        SerializedProperty GenresLabel;
        SerializedProperty DateLabel;
        SerializedProperty ThumbnailImage;
        SerializedProperty PlayLabel;
        // Audio
        SerializedProperty AudioSource;
        SerializedProperty[] AudioClips;
        // Video
        SerializedProperty ControlHandler;
        SerializedProperty URLInputField;

        public override void OnEnable()
        {
            base.OnEnable();

            // Find Serializable AudioClip properties...
            System.Type audioClipType = typeof(AudioClip);
            System.Type targetType = Script.GetType();
            FieldInfo[] properties = targetType.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            List<SerializedProperty> serializedProperties = new List<SerializedProperty>();
            foreach (FieldInfo field in properties)
            {
                if (field.Name.StartsWith("Clip") && field.FieldType.Equals(audioClipType))
                    serializedProperties.Add(serializedObject.FindProperty(field.Name));
            }
            AudioClips = serializedProperties.ToArray();

            serializedObject.Update();
            int i = 0;
            HashSet<Object> PropertyReferences = new HashSet<Object>();
            while (i < Categories.arraySize)
            {
                var categoryProperty = Categories.GetArrayElementAtIndex(i);
                CentralMediaCategory category = categoryProperty.objectReferenceValue as CentralMediaCategory;
                if (category == null || PropertyReferences.Contains(category))
                    Categories.DeleteArrayElementAtIndex(i);
                else
                {
                    if (!category.transform.IsChildOf(transform)) category.transform.SetParent(transform);
                    PropertyReferences.Add(category);
                    i++;
                }
            }

            CentralMediaCategory[] categoryComponents = transform.GetComponentsInChildren<CentralMediaCategory>();
            foreach (CentralMediaCategory category in categoryComponents)
            {
                if (PropertyReferences.Contains(category)) continue;
                PropertyReferences.Add(category);
                int index = Categories.arraySize++;
                SerializedProperty categoryProperty = Categories.GetArrayElementAtIndex(index);
                categoryProperty.objectReferenceValue = category;
            }

            for (i = 0; i < Categories.arraySize; i++)
            {
                SerializedProperty categoryProperty = Categories.GetArrayElementAtIndex(i);
                SerializedObject serializedCategory = new SerializedObject(categoryProperty.objectReferenceValue);
                serializedCategory.Update();
                SerializedProperty relativeLibrary = serializedCategory.FindProperty("MediaLibrary");
                relativeLibrary.objectReferenceValue = target;
                serializedCategory.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private bool ContainerFoldout, InfoPanelFoldout;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool doRepaint = false;
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            serializedObject.Update();

            EditorGUILayout.PropertyField(Animator);
            EditorGUILayout.PropertyField(Categories, new GUIContent("Categorías"));

            if (BeginCachedFoldoutHeaderGroup("UI Elements"))
            {
                EditorGUILayout.PropertyField(SearchField);
                EditorGUILayout.PropertyField(PageButtonPrev);
                EditorGUILayout.PropertyField(PageButtonNext);
                EditorGUILayout.PropertyField(PageTextLabel);
                EditorGUILayout.PropertyField(SearchFailed);
                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(ChapterPageButtonNext);
                EditorGUILayout.PropertyField(ChapterPageButtonPrev);
                EditorGUILayout.PropertyField(ChapterPageLabel);

                ContainerFoldout = EditorGUILayout.Foldout(ContainerFoldout, "Containers");
                if (ContainerFoldout)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(CategoryContainer);
                    EditorGUILayout.PropertyField(MediaContainer);
                    EditorGUILayout.PropertyField(SortingContainer);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(RatingContainer); // Filters
                    EditorGUILayout.PropertyField(GenreContainer);
                    EditorGUILayout.PropertyField(YearContainer);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(SeasonContainer);
                    EditorGUILayout.PropertyField(ChapterContainer);
                    EditorGUI.indentLevel--;
                }

                InfoPanelFoldout = EditorGUILayout.Foldout(InfoPanelFoldout, "Info Panel");
                if (InfoPanelFoldout)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(InfoPanel);
                    EditorGUILayout.PropertyField(DataPanel);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(ThumbnailImage);
                    EditorGUILayout.PropertyField(TitleLabel);
                    EditorGUILayout.PropertyField(DescriptionLabel);
                    EditorGUILayout.PropertyField(RatingLabel);
                    EditorGUILayout.PropertyField(RatingRadial);
                    EditorGUILayout.PropertyField(GenresLabel);
                    EditorGUILayout.PropertyField(DateLabel);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(PlayLabel);
                    EditorGUI.indentLevel--;
                }
            }
            EndCachedFoldoutHeaderGroup();

            if (BeginCachedFoldoutHeaderGroup("Audio"))
            {
                EditorGUILayout.PropertyField(AudioSource);
                foreach (SerializedProperty property in AudioClips)
                {
                    EditorGUILayout.PropertyField(property);
                }
            }
            EndCachedFoldoutHeaderGroup();

            if (BeginCachedFoldoutHeaderGroup("Video"))
            {
                EditorGUILayout.PropertyField(ControlHandler);
                EditorGUILayout.PropertyField(URLInputField);
            }
            EndCachedFoldoutHeaderGroup();

            if (BeginCachedFoldoutHeaderGroup("Settings"))
            {
                EditorGUILayout.PropertyField(MediaRowCount);
                EditorGUILayout.PropertyField(ChapterCount);
                EditorGUILayout.PropertyField(PlayerCooldown);
            }
            EndCachedFoldoutHeaderGroup();

            if (BeginCachedFoldoutHeaderGroup("Debug"))
            {
                EditorGUILayout.PropertyField(EnableDebug);

                if (GUILayout.Button("Send Panel Toggle"))
                {
                    Script.SendCustomEvent(
                        Script.Animator.GetBool("MainPanel") ? "CloseMainPanel" : "OpenMainPanel"
                    );
                }
            }
            EndCachedFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();

            if (doRepaint) Repaint();
        }

        public int callbackOrder => 14;
        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return OnBuildRequestedStatic();
        }
        public static bool OnBuildRequestedStatic()
        {
            CentralMediaLibrary[] centralMediaLibrary = FindObjectsOfType<CentralMediaLibrary>();
            if (centralMediaLibrary.Length == 1)
            {
                CentralMediaLibrary mediaLibrary = centralMediaLibrary[0];

                int[] indexes;
                for (int j = 0, l, i, t; j < mediaLibrary.Categories.Length; j++)
                {
                    CentralMediaCategory category = mediaLibrary.Categories[j];
                    l = category.Length;

                    indexes = new int[l];
                    for (t = 0; t < 3; t++)
                    {
                        for (i = 0; i < l; i++) indexes[i] = i; // Fill with normal ordered indexes
                        switch (t)
                        {
                            case 0:
                                category.SortedRating = indexes.OrderByDescending(ind => category.RatingMatrix[ind]).ToArray();
                                category.SortedRatingAlt = indexes.OrderBy(ind => category.RatingMatrix[ind]).ToArray();
                                break;
                            case 1:
                                category.SortedDate = indexes.OrderByDescending(ind => category.DateMatrix[ind]).ToArray();
                                category.SortedDateAlt = indexes.OrderBy(ind => category.DateMatrix[ind]).ToArray();
                                break;
                            case 2:
                                category.SortedTitle = indexes.OrderBy(ind => category.TitleMatrix[ind]).ToArray();
                                category.SortedTitleAlt = indexes.OrderByDescending(ind => category.TitleMatrix[ind]).ToArray();
                                break;
                        }
                    }

                    // Generate Year List
                    HashSet<short> yearList = new HashSet<short>();
                    for (i = 0; i < l; i++)
                    {
                        short year = category.YearMatrix[i];
                        if (!yearList.Contains(year)) yearList.Add(year);
                    }
                    category.YearList = yearList.OrderByDescending(y => y).ToArray();

                    // Generate Active Genres
                    HashSet<int> activeGenres = new HashSet<int>();
                    for (i = 0; i < l; i++)
                    {
                        int genreFlags = category.GenresMatrix[i];
                        for (t = 0; t < category.GenreNames.Length; t++)
                        {
                            if (FlagsHelper.IsSet(genreFlags, t) && !activeGenres.Contains(t))
                                activeGenres.Add(t);
                        }
                    }
                    category.ActiveGenres = activeGenres.OrderBy(v => v).ToArray();
                }

                // Validate Sensors
                CentralMediaSensor[] sensors = FindObjectsOfType<CentralMediaSensor>();
                if (sensors == null || sensors.Length == 0)
                {
                    Debug.LogError("No se han econtrado censores para el panel. El menú no podrá ser activado.");
                    return false;
                }

                List<USharpVideoPlayer> videoPlayers = new List<USharpVideoPlayer>();
                foreach (CentralMediaSensor s in sensors)
                {
                    USharpVideoPlayer vp = s.TargetVideoPlayer;
                    int ind = videoPlayers.IndexOf(vp);
                    if (ind < 0)
                    {
                        ind = videoPlayers.Count;
                        videoPlayers.Add(vp);
                    }

                    s.TargetID = ind;
                }

                mediaLibrary.RemoteTimestampLength = videoPlayers.Count;
            }
            else if (centralMediaLibrary.Length > 1)
            {
                Debug.LogError("Se han encontrado mas de 1 componente para (CentralMediaLibrary). Por favor asegúrese de que solo una instancia de este componente existe en la escena actual.");
                return false;
            }

            return true;
        }

        [MenuItem("Tools/Kittenji/Test/Central Media Library Build")]
        public static void RuntimeTest()
        {
            if (!Application.isPlaying) return;

            Debug.Log("Starting Runtime Test");
            OnBuildRequestedStatic();
        }
    }
}