using UnityEditor;
using UdonSharpEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using UnityEngine;
using static Kittenji.TMDBApi;
using VRC.SDKBase;
using System.Text;
using System.Reflection;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading;
using UdonSharp.Video;

namespace Kittenji
{
    [CustomEditor(typeof(CentralMediaCategory))]
    public class CentralMediaCategoryEditor : UEditor<CentralMediaCategory>
    {
        #region Initial Serialized Properties
        SerializedProperty MediaLibrary;
        SerializedObject SerializedMediaLibrary;

        SerializedProperty Name;
        SerializedProperty Icon;
        SerializedProperty Seasonal;

        SerializedProperty GenreCustoms;
        SerializedProperty GenreNames;
        SerializedProperty GenreIDs;

        SerializedProperty TitleMatrix;
        SerializedProperty DescriptionMatrix;
        SerializedProperty GenresMatrix;
        SerializedProperty RatingMatrix;
        SerializedProperty YearMatrix;
        SerializedProperty DateMatrix;
        SerializedProperty AddedMatrix;
        SerializedProperty ImageMatrix;
        SerializedProperty LinkMatrix;
        SerializedProperty SeasonMatrix;

        [Header("Additional Section HS")]
        private int num;
        private int startingNum;
        private string str;
        private int numberToAdd;
        private bool showAlternative = false;

        // Editor Only
        SerializedProperty IndexFoldout, SeasonFoldout;

        const int MaxItemPerGroup = 24;
        private Regex LastNumRegex = new Regex(@"(\d+)(?=\.)");

        public SeasonProxyTemp SeasonProxy = new SeasonProxyTemp();
        public MediaImporterTemp MediaImporter = new MediaImporterTemp();

        public bool IsSeasonal => Seasonal.boolValue;
        public int GetGenreIndexByID(int id)
        {
            for (int i = 0; i < GenreIDs.arraySize; i++)
                if (GenreIDs.GetArrayElementAtIndex(i).intValue == id) return i;
            return -1;
        }

        public override void OnEnable()
        {
            num = -1;
            startingNum = -1;
            str = "";
            numberToAdd = 0;

            base.OnEnable();
            if (SeasonProxy == null) SeasonProxy = new SeasonProxyTemp();

            Undo.undoRedoPerformed += ImportProxy;
            ImportProxy();
        }
        private void OnDisable()
        {
            Undo.undoRedoPerformed -= ImportProxy;
        }
        private void OnDestroy()
        {
            Undo.undoRedoPerformed -= ImportProxy;
        }

        public void ImportProxy()
        {
            SeasonProxy.Import(SeasonMatrix, LinkMatrix);
            SeasonProxy.IsDirty = false;
            Repaint();
        }

        public void TrySerializeMediaLibrary(bool findOnScene = true)
        {
            serializedObject.Update();
            for (int i = 0; i < 2; i++)
            {
                if (MediaLibrary.objectReferenceValue == null)
                    MediaLibrary.objectReferenceValue = i == 0 ? Script.transform.GetComponentInParent<CentralMediaLibrary>() : FindObjectOfType<CentralMediaLibrary>();
            }
            serializedObject.ApplyModifiedProperties();

            if (MediaLibrary.objectReferenceValue != null)
            {
                SerializedMediaLibrary = new SerializedObject(MediaLibrary.objectReferenceValue);
                SerializedMediaLibrary.Update();
                SerializedProperty categories = SerializedMediaLibrary.FindProperty("Categories");
                for (int i = 0; i < categories.arraySize; i++)
                {
                    SerializedProperty element = categories.GetArrayElementAtIndex(i);
                    if (element.objectReferenceValue != null && element.objectReferenceValue.Equals(target)) return;
                }

                int size = categories.arraySize++;
                categories.GetArrayElementAtIndex(size).objectReferenceValue = target;
                SerializedMediaLibrary.ApplyModifiedProperties();
            }
        }
        #endregion

        private bool ContentFoldout = true;
        private bool ImportFoldout = true;
        // private Vector2 ContentScroll;
        private bool GenreListFoldout;

        private string FilterTextField = string.Empty;
        private TMDBApi.Result[] SearchResults;

        private int Group;

        public override void OnInspectorGUI()
        {

            bool doRepaint = false;
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("No es recomendado editar las propiedades de este componente en runtime.", MessageType.Warning);
                return;
            }

            serializedObject.Update();

            GUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(MediaLibrary, new GUIContent("Librería"));
            if (MediaLibrary.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("No se ha asignado una referencia a la librería principal o la librería principal no existe en la escena.", MessageType.Error);
                GUILayout.EndVertical();
                serializedObject.ApplyModifiedProperties();
                return;
            }
            else if (!Script.transform.IsChildOf(((CentralMediaLibrary)MediaLibrary.objectReferenceValue).transform))
                EditorGUILayout.HelpBox("La librería principal no es pariente directo de este objeto.", MessageType.Warning);

