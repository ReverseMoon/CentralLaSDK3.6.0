// UnityEngine
using UnityEngine;
using UnityEngine.UI;
// TMP
using TMPro;
// VRChat
using UdonSharp;
using VRC.SDKBase;
using UdonSharp.Video;
using VRC.SDK3.Components;
using UnityEngine.Video;
using JetBrains.Annotations;

namespace Kittenji
{
    // TODO:
    // Handle string search too

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CentralMediaLibrary : UdonSharpBehaviour
    {
        #region Logger
        public bool EnableDebug = false;
        const string LOGGER_PREFIX = "[<color=red>CentralMediaLibrary</color>] ";
        void LogMessage(string message) => Debug.Log(LOGGER_PREFIX + message);
        void LogWarning(string message) => Debug.LogWarning(LOGGER_PREFIX + message);
        void LogError(string message) => Debug.LogError(LOGGER_PREFIX + message);
        void LogError(string message, Object context) => Debug.LogError(LOGGER_PREFIX + message, context);
        #endregion

        #region Library
        private int MediaColumnCount = 4;
        public int MediaRowCount = 4;
        public int MediaCount => MediaColumnCount * MediaRowCount;
        public int ChapterCount = 45;
        public double PlayerCooldown = 60.0;

        public Animator Animator;
        public CentralMediaCategory[] Categories;
        public CentralMediaCategory CurrentCategory;
        public int SelectedIndex = -1;
        public int CategoryIndex
        {
            get { return m_CategoryIndex; }
            set {
                m_CategoryIndex = value;
                CurrentCategory = value < 0 ? null : Categories[value];
            }
        }
        private int m_CategoryIndex = -1;

        private int FilterYearIndex = -1;
        private short FilterYear => FilterYearIndex < 0 ? (short)-1 : CurrentCategory.YearList[FilterYearIndex];

        private int FilterGenreIndex = -1;
        private int FilterGenre => FilterGenreIndex < 0 ? -1 : CurrentCategory.ActiveGenres[FilterGenreIndex];

        private int FilterRating = -1;
        private string FilterSearch = null;

        private bool FilterActive => FilterRating > -1 || FilterGenreIndex > -1 || FilterYearIndex > -1 || !string.IsNullOrWhiteSpace(FilterSearch);


        private CentralMediaSensor CurrentSensor;
        // 0: Default
        // 1: Rating
        // 2: Date
        // 3: Name
        private int SortingBy = 0;
        private bool SortingAlt = false;

        // Seasonal Data
        private VRCUrl[][] ParsedSeasonData;
        private int SelectedSeason = -1;
        private int SelectedChapter = -1;
        private int ChapterPage = 0;
        #endregion

        #region UI Elements
        // Containers
        public RectTransform CategoryContainer;
        public RectTransform MediaContainer;
        private GridLayoutGroup MediaContainerGroup;
        public RectTransform SortingContainer;

        public RectTransform RatingContainer; // Filters
        public RectTransform GenreContainer;
        public RectTransform YearContainer;

        public RectTransform SeasonContainer; // Seasonal
        public RectTransform ChapterContainer;

        // Search
        public InputField SearchField;
        public Button PageButtonPrev;
        public Button PageButtonNext;
        public TextMeshProUGUI PageTextLabel;

        public Button ChapterPageButtonPrev;
        public Button ChapterPageButtonNext;
        public TextMeshProUGUI ChapterPageLabel;

        public GameObject SearchFailed;

        // Information Panel
        public GameObject InfoPanel;
        public GameObject DataPanel; // For Seasonal
        public TextMeshProUGUI TitleLabel;
        public TextMeshProUGUI DescriptionLabel;
        public TextMeshProUGUI RatingLabel;
        public Image RatingRadial;
        public TextMeshProUGUI GenresLabel;
        public TextMeshProUGUI DateLabel;
        public RawImage ThumbnailImage;
        public TextMeshProUGUI PlayLabel;

        // Audio
        public AudioSource AudioSource;
        [Space(2)]
        public AudioClip ClipPanelOpen;
        public AudioClip ClipPanelClose;
        [Space(2)]
        public AudioClip ClipSocialOpen;
        public AudioClip ClipSocialClose;
        [Space(2)]
        public AudioClip ClipInfoOpen;
        public AudioClip ClipInfoClose;
        [Space(2)]
        public AudioClip ClipFilterClick;
        public AudioClip ClipSortClick;
        [Space(2)]
        public AudioClip ClipPageNext;
        public AudioClip ClipPagePrev;
        [Space(2)]
        public AudioClip ClipCategoryClick;
        public AudioClip ClipSearchTitle;
        [Space(2)]
        public AudioClip ClipPlayerCooldown;
        public AudioClip ClipPlayerOkay;

        // Prefab Cache
        private GameObject MediaPrefab;
        private GameObject GenrePrefab;
        private GameObject YearPrefab;
        private GameObject SeasonPrefab;
        private GameObject ChapterPrefab;
        #endregion

        #region Initialization
        public bool Initialized;

        public void Start()
        {
            if (!Initialized)
            {
                MediaContainerGroup = MediaContainer.GetComponent<GridLayoutGroup>();
                MediaColumnCount = MediaContainerGroup.constraintCount; // Fixed column count

                InstantiateCategories();
                FetchListingPrefabs();
                SelectCategory();

                RemoteTimestamp = new double[RemoteTimestampLength];
                LocalCooldown = new float[RemoteTimestampLength];

                SyncModeController syncModeController = GetComponentInChildren<SyncModeController>(true);
                if (syncModeController != null) syncModeController.SetAnimator(Animator);
                else LogError("Could not find a SyncModeController.");

                Initialized = true;
            }
        }

        private void FetchListingPrefabs()
        {
            Transform child = MediaContainer.GetChild(0);
            if (Utilities.IsValid(child)) MediaPrefab = child.gameObject;
            else LogError("Could not find Media Prefab.", gameObject);
            MediaPrefab.SetActive(false);

            child = GenreContainer.GetChild(0);
            if (Utilities.IsValid(child)) GenrePrefab = child.gameObject;
            else LogError("Could not find Genre Prefab.", gameObject);
            GenrePrefab.SetActive(false);

            child = YearContainer.GetChild(0);
            if (Utilities.IsValid(child)) YearPrefab = child.gameObject;
            else LogError("Could not find Year Prefab.", gameObject);
            YearPrefab.SetActive(false);

            child = SeasonContainer.GetChild(0);
            if (Utilities.IsValid(child)) SeasonPrefab = child.gameObject;
            else LogError("Could not find Season Prefab.", gameObject);
            SeasonPrefab.SetActive(false);

            child = ChapterContainer.GetChild(0);
            if (Utilities.IsValid(child)) ChapterPrefab = child.gameObject;
            else LogError("Could not find Chapter Prefab.", gameObject);
            ChapterPrefab.SetActive(false);
        }

        private bool InstantiateCategories()
        {
            if (!Utilities.IsValid(CategoryContainer))
            {
                LogError($"CentralMediaLibrary.CategoryContainer was not properly defined.", gameObject);
                return false;
            }

            GameObject categoryElementPrefab = null;
            int categoryPrefabIndex = 0;
            foreach (Transform child in CategoryContainer.transform)
            {
                if (!child.gameObject.activeSelf)
                {
                    categoryElementPrefab = child.gameObject;
                    categoryPrefabIndex = child.GetSiblingIndex();
                    break;
                }
            }

            if (!categoryElementPrefab)
            {
                LogError("Couldn't find a desirable prefab for the Category Container.", gameObject);
                return false;
            }

            Toggle categoryElementToggle = categoryElementPrefab.GetComponent<Toggle>();
            if (!categoryElementToggle)
            {
                LogError("Desired category element prefab for the container doesn't have a Toggle component attached.", gameObject);
                return false;
            }
            else
            {
                categoryElementToggle.SetIsOnWithoutNotify(false);
            }

            if (Categories == null || Categories.Length == 0)
                Categories = transform.GetComponentsInChildren<CentralMediaCategory>();

            if (Categories == null || Categories.Length == 0)
            {
                LogError("Category list is empty.", gameObject);
                return false;
            }

            for (int i = 0; i < Categories.Length; i++)
            {
                CentralMediaCategory category = Categories[i];

                GameObject go = Instantiate(categoryElementPrefab, CategoryContainer, false);
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.SetSiblingIndex(++categoryPrefabIndex);

                Toggle toggle = rect.GetComponent<Toggle>();


                Image image = (Image)toggle.targetGraphic;
                if (!image) image = toggle.GetComponentInChildren<Image>();

                //Image image = toggle.GetComponentInChildren<Image>();
                if (!image)
                {
                    LogError("Couldn't find a proper Image component for the category icon.", gameObject);
                    return false;
                }

                image.sprite = category.Icon;
                toggle.SetIsOnWithoutNotify(i == 0);

                category.Toggle = toggle;

                go.name = category.name;
                go.SetActive(true);
            }

            return true;
        }
        #endregion

        #region Public API
        [PublicAPI]
        public string GetCurrentTitleAt(int category, int entry)
        {
            return Categories[category].TitleMatrix[entry];
        }
        #endregion

        #region API
        public void SetSensor(CentralMediaSensor sensor)
        {
            ControlHandler.SetControlledVideoPlayer(sensor.TargetVideoPlayer);
            CurrentSensor = sensor;
            RemoteTimestampIndex = sensor.TargetID;
            SetCooldown(false, false); // Update cooldown text
        }
        void DisableToggleGroup(Toggle[] toggles)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    toggle.isOn = false;
                    break;
                }
            }
        }
        void ClearFilters(bool clearToggles)
        {
            FilterYearIndex = -1;
            FilterGenreIndex = -1;
            FilterRating = -1;
            FilterSearch = null;

            if (clearToggles)
            {
                Toggle[] toggles = YearContainer.GetComponentsInChildren<Toggle>();
                DisableToggleGroup(toggles);
                toggles = GenreContainer.GetComponentsInChildren<Toggle>();
                DisableToggleGroup(toggles);
                toggles = RatingContainer.GetComponentsInChildren<Toggle>();
                DisableToggleGroup(toggles);
            }

            RefreshFilters();
        }
        private void SetSortingDirection(bool value)
        {
            SortingAlt = value;
            SortingContainer.GetChild(SortingBy)
                .GetChild(0).transform.localScale = new Vector3(1,value ? -1 : 1, 1);
            RefreshFilters();
        }
        void RefreshFilters()
        {
            PageHistory = new int[] { 0 };
            ElementIndex = 0;
            ElementLength = 0;
            ElementPage = 0;
            ElementUpdating = true;
            SetNavigationButtons(false, false);
            CloseInfoPanel();
            DisableUnusedEntries(0);
        }
        void SetNavigationButtons(bool pEnabled, bool nEnabled)
        {
            PageButtonPrev.gameObject.SetActive(pEnabled);
            PageButtonNext.gameObject.SetActive(nEnabled && !ElementUpdating);
            bool noResults = ElementLength == 0 && !ElementUpdating;
            if (string.IsNullOrWhiteSpace(FilterSearch))
            {
                bool active = pEnabled || nEnabled || ElementUpdating || ElementLength == 0;
                PageTextLabel.gameObject.SetActive(active);
                if (noResults) PageTextLabel.text = "Sin resultados...";
                else if (active) PageTextLabel.text = ElementUpdating ? $"Buscando..." : $"Página {ElementPage + 1}";
            } else {
                PageTextLabel.gameObject.SetActive(true);
                PageTextLabel.text = noResults ? $"Sin resultados: <color=grey>{FilterSearch}</color>" : $"Buscando: <color=grey>{FilterSearch}</color>";
            }

            SearchFailed.SetActive(ElementLength == 0 && !ElementUpdating);
        }
        void SetChapterNavigationButtons(bool pEnabled, bool nEnabled)
        {
            ChapterPageButtonNext.gameObject.SetActive(nEnabled);
            ChapterPageButtonPrev.gameObject.SetActive(pEnabled);
            bool active = pEnabled || nEnabled;
            ChapterPageLabel.gameObject.SetActive(active);
        }
        void DisableUnusedEntries(int index)
        {
            Transform tr;
            if (index == 0)
            {
                MediaContainer.anchoredPosition = Vector2.zero;
            }
            for (index += 1; index < MediaContainer.childCount; index++)
            {
                tr = MediaContainer.GetChild(index);
                if (tr.gameObject.activeSelf)
                    tr.gameObject.SetActive(false);
                else break;
            }
        }
        int GetNormalizedIndex(int index)
        {
            switch (SortingBy)
            {
                case 1: // Rating
                    return SortingAlt ? CurrentCategory.SortedRatingAlt[index] : CurrentCategory.SortedRating[index];
                case 2: // Date
                    return SortingAlt ? CurrentCategory.SortedDateAlt[index] : CurrentCategory.SortedDate[index];
                case 3: // Name
                    return SortingAlt ? CurrentCategory.SortedTitleAlt[index] : CurrentCategory.SortedTitle[index];
                default: // Default (Added Date)
                    return SortingAlt ? index : (CurrentCategory.Length - index) - 1;
            }
        }
        void PlayAudioClip(AudioClip clip, bool oneShot = true, float pitch = 1)
        {
            if (Utilities.IsValid(AudioSource) && Utilities.IsValid(clip))
            {
                AudioSource.pitch = pitch;
                if (oneShot) AudioSource.PlayOneShot(clip);
                else {
                    if (AudioSource.isPlaying) AudioSource.Stop();
                    AudioSource.clip = clip;
                    AudioSource.Play();
                }
            }
        }
        void ChapterPageUpdate()
        {
            VRCUrl[] data = ParsedSeasonData[SelectedSeason];
            int count = data.Length;
            int pages = Mathf.CeilToInt(count / (float)ChapterCount);
            if (ChapterPage < 0) ChapterPage = pages - 1;
            else if (ChapterPage >= pages) ChapterPage = 0;

            if (pages == 1) SetChapterNavigationButtons(false, false);
            else
            {
                Debug.Log("Enabling pages");
                SetChapterNavigationButtons(true, true);
                ChapterPageLabel.text = $"Página {ChapterPage + 1} de {pages}";
            }

            int childCount = ChapterContainer.childCount - 1;
            int entryCount = ChapterPage + 1 < pages ? ChapterCount : count % ChapterCount;
            int length = Mathf.Max(childCount, entryCount);
            Transform child;
            GameObject target;
            for (int j = 0; j < length; j++)
            {
                if (j < childCount)
                {
                    child = ChapterContainer.GetChild(j + 1);
                    target = child.gameObject;

                    if (j >= entryCount)
                    {
                        target.SetActive(false);
                        continue;
                    }
                }
                else target = Instantiate(ChapterPrefab, ChapterContainer, false);

                TextMeshProUGUI label = target.GetComponentInChildren<TextMeshProUGUI>();
                label.text = $"{j + (ChapterCount * ChapterPage) + 1}";
                if (!label.gameObject.activeSelf) label.gameObject.SetActive(true);
                target.SetActive(true);
            }
        }
        #endregion

        #region Video API
        public VideoControlHandler ControlHandler;
        public VRCUrlInputField URLInputField;

        [UdonSynced]
        public double[] RemoteTimestamp;
        public int RemoteTimestampLength = 1;
        private int RemoteTimestampIndex = 0;
        private float[] LocalCooldown;
        private float Cooldown
        {
            get { return LocalCooldown[RemoteTimestampIndex]; }
            set { LocalCooldown[RemoteTimestampIndex] = value; }
        }

        USharpVideoPlayer VideoPlayer
            => Utilities.IsValid(ControlHandler) ? ControlHandler.targetVideoPlayer : null;

        /*
        void LoadPlaylist(VRCUrl[] urls, int index)
        {
            if (CanPlayMedia())
            {
                SetCooldown(true, true);
                VideoPlayer.SetLibraryIndex(CategoryIndex, SelectedIndex, SelectedSeason);
                // VideoPlayer.playlist = urls;
                VideoPlayer.PlayVideo(urls[index]);
                // VideoPlayer.SetNextPlaylistVideo(index + 1);
                LogMessage("Loading playlist...");

                PlayAudioClip(ClipPlayerOkay);
            }
        }
        */

        void LoadURL(VRCUrl url)
        {
            if (CanPlayMedia())
            {
                SetCooldown(true, true);
                VideoPlayer.SetLibraryIndex(CategoryIndex, SelectedIndex, SelectedSeason, SelectedChapter);
                // VideoPlayer.playlist = new VRCUrl[] { url };
                VideoPlayer.PlayVideo(url);
                LogMessage("Loading url: " + url);

                PlayAudioClip(ClipPlayerOkay);
            }
        }

        public void SetCooldown(bool owner, bool value)
        {
            if (value)
            {
                if (owner && !Networking.IsOwner(gameObject))
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);

                double ServerTimestamp = Networking.GetServerTimeInSeconds();
                // LogMessage("Server Timestamp: " + ServerTimestamp);

                if (owner)
                {
                    RemoteTimestamp[RemoteTimestampIndex] = ServerTimestamp + PlayerCooldown;
                    RequestSerialization();
                }

                float timespan;
                for (int i = 0; i < RemoteTimestampLength; i++)
                {
                    double timestamp = RemoteTimestamp[i];
                    if (timestamp < double.Epsilon && timestamp > -double.Epsilon)
                    {
                        LocalCooldown[i] = -1;
                        continue;
                    }

                    timespan = (float)System.Math.Min(System.Math.Abs(timestamp - ServerTimestamp), PlayerCooldown);
                    if (timespan > 0)
                    {
                        LocalCooldown[i] = Time.unscaledTime + timespan;
                        // Time remaining in seconds...
                        if (i == RemoteTimestampIndex)
                        {
                            PlayLabel.transform.GetChild(0)
                                .gameObject.SetActive(false);
                            PlayLabel.transform.GetChild(1)
                                .gameObject.SetActive(true);
                        }
                    }
                }
            } else {
                PlayLabel.transform.GetChild(0)
                    .gameObject.SetActive(true);
                PlayLabel.transform.GetChild(1)
                    .gameObject.SetActive(false);
                PlayLabel.text = "Reproducir";
            }
        }

        // This is for UI
        public void OnURLInput()
        {
            if (CanPlayMedia())
            {
                CloseInfoPanel();
                SelectedIndex = -1;
                SelectedChapter = -1;
                SelectedSeason = -1;
                SelectedChapter = -1;
                LoadURL(URLInputField.GetUrl());
            }
            else PlayAudioClip(ClipPlayerCooldown, false, Random.Range(0.9f, 1.2f));
            URLInputField.SetUrl(VRCUrl.Empty);
        }

        public bool CanPlayMedia()
        {
            return Utilities.IsValid(VideoPlayer) && VideoPlayer.CanControlVideoPlayer() && Cooldown <= 0;
        }
        #endregion

        #region VRChat Networking
        public override void OnDeserialization()
        {
            SetCooldown(false, true);

            /*
            LogMessage("Deserializing Library:");
            for (int i = 0; i < RemoteTimestamp.Length; i++)
            {
                LogMessage($"[{i}] RemoteTimestamp - {RemoteTimestamp[i]}\nLocalTimestamp - {LocalCooldown[i]}");
            }
            */
        }
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (Networking.IsOwner(gameObject))
                RequestSerialization();
        }
        #endregion

        #region Update | Instantiation
        private int[] PageHistory = new int[] { 0 };
        private int ElementPage;
        private int ElementIndex;
        private int ElementLength;
        private bool ElementUpdating;

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.P))
            {
                Networking.LocalPlayer.TeleportTo(new Vector3(-21.91f, 3.8f, -13.01f), Quaternion.identity);
            }
            */

            if (Cooldown > 0) // Handle Cooldown
            {
                if (Time.unscaledTime > Cooldown)
                {
                    Cooldown = -1;
                    SetCooldown(false, false);
                } else
                {
                    float elapsed = Cooldown - Time.unscaledTime;
                    int minutes = Mathf.FloorToInt(elapsed / 60);
                    float seconds = elapsed % 60;
                    float msec = (elapsed % 1) * 100.0f;
                    PlayLabel.text = "<mspace=0.55em>" + minutes.ToString("00") + ":" + seconds.ToString("00") + "." + msec.ToString("00");
                }
            }

            if (!ElementUpdating) return;

            if (ElementIndex < CurrentCategory.Length && ElementLength < MediaCount)
            {
                int count = Mathf.Min(ElementIndex + MediaColumnCount, CurrentCategory.Length);
                for (; ElementIndex < count && ElementLength < MediaCount; ElementIndex++)
                {
                    int index = GetNormalizedIndex(ElementIndex);
                    string title = CurrentCategory.GetTitleAt(index);
                    if (FilterActive)
                    {
                        bool pass = true;
                        if (FilterRating > -1)
                        {
                            float rating = CurrentCategory.GetRatingAt(index);
                            pass = pass && rating <= FilterRating && rating >= FilterRating - 1;
                        }
                        if (FilterGenre > -1)
                        {
                            int genreFlags = CurrentCategory.GetGenresAt(index);
                            pass = pass && (genreFlags & (1 << FilterGenre)) != 0;
                        }
                        if (FilterYear > -1)
                        {
                            int year = CurrentCategory.GetYearAt(index);
                            pass = pass && year == FilterYear;
                        }
                        if (!string.IsNullOrWhiteSpace(FilterSearch))
                            pass = pass && title.ToLowerInvariant().Contains(FilterSearch.ToLowerInvariant());

                        if (!pass) continue;
                    }

                    int element = ++ElementLength;
                    GameObject go = element < MediaContainer.childCount ? MediaContainer.GetChild(element).gameObject : Instantiate(MediaPrefab, MediaContainer, false);

                    TextMeshProUGUI label = go.GetComponentInChildren<TextMeshProUGUI>();
                    label.text = title;
                    RawImage rawImage = go.GetComponent<RawImage>();
                    rawImage.texture = CurrentCategory.GetImageAt(index);

                    go.name = index.ToString();
                    go.SetActive(true);
                }
            } else
            {
                ElementUpdating = false;
                DisableUnusedEntries(ElementLength);
                SetNavigationButtons(ElementPage > 0, ElementIndex < CurrentCategory.Length);
                LogMessage("Done updating entries.");
            }
        }
        #endregion

        #region UI Events
        // Player
        public void PlaySelectedMedia()
        {
            if (SelectedIndex > -1 && Utilities.IsValid(CurrentSensor))
            {
                bool isSeasonal = CurrentCategory.Seasonal;
                bool canPlay = CanPlayMedia();
                if (isSeasonal)
                {
                    Transform trans = null;
                    for (int i = 1; i < ChapterContainer.childCount; i++)
                    {
                        trans = ChapterContainer.GetChild(i);
                        if (!trans.gameObject.activeSelf) break;

                        trans = trans.GetChild(0);
                        if (!trans.gameObject.activeSelf)
                        {
                            trans.gameObject.SetActive(true);
                            if (canPlay)
                            {
                                SelectedChapter = (i - 1) + (ChapterPage * ChapterCount);
                                LoadURL(ParsedSeasonData[SelectedSeason][SelectedChapter]);
                                trans = null;
                            }
                            break;
                        }
                    }

                    if (canPlay && ChapterContainer.childCount > 0 && trans != null)
                    {
                        SelectedChapter = 0;
                        LoadURL(ParsedSeasonData[SelectedSeason][SelectedChapter]);
                    }
                }
                else if (canPlay)
                {
                    SelectedSeason = -1;
                    SelectedChapter = -1;
                    VRCUrl url = CurrentCategory.GetUrlAt(SelectedIndex);
                    LoadURL(url);
                }
                
                if (!canPlay) PlayAudioClip(ClipPlayerCooldown, false, Random.Range(0.9f, 1.2f));
            }
        }
        // Panel
        public void OpenMainPanel()
        {
            if (!Animator.GetBool("MainPanel"))
            {
                Animator.SetBool("MainPanel", true);
                PlayAudioClip(ClipPanelOpen);
            }
        }
        public void CloseMainPanel()
        {
            if (Animator.GetBool("MainPanel"))
            {
                Animator.SetBool("MainPanel", false);
                PlayAudioClip(ClipPanelClose);
            }
        }
        // Info
        public void OpenInfoPanel()
        {
            if (!Animator.GetBool("InfoPanel"))
            {
                Animator.SetBool("InfoPanel", true);
                PlayAudioClip(ClipInfoOpen);
            }
        }
        public void CloseInfoPanel()
        {
            if (Animator.GetBool("InfoPanel"))
            {
                Animator.SetBool("InfoPanel", false);
                PlayAudioClip(ClipInfoClose);
            }
        }
        // Social
        public void OpenSocialPanel()
        {
            if (!Animator.GetBool("SocialPanel"))
            {
                Animator.SetBool("SocialPanel", true);
                PlayAudioClip(ClipSocialOpen, false);
            }
        }
        public void CloseSocialPanel()
        {
            if (Animator.GetBool("SocialPanel"))
            {
                Animator.SetBool("SocialPanel", false);
                PlayAudioClip(ClipSocialClose, false);
            }
        }
        // Chapter Page
        public void ChapterPageNext()
        {
            ChapterPage = ChapterPage + 1;
            ChapterPageUpdate();
        }
        public void ChapterPagePrev()
        {
            VRCUrl[] data = ParsedSeasonData[SelectedSeason];
            int count = data.Length;
            int pages = Mathf.CeilToInt(count / (float)ChapterCount);
            ChapterPage = ChapterPage - 1;
            ChapterPageUpdate();
        }
        public void SelectSeason()
        {
            Toggle[] toggles = SeasonContainer.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                int index = toggle.transform.GetSiblingIndex() - 1;
                if (toggle.isOn)
                {
                    if (SelectedSeason != index)
                    {
                        SelectedSeason = index;
                        SelectedChapter = 0;
                        ChapterPage = -1;
                        ChapterPageNext();
                    }
                    break;
                }
            }
        }
        public void SelectEntry()
        {
            for (int i = 1; i < MediaContainer.childCount; i++)
            {
                Transform entry = MediaContainer.GetChild(i);
                Transform child = entry.GetChild(0);
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    if (int.TryParse(entry.name, out int index))
                    {
                        SelectedIndex = index;

                        bool isSeasonal = CurrentCategory.Seasonal;
                        TitleLabel.text = CurrentCategory.GetTitleAt(index);
                        float rating = CurrentCategory.GetRatingAt(index);
                        RatingLabel.text = $"{Mathf.RoundToInt(rating * 10f)}<size=4>%</size>";
                        RatingRadial.fillAmount = rating / 10f;
                        DateLabel.text = CurrentCategory.GetYearAt(index).ToString();
                        if (isSeasonal)
                        {
                            ParsedSeasonData = CurrentCategory.ParseUrlsAt(index);
                            if (ParsedSeasonData == null)
                                LogError("Invalid seasonal data at " + index);
                            else
                            {
                                int childCount = SeasonContainer.childCount - 1;
                                int entryCount = ParsedSeasonData.Length;
                                int length = Mathf.Max(childCount, entryCount);
                                GameObject target;
                                for (int j = 0; j < length; j++)
                                {
                                    if (j < childCount)
                                    {
                                        child = SeasonContainer.GetChild(j + 1);
                                        target = child.gameObject;

                                        if (j >= entryCount)
                                        {
                                            target.SetActive(false);
                                            continue;
                                        }
                                    }
                                    else target = Instantiate(SeasonPrefab, SeasonContainer, false);

                                    TextMeshProUGUI label = target.GetComponentInChildren<TextMeshProUGUI>();
                                    label.text = $"Temporada {j + 1}";
                                    Toggle toggle = target.GetComponent<Toggle>();
                                    toggle.SetIsOnWithoutNotify(j == 0);
                                    target.SetActive(true);
                                }

                                SelectedSeason = -1;
                                SelectSeason();
                            }

                            DataPanel.SetActive(true);
                            DateLabel.text = DateLabel.text + $" • {ParsedSeasonData.Length} Temporadas";
                        } else DataPanel.SetActive(false);
                        DescriptionLabel.text = CurrentCategory.GetDescriptionAt(index);
                        GenresLabel.text = CurrentCategory.GetGenreStringAt(index);
                        ThumbnailImage.texture = CurrentCategory.GetImageAt(index);

                        OpenInfoPanel();
                    } else LogError("Failed to parse entry index.");

                    return;
                }
            }
        }
        // Paging
        public void PrevPage()
        {
            ElementPage -= 1;
            ElementIndex = PageHistory[ElementPage];
            ElementLength = 0;
            ElementUpdating = true;
            SetNavigationButtons(ElementPage > 0, false);
            DisableUnusedEntries(0);
            PlayAudioClip(ClipPagePrev);
        }
        public void NextPage()
        {
            if (ElementUpdating) return;

            ElementPage += 1;
            if (ElementPage == PageHistory.Length)
            {
                int[] array = new int[PageHistory.Length + 1];
                PageHistory.CopyTo(array, 0);
                PageHistory = array;
            }
            PageHistory[ElementPage] = ElementIndex;
            ElementLength = 0;
            ElementUpdating = true;
            SetNavigationButtons(ElementPage > 0, false);
            DisableUnusedEntries(0);
            PlayAudioClip(ClipPageNext);
        }
        // Sorting
        public void InvertSorting()
        {
            SetSortingDirection(!SortingAlt);
            PlayAudioClip(ClipSortClick, false, Mathf.Pow(2, (SortingAlt ? 10 : 8) / 12.0f));
        }
        public void SelectSorting()
        {
            Toggle[] toggles = SortingContainer.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                int index = toggle.transform.GetSiblingIndex();
                if (toggle.isOn)
                {
                    if (SortingBy != index)
                    {
                        SortingBy = index;
                        SetSortingDirection(false);
                        PlayAudioClip(ClipSortClick, false, Mathf.Pow(2, index / 12.0f));
                    }
                    break;
                }
            }
        }
        // Search
        public void SelectSearchString()
        {
            string text = SearchField.text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                if (!string.IsNullOrWhiteSpace(FilterSearch))
                {
                    FilterSearch = null;
                    RefreshFilters();
                }
            } else {
                FilterSearch = text.Trim();
                RefreshFilters();
            }

            PlayAudioClip(ClipSearchTitle);
        }
        // Filters
        public void ClearAllFilters()
        {
            ClearFilters(true);
            PlayAudioClip(ClipFilterClick, false, 0.5f);
        }
        public void SelectGenre()
        {
            bool selected = false;
            Toggle[] toggles = GenreContainer.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    selected = true;
                    int index = toggle.transform.GetSiblingIndex() - 1;
                    if (FilterGenreIndex != index)
                    {
                        FilterGenreIndex = index;
                        PlayAudioClip(ClipFilterClick, false, (1.0f - (index / (CurrentCategory.ActiveGenres.Length * 2f))) + 0.2f);
                        RefreshFilters();
                    }
                    break;
                }
            }

            if (!selected)
            {
                FilterGenreIndex = -1;
                PlayAudioClip(ClipFilterClick, false, 0.5f);
                RefreshFilters();
            }
        }
        public void SelectYear()
        {
            bool selected = false;
            Toggle[] toggles = YearContainer.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    selected = true;
                    int index = toggle.transform.GetSiblingIndex() - 1;
                    if (FilterYearIndex != index)
                    {
                        FilterYearIndex = index;
                        PlayAudioClip(ClipFilterClick, false, (1.0f - (index / (CurrentCategory.YearList.Length*2f))) + 0.2f);
                        RefreshFilters();
                    }
                    break;
                }
            }

            if (!selected)
            {
                FilterYearIndex = -1;
                PlayAudioClip(ClipFilterClick, false, 0.5f);
                RefreshFilters();
            }
        }
        public void SelectRating()
        {
            bool selected = false;
            Toggle[] toggles = RatingContainer.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    selected = true;
                    int index = RatingContainer.childCount - toggle.transform.GetSiblingIndex();
                    if (FilterRating != index)
                    {
                        FilterRating = index;
                        PlayAudioClip(ClipFilterClick, false, (index / 20f) + 0.7f);
                        RefreshFilters();
                    }
                    break;
                }
            }

            if (!selected)
            {
                FilterRating = -1;
                PlayAudioClip(ClipFilterClick, false, 0.5f);
                RefreshFilters();
            }
        }
        // General
        public void SelectCategory()
        {
            for (int i = 0; i < Categories.Length; i++)
            {
                CentralMediaCategory category = Categories[i];
                if (category.IsSelected)
                {
                    if (CategoryIndex != i || CurrentCategory == null)
                    {
                        CategoryIndex = i;
                        ClearFilters(true);

                        // Instantiate filter data
                        // Genres
                        int childCount = GenreContainer.childCount-1;
                        int entryCount = CurrentCategory.ActiveGenres.Length;
                        int length = Mathf.Max(childCount, entryCount);
                        Transform child;
                        GameObject target;
                        for (int j = 0; j < length; j++)
                        {
                            if (j < childCount)
                            {
                                child = GenreContainer.GetChild(j+1);
                                target = child.gameObject;

                                if (j >= entryCount)
                                {
                                    // Destroy(target);
                                    // Let's avoid wasting memory instead
                                    target.SetActive(false);
                                    continue;
                                }
                            } else target = Instantiate(GenrePrefab, GenreContainer, false);

                            int genreInd = CurrentCategory.ActiveGenres[j];
                            string genreLabel = CurrentCategory.GenreNames[genreInd];
                            TextMeshProUGUI label = target.GetComponentInChildren<TextMeshProUGUI>();
                            label.text = genreLabel;
                            Toggle toggle = target.GetComponent<Toggle>();
                            toggle.SetIsOnWithoutNotify(false);
                            target.SetActive(true);
                        }
                        // Years
                        childCount = YearContainer.childCount - 1;
                        entryCount = CurrentCategory.YearList.Length;
                        length = Mathf.Max(childCount, entryCount);
                        for (int j = 0; j < length; j++)
                        {
                            if (j < childCount)
                            {
                                child = YearContainer.GetChild(j + 1);
                                target = child.gameObject;

                                if (j >= entryCount)
                                {
                                    // Destroy(target);
                                    // Let's avoid wasting memory instead
                                    target.SetActive(false);
                                    continue;
                                }
                            } else target = Instantiate(YearPrefab, YearContainer, false);

                            short year = CurrentCategory.YearList[j];
                            TextMeshProUGUI label = target.GetComponentInChildren<TextMeshProUGUI>();
                            label.text = year.ToString();
                            Toggle toggle = target.GetComponent<Toggle>();
                            toggle.SetIsOnWithoutNotify(false);
                            target.SetActive(true);
                        }

                        if (Initialized) PlayAudioClip(ClipCategoryClick);
                        // LogMessage($"Selected category '{CurrentCategory.Name}' at index '{CategoryIndex}'");
                    }
                    break;
                }
            }

            CloseInfoPanel();
        }
        #endregion
    }
}