            GUILayout.Space(4);
            EditorGUILayout.PropertyField(Name, new GUIContent("Nombre"));
            EditorGUILayout.PropertyField(Icon, new GUIContent("Icono"));
            GUI.enabled = TitleMatrix.arraySize == 0;
            EditorGUILayout.PropertyField(Seasonal, new GUIContent("Seasonal"));
            GUI.enabled = true;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("The Movie Database", EditorStyles.centeredGreyMiniLabel);
            bool isNull = string.IsNullOrEmpty(TMDBApi.ApiKey);
            if (isNull) EditorGUILayout.HelpBox("No es posible buscar información en TMDB porque no se ha especificado una clave para la API.\nVisita \"themoviedb.org/settings/api\" para configurar tu clave.", MessageType.Error);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = !isNull && !GenreCustoms.boolValue;
            if (GUILayout.Button("↶ Géneros", GUILayout.Width(100)))
            {
                Genre[] genreList = TMDBApi.GetGenreList(!Seasonal.boolValue);
                if (genreList.Length != GenreIDs.arraySize)
                {
                    GenreIDs.ClearArray();
                    GenreNames.ClearArray();
                }

                for (int i = 0; i < genreList.Length; i++)
                {
                    var genre = genreList[i];
                    string gName = genre.name;
                    int gID = genre.id;

                    bool contained = GenreIDs.ContainsArrayElement(gID);
                    if (contained) continue;

                    int index = GenreIDs.arraySize++;
                    GenreNames.arraySize = GenreIDs.arraySize;

                    GenreIDs.GetArrayElementAtIndex(index).intValue = gID;
                    GenreNames.GetArrayElementAtIndex(index).stringValue = gName;
                }
            }
            GUI.enabled = true;
            TMDBApi.ApiKey = EditorGUILayout.PasswordField(TMDBApi.ApiKey);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(GenreCustoms, new GUIContent("Custom"));
            if (!GenreCustoms.boolValue)
            {
                GenreListFoldout = EditorGUILayout.Foldout(GenreListFoldout, "Géneros");
                if (GenreListFoldout)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < GenreIDs.arraySize; i++)
                    {
                        int id = GenreIDs.GetArrayElementAtIndex(i).intValue;
                        EditorGUILayout.PropertyField(GenreNames.GetArrayElementAtIndex(i), new GUIContent(id.ToString()));
                    }
                    EditorGUI.indentLevel--;
                }
            } else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(GenreNames, new GUIContent("Géneros"));
                if (EditorGUI.EndChangeCheck())
                {
                    if (GenreIDs.arraySize != GenreNames.arraySize)
                        GenreIDs.arraySize = GenreNames.arraySize;
                    Debug.Log("Changed array stuff");
                }
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            GUILayout.EndVertical();

            if (TitleMatrix.arraySize == 0 && ((!IsSeasonal && MediaImporter.CanImportInfo) || (IsSeasonal && MediaImporter.CanImportSeri)))
            {
                ImportFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(ImportFoldout, new GUIContent("Importar"));

                if (ImportFoldout)
                {
                    Rect drop_area = EditorGUILayout.BeginVertical();
                    EditorGUILayout.HelpBox("Arrastra aquí el objeto principal que contiene la información.", MessageType.Info);
                    EditorGUILayout.EndVertical();

                    // Drag and drop
                    Event evt = Event.current;

                    switch (evt.type)
                    {
                        case EventType.DragUpdated:
                        case EventType.DragPerform:
                            if (!drop_area.Contains(evt.mousePosition))
                                return;

                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                            if (evt.type == EventType.DragPerform)
                            {
                                DragAndDrop.AcceptDrag();
                                if (DragAndDrop.objectReferences.Length == 1 && DragAndDrop.objectReferences[0] is GameObject)
                                {
                                    GameObject go = DragAndDrop.objectReferences[0] as GameObject;
                                    if (IsSeasonal)
                                    {
                                        // Not implemented yet
                                    } else {
                                        MediaImporterTemp.InfoContent[] contents = MediaImporter.GetOldInfoFrom(go);
                                        serializedObject.Update();
                                        for (int i = 0; i < contents.Length; i++)
                                        {
                                            var c = contents[i];
                                            if (EditorUtility.DisplayCancelableProgressBar("Importando", $"{i}. Importando {c.title}", i / (float)contents.Length))
                                            {
                                                ClearAllEntries();
                                                break;
                                            };
                                            CreateNewEntry();
                                            // Debug.Log("Importing: " + c.title);
                                            int index = TitleMatrix.arraySize - 1;
                                            TitleMatrix.GetArrayElementAtIndex(index).stringValue = c.title;
                                            DescriptionMatrix.GetArrayElementAtIndex(index).stringValue = c.description;
                                            YearMatrix.GetArrayElementAtIndex(index).intValue = c.year_num;
                                            DateMatrix.GetArrayElementAtIndex(index).longValue = DateTimeToUnixTimeStamp(new DateTime(c.year_num, 1, 1));
                                            LinkMatrix.GetArrayElementAtIndex(index).FindPropertyRelative("url").stringValue = c.urls.FirstOrDefault().ToString();
                                            long tstamp = DateTimeToUnixTimeStamp(DateTime.Now);
                                            AddedMatrix.GetArrayElementAtIndex(index).longValue = tstamp + 1;

                                            int genreFlags = 0;
                                            Debug.Log(c.genres.Length);
                                            for (int j = 0; j < Script.GenreNames.Length; j++)
                                            {
                                                string genre = Script.GenreNames[j].ToLowerInvariant();
                                                if (c.genres.Any(g => g.Equals(genre)))
                                                    FlagsHelper.Set(ref genreFlags, j);
                                            }

                                            GenresMatrix.GetArrayElementAtIndex(index).intValue = genreFlags;
                                            ImageMatrix.GetArrayElementAtIndex(index).objectReferenceValue = c.image;
                                        }
                                        serializedObject.ApplyModifiedPropertiesWithoutUndo();
                                        EditorGUILayout.EndFoldoutHeaderGroup();
                                        EditorUtility.ClearProgressBar();
                                        return;
                                    }
                                }
                            }
                            break;
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }


            GUILayout.BeginHorizontal();
            ContentFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(ContentFoldout, new GUIContent($"Contenido ({TitleMatrix.arraySize})"));
            GUI.enabled = TitleMatrix.arraySize > 0;
            if (ContentFoldout && GUILayout.Button("Suprimir") &&
                EditorUtility.DisplayDialog("¡Espere un momento!", "Esta acción borrará todo el contenido guardado en esta categoría.\n\n¿Está completamente seguro de que desea continuar?", "Si", "Cancelar") &&
                (TitleMatrix.arraySize < 5 || EditorUtility.DisplayDialog($"Borrar contenido.", $"Se borrarán {TitleMatrix.arraySize} entradas contenidas dentro de esta categoría.", "Ok", "Cancelar")))
            {
                ClearAllEntries();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();


            if (ContentFoldout)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                FilterTextField = EditorGUILayout.TextField("Filtrar", FilterTextField);

                GUILayout.EndVertical();
                int arraySize = TitleMatrix.arraySize;
                int padding = arraySize.ToString().Length;
                bool foldout;
                string filterLower = FilterTextField.ToLowerInvariant();

                int maxGroup = (arraySize / MaxItemPerGroup);
                int group = Group;
                int start = MaxItemPerGroup * group;
                int end = Mathf.Min(start + MaxItemPerGroup, arraySize);

                for (int i = start; i < end; i++)
                {
                    SerializedProperty titleProperty = TitleMatrix.GetArrayElementAtIndex(i);
                    SerializedProperty ratingProperty = RatingMatrix.GetArrayElementAtIndex(i);
                    if (filterLower.StartsWith(">"))
                    {
                        if (ratingProperty.floatValue > 0.0001f) continue;
                    }
                    else if (!string.IsNullOrEmpty(FilterTextField) && !titleProperty.stringValue.ToLowerInvariant().Contains(filterLower)) continue;
                    GUI.color = ratingProperty.floatValue > 0.0001f ? Color.white : Color.red;
                    GUILayout.BeginVertical("Box");
                    GUI.color = Color.white;

                    GUILayout.BeginHorizontal();
                    EditorGUI.BeginChangeCheck();
                    foldout = IndexFoldout.intValue == i;
                    string id = (i + 1).ToString().PadLeft(padding, '0');
                    EditorGUI.indentLevel++;
                    foldout = EditorGUILayout.Foldout(foldout, string.IsNullOrEmpty(titleProperty.stringValue) ? "...¡Vacío!..." : titleProperty.stringValue, foldout ? UStyles.FoldoutBold : EditorStyles.foldout);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.LabelField(id, UStyles.MiniLabelRight);
                    if (EditorGUI.EndChangeCheck())
                    {
                        IndexFoldout.intValue = foldout ? i : -1;
                        SearchResults = null;
                    }
                    GUILayout.EndHorizontal();

                    if (foldout)
                    {
                        SerializedProperty descriptionProperty = DescriptionMatrix.GetArrayElementAtIndex(i);
                        SerializedProperty genresProperty = GenresMatrix.GetArrayElementAtIndex(i);
                        SerializedProperty yearProperty = YearMatrix.GetArrayElementAtIndex(i);
                        SerializedProperty dateProperty = DateMatrix.GetArrayElementAtIndex(i);
                        SerializedProperty addedProperty = AddedMatrix.GetArrayElementAtIndex(i);
                        SerializedProperty imageProperty = ImageMatrix.GetArrayElementAtIndex(i);

                        GUILayout.BeginHorizontal();
                        EditorGUI.BeginChangeCheck();
                        UnityEngine.Object imageObject = EditorGUILayout.ObjectField(imageProperty.objectReferenceValue, typeof(Texture2D), false, GUILayout.Width(60), GUILayout.Height(80));
                        if (EditorGUI.EndChangeCheck())
                        {
                            imageProperty.objectReferenceValue = imageObject;
                            if (imageObject != null)
                            {
                                string assetPath = AssetDatabase.GetAssetPath(imageObject);
                                string imageDestinationFolder = Path.Combine(ScriptFolder, "Posters", "Custom");
                                if (!Directory.Exists(imageDestinationFolder)) Directory.CreateDirectory(imageDestinationFolder);

                                string path1 = Path.GetFullPath(imageDestinationFolder).TrimEnd('\\');
                                string path2 = Path.GetFullPath(Path.GetDirectoryName(assetPath)).TrimEnd('\\');
                                int comp = String.Compare(path1, path2, StringComparison.InvariantCultureIgnoreCase);
                                if (comp != 0)
                                {
                                    int indexOf = Path.GetFullPath(Path.GetDirectoryName(Application.dataPath)).TrimEnd('\\').Length;
                                    string assetDest = Path.Combine(path1.Substring(indexOf + 1), Path.GetFileName(assetPath));
                                    Debug.Log(assetPath);
                                    Debug.Log(assetDest);
                                    AssetDatabase.MoveAsset(assetPath, assetDest);
                                    AssetDatabase.Refresh();
                                }

                                ApplyTextureSettings((Texture2D)imageObject);
                            }
                        }
                        //EditorGUILayout.PropertyField(imageProperty, GUIContent.none, GUILayout.Width(90), GUILayout.Height(100));
                        GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(titleProperty, GUIContent.none);
                        if (GUILayout.Button("✕", GUILayout.Width(24)))
                        {
                            DeleteEntryAt(i);
                            IndexFoldout.intValue = -1;
                            doRepaint = true;
                            break;
                        }
                        GUILayout.EndHorizontal();
                        EditorGUI.BeginChangeCheck();
                        string descriptionString = EditorGUILayout.TextArea(descriptionProperty.stringValue, UStyles.WrappedTextArea, GUILayout.Height(58));
                        if (EditorGUI.EndChangeCheck()) descriptionProperty.stringValue = descriptionString.Trim();
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        string[] genreList = Script.GenreNames;
                        int genresValue = genresProperty.intValue;
                        if (genreList == null || genreList.Length == 0)
                        {
                            EditorGUILayout.HelpBox("No se ha definido una lista de Géneros en esta categoría.", MessageType.Error);
                        }
                        else
                        {
                            EditorGUI.BeginChangeCheck();
                            try
                            {
                                genresValue = EditorGUILayout.MaskField("Géneros", genresValue, genreList);
                            }
                            catch
                            {
                                GUILayout.BeginVertical(EditorStyles.helpBox);
                                EditorGUILayout.HelpBox("Hubo un error obteniendo la información de géneros.\n¿Desea resetear este valor al predeterminado?", MessageType.Error);
                                if (GUILayout.Button("Resetear Valor")) genresValue = genresProperty.intValue = 0;
                                GUILayout.EndVertical();
                            }
                            if (EditorGUI.EndChangeCheck()) genresProperty.intValue = genresValue;
                        }

                        EditorGUI.BeginChangeCheck();
                        float rating = EditorGUILayout.Slider(new GUIContent("Puntuación"), ratingProperty.floatValue, 0f, 10f);
                        if (EditorGUI.EndChangeCheck()) ratingProperty.floatValue = rating;

                        GUILayout.Space(4);
                        EditorGUI.BeginChangeCheck();
                        (long releaseDate, int releaseYear) = UnixTSField(new GUIContent("Fecha (M/D/A)", yearProperty.intValue.ToString()), dateProperty.longValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            dateProperty.longValue = releaseDate;
                            yearProperty.intValue = releaseYear;
                        }
                        EditorGUI.BeginChangeCheck();
                        (releaseDate, releaseYear) = UnixTSField(new GUIContent("Agregado (M/D/A)"), addedProperty.longValue);
                        if (EditorGUI.EndChangeCheck()) addedProperty.longValue = releaseDate;
                        GUILayout.Space(4);

                        if (IsSeasonal)
                        {
                            EditorGUI.indentLevel++;

                            EditorGUILayout.BeginVertical("box");
                            EditorGUILayout.BeginVertical("box");
                            List<List<string>> seasonData = SeasonProxy.Matrix[i];
                            for (int j = 0; j < seasonData.Count; j++) // Iterate trough all seasons
                            {
                                EditorGUI.BeginChangeCheck();
                                Rect drop_area = EditorGUILayout.BeginHorizontal();
                                bool seasonFoldout = EditorGUILayout.Foldout(SeasonFoldout.intValue == j, $"Temporada {j+1}");
                                List<string> seasonUrls = seasonData[j];
                                EditorGUILayout.LabelField($"{seasonUrls.Count} Capítulos", UStyles.MiniLabelRight);
                                if (GUILayout.Button("-", GUILayout.Width(24)) &&
                                   (seasonUrls.Count == 0 || EditorUtility.DisplayDialog("Borrar Temporada", $"Estás a punto de borrar una temporada completa con {seasonUrls.Count} capítulos.\n¿Seguro que desea proceder?", "Si", "Cancelar")))
                                {
                                    seasonData.RemoveAt(j);
                                    SeasonProxy.MarkDirty();
                                    doRepaint = true;
                                    break;
                                };
                                EditorGUILayout.EndHorizontal();

                                if (MediaImporter.CanImportSeri)
                                {
                                    // Import seri drop
                                    // Drag and drop
                                    Event evt = Event.current;

                                    switch (evt.type)
                                    {
                                        case EventType.DragUpdated:
                                        case EventType.DragPerform:
                                            if (!drop_area.Contains(evt.mousePosition))
                                                break;

                                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                                            if (evt.type == EventType.DragPerform)
                                            {
                                                DragAndDrop.AcceptDrag();
                                                if (DragAndDrop.objectReferences.Length == 1 && DragAndDrop.objectReferences[0] is GameObject)
                                                {
                                                    GameObject go = DragAndDrop.objectReferences[0] as GameObject;

                                                    MediaImporterTemp.InfoContent contents = MediaImporter.GetOldSeriInfoFrom(go);
                                                    if (contents.urls != null && contents.urls.Length > 0)
                                                    {
                                                        for (int t = 0; t < contents.urls.Length; t++)
                                                        {
                                                            seasonUrls.Add(contents.urls[t].Get());
                                                        }
                                                        SeasonProxy.MarkDirty();
                                                        seasonFoldout = false;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }

                                if (EditorGUI.EndChangeCheck()) SeasonFoldout.intValue = seasonFoldout ? j : -1;
                                if (!seasonFoldout) continue;

                                for (int t = 0; t < seasonUrls.Count; t++)
                                {
                                    string url = seasonUrls[t];
                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.PrefixLabel(string.Empty);
                                    if (GUILayout.Button("-", GUILayout.Height(16), GUILayout.Width(16)))
                                    {
                                        seasonUrls.RemoveAt(t);
                                        SeasonProxy.MarkDirty();
                                        doRepaint = true;
                                        break;
                                    }
                                    url = EditorGUILayout.TextField($"Cap {t + 1} ", url);
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        seasonUrls[t] = url;
                                        SeasonProxy.MarkDirty();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }

                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.PrefixLabel(string.Empty);
                                if (GUILayout.Button("+", GUILayout.Height(16)))
                                {
                                    string last = seasonUrls.LastOrDefault() ?? string.Empty;
                                    last = LastNumRegex.Replace(last, (seasonUrls.Count + 1).ToString());

                                    seasonUrls.Add(last);
                                    SeasonProxy.MarkDirty();
                                }
                                EditorGUILayout.EndHorizontal();

                                /// 
                                /// SECTION DEVELOPED BY HASH STUDIOS AS PER CONTRACT
                                /// 

                                EditorGUILayout.BeginHorizontal();
                                numberToAdd = EditorGUILayout.IntField(numberToAdd, GUILayout.Width(200));
                                if (GUILayout.Button("+", GUILayout.Height(16)))
                                {
                                    for(int f = 0; f < numberToAdd; f++){
                                        string last = seasonUrls.LastOrDefault() ?? string.Empty;
                                    last = LastNumRegex.Replace(last, (seasonUrls.Count + 1).ToString());

                                    seasonUrls.Add(last);
                                    SeasonProxy.MarkDirty();
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                ///
                                /// SECTION END
                                ///

                                /// 
                                /// SECTION DEVELOPED BY HASH STUDIOS AS PER CONTRACT
                                /// 
                                if (GUILayout.Button(showAlternative ? "Ocultar alternativa" : "Mostrar alternativa"))
                                {
                                    showAlternative = !showAlternative;
                                }

                                if (showAlternative)
                                {
                                    EditorGUILayout.BeginVertical();

                                    EditorGUILayout.LabelField("--- Añadir enlace personalizado ---");
                                    //EditorGUILayout.LabelField("Add Instructions Here");
                               
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Número inicial", GUILayout.Width(200));
                                    startingNum = EditorGUILayout.IntField(startingNum, GUILayout.Width(200));
                                    EditorGUILayout.EndHorizontal();
                                
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Número de enlaces", GUILayout.Width(200));
                                    num = EditorGUILayout.IntField(num, GUILayout.Width(200));
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Enlace para añadir", GUILayout.Width(200));
                                    str = EditorGUILayout.TextField(str, GUILayout.Width(200));
                                    EditorGUILayout.EndHorizontal();

                                    if(num >= 1 && str.Contains("[i]") && startingNum >= 0){
                                        if (GUILayout.Button("Add " + num, GUILayout.Height(16)))
                                        { 
                                            String temp = "";
                                            for(int f = 0; f < num; f++){
                                                temp = str;
                                                temp = temp.Replace("[i]", "" + startingNum + f);
                                                //last = LastNumRegex.Replace(last.ToString());

                                                seasonUrls.Add(temp);
                                                SeasonProxy.MarkDirty();
                                            }
                                        }
                                    }
                                    EditorGUILayout.LabelField("--------------------");
                                    EditorGUILayout.EndVertical();
                                }
                                ///
                                /// SECTION END
                                ///

                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                            if (GUILayout.Button("+ Temporada", GUILayout.Height(16)))
                            {
                                seasonData.Add(new List<string>());
                                SeasonProxy.MarkDirty();
                                SeasonFoldout.intValue = seasonData.Count - 1;
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndVertical();

                            EditorGUI.indentLevel--;
                        }
                        else
                        {
                            SerializedProperty linkProperty = LinkMatrix.GetArrayElementAtIndex(i);
                            EditorGUILayout.PropertyField(linkProperty, new GUIContent("Link (URL)"));
                        }

                        // The movie database auto entry
                        EditorGUILayout.BeginVertical("box");
                        GUI.enabled = !isNull;
                        if (GUILayout.Button("↶ Buscar")) SearchResults = SearchResults == null ? TMDBApi.SearchMovie(titleProperty.stringValue, !Seasonal.boolValue) : null;
                        GUI.enabled = true;

                        if (SearchResults != null && SearchResults.Length > 0)
                        {
                            for (int j = 0; j < SearchResults.Length; j++)
                            {
                                var result = SearchResults[j];
                                string displayName = result.GetName();
                                string originalDate = result.GetDate();
                                if (GUILayout.Button($"{displayName} ({originalDate})", UStyles.ToolbarButtonLeft))
                                {
                                    FillResultAt(i, result);
                                    SearchResults = null;
                                    doRepaint = true;
                                    break;
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Página {(group + 1)}/{maxGroup + 1}");
                GUI.enabled = Group > 0;
                if (GUILayout.Button("<", GUILayout.Width(54)))
                {
                    Group = Group = Mathf.Max(Group - 1, 0);
                }
                GUI.enabled = Group < maxGroup;
                if (GUILayout.Button(">", GUILayout.Width(54)))
                {
                    Group = Group = Mathf.Min(Group + 1, maxGroup);
                }
                GUI.enabled = true;
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                if (GUILayout.Button("Nueva Entrada"))
                {
                    CreateNewEntry();
                    doRepaint = true;
                }
                GUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (IsSeasonal && SeasonProxy.IsDirty)
            {
                SeasonProxy.Export(SeasonMatrix, LinkMatrix);
                SeasonProxy.ClearDirty();
            }
            serializedObject.ApplyModifiedProperties();
            if (doRepaint) Repaint();

            /*
            if (GUILayout.Button("Test Validation"))
            {
                ValidateEntriesLength();
            }
            */
        }

        public void FillResultAt(int i, Result result)
        {
            string displayName = result.GetName();
            string originalDate = result.GetDate();

            SerializedProperty titleProperty = TitleMatrix.GetArrayElementAtIndex(i);
            SerializedProperty descriptionProperty = DescriptionMatrix.GetArrayElementAtIndex(i);
            SerializedProperty genresProperty = GenresMatrix.GetArrayElementAtIndex(i);
            SerializedProperty ratingProperty = RatingMatrix.GetArrayElementAtIndex(i);
            SerializedProperty yearProperty = YearMatrix.GetArrayElementAtIndex(i);
            SerializedProperty dateProperty = DateMatrix.GetArrayElementAtIndex(i);
            // SerializedProperty addedProperty = AddedMatrix.GetArrayElementAtIndex(i);
            SerializedProperty imageProperty = ImageMatrix.GetArrayElementAtIndex(i);

            titleProperty.stringValue = displayName;
            descriptionProperty.stringValue = result.overview;

            int[] genres = result.genre_ids;
            int genresValue = 0;
            foreach (int gID in genres)
            {
                int index = GetGenreIndexByID(gID);
                if (index < 0) continue;
                FlagsHelper.Set(ref genresValue, index);
            }
            genresProperty.intValue = genresValue;

            ratingProperty.floatValue = result.vote_average;

            string poster_path = result.poster_path.Substring(1);
            string imageDestinationFolder = Path.Combine(ScriptFolder, "Posters");
            if (!Directory.Exists(imageDestinationFolder)) Directory.CreateDirectory(imageDestinationFolder);
            string imageDestinationPath = Path.Combine(imageDestinationFolder, poster_path + ".png");
            Texture2D imageResult;
            if (!File.Exists(imageDestinationPath))
            {
                imageResult = TMDBApi.GetThumbnailFromResult(result);
                byte[] imageBytes = imageResult.EncodeToPNG();
                File.WriteAllBytes(imageDestinationPath, imageBytes);
                AssetDatabase.Refresh();
            }
            imageResult = (Texture2D)AssetDatabase.LoadAssetAtPath(imageDestinationPath, typeof(Texture2D));
            imageProperty.objectReferenceValue = imageResult;
            ApplyTextureSettings(imageResult);

            DateTime releaseDateParsed = DateTime.ParseExact(originalDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            dateProperty.longValue = DateTimeToUnixTimeStamp(releaseDateParsed);
            yearProperty.intValue = releaseDateParsed.Year;

            // SearchResults = null;
        }

        public void ValidateEntriesLength()
        {
            int titleSize = TitleMatrix.arraySize;
            int descSize = DescriptionMatrix.arraySize;
            int genreSize = GenresMatrix.arraySize;
            int ratingSize = RatingMatrix.arraySize;
            int yearSize = YearMatrix.arraySize;
            int dateSize = DateMatrix.arraySize;
            int addedSize = AddedMatrix.arraySize;
            int imageSize = ImageMatrix.arraySize;

            Debug.Log("Checking Sizes:\n" +
                $"Title : {titleSize}\n" +
                $"Desc  : {descSize}\n" +
                $"Genre : {genreSize}\n" +
                $"Rating: {ratingSize}\n" +
                $"Year  : {yearSize}\n" +
                $"Date  : {dateSize}\n" +
                $"Added : {addedSize}\n" +
                $"Image : {imageSize}");
        }

        public void DeleteEntryAt(int index)
        {
            SearchResults = null;
            TitleMatrix.DeleteArrayElementAtIndex(index);
            DescriptionMatrix.DeleteArrayElementAtIndex(index);
            GenresMatrix.DeleteArrayElementAtIndex(index);
            RatingMatrix.DeleteArrayElementAtIndex(index);
            YearMatrix.DeleteArrayElementAtIndex(index);
            DateMatrix.DeleteArrayElementAtIndex(index);
            AddedMatrix.DeleteArrayElementAtIndex(index);

            ImageMatrix.GetArrayElementAtIndex(index).objectReferenceValue = null;
            ImageMatrix.DeleteArrayElementAtIndex(index);

            if (!IsSeasonal)
                LinkMatrix.DeleteArrayElementAtIndex(index);
            else
            {
                SeasonProxy.RemoveAt(index);
            }
        }

        public void ClearAllEntries()
        {
            SearchResults = null;
            TitleMatrix.ClearArray();
            DescriptionMatrix.ClearArray();
            GenresMatrix.ClearArray();
            RatingMatrix.ClearArray();
            YearMatrix.ClearArray();
            DateMatrix.ClearArray();
            AddedMatrix.ClearArray();
            ImageMatrix.ClearArray();
            LinkMatrix.ClearArray();
            SeasonMatrix.ClearArray();
            SeasonProxy.Import(SeasonMatrix, LinkMatrix);
        }

        public void CreateNewEntry()
        {
            SearchResults = null;
            IndexFoldout.intValue = TitleMatrix.arraySize++;
            Group = (TitleMatrix.arraySize / MaxItemPerGroup);
            DescriptionMatrix.arraySize = TitleMatrix.arraySize;
            GenresMatrix.arraySize = TitleMatrix.arraySize;
            RatingMatrix.arraySize = TitleMatrix.arraySize;
            YearMatrix.arraySize = TitleMatrix.arraySize;
            DateMatrix.arraySize = TitleMatrix.arraySize;
            AddedMatrix.arraySize = TitleMatrix.arraySize;
            ImageMatrix.arraySize = TitleMatrix.arraySize;
            // SeasonMatrix.arraySize = TitleMatrix.arraySize;

            if (!IsSeasonal)
            {
                LinkMatrix.arraySize = TitleMatrix.arraySize;
                LinkMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).FindPropertyRelative("url").stringValue = string.Empty;
            } else
            {
                SeasonProxy.AddEntry();
            }

            TitleMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).stringValue = string.Empty;
            DescriptionMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).stringValue = string.Empty;
            GenresMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).intValue = 0;
            RatingMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).floatValue = 0;
            YearMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).intValue = 0;
            DateMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).intValue = 0;
            AddedMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).longValue = DateTimeToUnixTimeStamp(DateTime.Now);
            ImageMatrix.GetArrayElementAtIndex(IndexFoldout.intValue).objectReferenceValue = null;
            // SeasonMatrix.GetArrayElementAtIndex(IndexFoldout).stringValue = string.Empty;
            // Debug.Log("Created new entry: " + IndexFoldout.intValue);
        }

        public void ApplyTextureSettings(Texture2D texture)
        {
            var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
            importer.maxTextureSize = 128;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.SaveAndReimport();
        }

        public class MediaImporterTemp
        {
            const string @namespace = "UdonSharp.Video";
            const string @class_info = "More_info";
            const string @class_seri = "More_info_seri";

            const string field_title_name = "titulo";
            const string field_urls_name = "url_video"; // VRCUrl array
            const string field_desc_name = "descripcion";
            const string field_year_name = "year";
            const string field_genre_name = "genero";

            public bool CanImportInfo { get; private set; }
            public bool CanImportSeri { get; private set; }

            private Type info_type;
            private Type seri_type;

            private FieldInfo info_field_title;
            private FieldInfo info_field_urls;
            private FieldInfo info_field_desc;
            private FieldInfo info_field_year;
            private FieldInfo info_field_genre;

            private FieldInfo seri_field_title;
            private FieldInfo seri_field_urls;
            private FieldInfo seri_field_desc;
            private FieldInfo seri_field_year;
            private FieldInfo seri_field_genre;

            public Type FindTypeByName(string className)
            {
                var type = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from __type in assembly.GetTypes()
                            where __type.FullName == className
                            select __type);
                return type == null ? null : type.FirstOrDefault();
            }

            public MediaImporterTemp()
            {
                info_type = FindTypeByName(String.Format("{0}.{1}", @namespace, @class_info));
                seri_type = FindTypeByName(String.Format("{0}.{1}", @namespace, @class_seri));
                CanImportInfo = info_type != null;
                CanImportSeri = seri_type != null;

                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                if (CanImportInfo)
                {
                    info_field_title = info_type.GetField(field_title_name, flags);
                    info_field_urls  = info_type.GetField(field_urls_name, flags);
                    info_field_desc  = info_type.GetField(field_desc_name, flags);
                    info_field_year  = info_type.GetField(field_year_name, flags);
                    info_field_genre = info_type.GetField(field_genre_name, flags);
                }

                if (CanImportSeri)
                {
                    seri_field_title = seri_type.GetField(field_title_name, flags);
                    seri_field_urls  = seri_type.GetField(field_urls_name, flags);
                    seri_field_desc  = seri_type.GetField(field_desc_name, flags);
                    seri_field_year  = seri_type.GetField(field_year_name, flags);
                    seri_field_genre = seri_type.GetField(field_genre_name, flags);
                }
            }

            public InfoContent GetOldSeriInfoFrom(GameObject gameObject)
            {
                Component comp = gameObject.GetComponent(seri_type);
                InfoContent info = new InfoContent();
                if (comp == null) return info;

                info.title = (string)seri_field_title.GetValue(comp);
                info.urls = (VRCUrl[])seri_field_urls.GetValue(comp);
                info.description = (string)seri_field_desc.GetValue(comp);
                info.year = (string)seri_field_year.GetValue(comp);
                info.genres = ((string)seri_field_genre.GetValue(comp)).Split(',');
                for (int j = 0; j < info.genres.Length; j++)
                    info.genres[j] = info.genres[j].ToLowerInvariant().Trim();

                Image imageComp = comp.GetComponent<Image>();
                if (imageComp)
                {
                    Sprite sprite = imageComp.sprite;
                    if (sprite != null)
                        info.image = sprite.texture;
                    else
                        Debug.Log("No se ha encontrado un sprite.", comp.gameObject);
                }

                return info;
            }
            public InfoContent[] GetOldInfoFrom(GameObject gameObject)
            {
                Component[] components = gameObject.GetComponentsInChildren(info_type, true);
                InfoContent[] infos = new InfoContent[components.Length];
                for (int i = 0; i < components.Length; i++)
                {
                    Component comp = components[i];
                    InfoContent info = new InfoContent();

                    info.title = (string)info_field_title.GetValue(comp);
                    info.urls = (VRCUrl[])info_field_urls.GetValue(comp);
                    info.description = (string)info_field_desc.GetValue(comp);
                    info.year = (string)info_field_year.GetValue(comp);
                    info.genres = ((string)info_field_genre.GetValue(comp)).Split(',');
                    for (int j = 0; j < info.genres.Length; j++)
                        info.genres[j] = info.genres[j].ToLowerInvariant().Trim();

                    infos[(components.Length - 1) - i] = info;
                    Image imageComp = comp.GetComponent<Image>();
                    if (imageComp) {
                        Sprite sprite = imageComp.sprite;
                        if (sprite != null)
                            info.image = sprite.texture;
                        else
                            Debug.Log("No se ha encontrado un sprite.", comp.gameObject);
                    }
                }

                return infos;
            }

            public class InfoContent
            {
                public string title;
                public string description;
                public string year;
                public short year_num {
                    get {
                        if (short.TryParse(this.year, out short result)) return result;
                        return -1;
                    }
                }
                public string[] genres;
                public VRCUrl[] urls;
                public Texture2D image;
            }
        }

        public class SeasonProxyTemp
        {
            public bool IsDirty { get; set; }
            public List<List<List<string>>> Matrix = new List<List<List<string>>>();

            public void MarkDirty() => IsDirty = true;
            public void ClearDirty() => IsDirty = false;

            public void RemoveAt(int index)
            {
                Matrix.RemoveAt(index);
                MarkDirty();
            }
            public void AddEntry()
            {
                Matrix.Add(new List<List<string>> { new List<string>() });
                MarkDirty();
            }

            public void Import(SerializedProperty Entries, SerializedProperty Links)
            {
                Matrix.Clear();
                int entryCount = Entries.arraySize;
                
                for (int i = 0; i < entryCount; i++)
                {
                    string rawData = Entries.GetArrayElementAtIndex(i).stringValue;
                    string[] rawSplit = rawData.Split('\n');

                    var entryData = new List<List<string>>(rawSplit.Length);
                    for (int j = 0; j < rawSplit.Length; j++)
                    {
                        List<string> urlData = new List<string>();
                        string content = rawSplit[j];
                        if (string.IsNullOrEmpty(content)) continue;

                        string[] rawIndexData = content.Split(' ');

                        if (int.TryParse(rawIndexData[0], out int start) && int.TryParse(rawIndexData[1], out int end))
                        {
                            for (int t = start; t < end; t++)
                            {
                                string url = Links.GetArrayElementAtIndex(t).FindPropertyRelative("url").stringValue;
                                urlData.Add(url);
                            }
                        }

                        entryData.Add(urlData);
                    }

                    Matrix.Add(entryData);
                }
            }

            public void Export(SerializedProperty Entries, SerializedProperty Links)
            {
                (string[] showData, string[] urlData) = this.Export();
                Entries.arraySize = showData.Length;
                Links.arraySize = urlData.Length;
                for (int i = 0; i < showData.Length; i++)
                    Entries.GetArrayElementAtIndex(i).stringValue = showData[i];
                for (int i = 0; i < urlData.Length; i++)
                    Links.GetArrayElementAtIndex(i).FindPropertyRelative("url").stringValue = urlData[i];
            }

            public (string[], string[]) Export()
            {
                List<string> urls = new List<string>();
                StringBuilder sb = new StringBuilder();

                int showCount = Matrix.Count;
                string[] showData = new string[showCount];
                for (int entryIndex = 0, c = 0; entryIndex < showCount; entryIndex++)
                {
                    List<List<string>> rawData = Matrix[entryIndex];
                    sb.Clear();

                    for (int seasonIndex = 0; seasonIndex < rawData.Count; seasonIndex++)
                    {
                        List<string> data = rawData[seasonIndex];
                        c = data.Count;
                        if (sb.Length > 0) sb.Append('\n');
                        sb.Append($"{urls.Count} {urls.Count + c} {c}");
                        urls.AddRange(data);
                    }

                    showData[entryIndex] = sb.ToString();
                }

                return (showData, urls.ToArray());
            }
        }
    }
}