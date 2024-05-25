
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Kittenji
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class CentralMediaCategory : UdonSharpBehaviour
    {
        #region Logger
        const string LOGGER_PREFIX = "[<color=red>CentralMediaCategory</color>] ";
        void LogMessage(string message) => Debug.Log(LOGGER_PREFIX + message);
        void LogWarning(string message) => Debug.LogWarning(LOGGER_PREFIX + message);
        void LogError(string message) => Debug.LogError(LOGGER_PREFIX + message);
        // Debug
        void StartDebug()
        {
            if (YearList == null) LogError("Year List is Null...");
            if (ActiveGenres == null) LogError("Active Genres is Null...");
        }
        #endregion

        private void Start()
        {
            if (Utilities.IsValid(MediaLibrary) && MediaLibrary.EnableDebug)
                StartDebug();
        }

        #region Category Properties
        // Editor Only
        [HideInInspector]
        public int IndexFoldout = -1, SeasonFoldout = -1;

        // Generated
        public short[] YearList;
        public int[] ActiveGenres;

        // UI Stuff
        public Toggle Toggle;
        public bool IsSelected => Utilities.IsValid(Toggle) ? Toggle.isOn : false;

        // Categories
        public CentralMediaLibrary MediaLibrary;
        public string Name;
        public Sprite Icon;
        public bool Seasonal;

        // Genres
        public bool GenreCustoms;
        public string[] GenreNames;
        public int[] GenreIDs;

        // Matrices
        public string[] TitleMatrix;
        public string[] DescriptionMatrix;
        public int[] GenresMatrix;
        public float[] RatingMatrix;
        public long[] AddedMatrix;
        public long[] DateMatrix;
        public short[] YearMatrix;
        public Texture2D[] ImageMatrix;
        public VRCUrl[] LinkMatrix;
        public string[] SeasonMatrix;

        //Default sorting not needed for now I think
        //public int[][] LibSortedAdded;
        public int[] SortedRating, SortedRatingAlt;
        public int[] SortedDate, SortedDateAlt;
        public int[] SortedTitle, SortedTitleAlt;
        #endregion

        #region Category Accessors
        public int Length => TitleMatrix.Length;
        // Not cheking for errors because I want to debug them and prevent them from ocurring instead
        // Get Matrices
        public string GetTitleAt(int index) => TitleMatrix[index];
        public string GetDescriptionAt(int index) => DescriptionMatrix[index];
        public int GetGenresAt(int index) => GenresMatrix[index];
        public string GetGenreStringAt(int index)
        {
            int genreFlags = GetGenresAt(index);
            string str = "";
            bool flag;
            for (int i = 0; i < GenreNames.Length; i++)
            {
                flag = (genreFlags & (1 << i)) != 0;
                if (flag)
                {
                    if (str.Length > 0) str += ", ";
                    str += GenreNames[i];
                }
            }
            return str;
        }
        public float GetRatingAt(int index) => RatingMatrix[index];
        public int GetRatingPercentageAt(int index) => Mathf.RoundToInt(RatingMatrix[index] * 10f);

        public long GetAddedAt(int index) => AddedMatrix[index];
        public long GetDateAt(int index) => DateMatrix[index];
        public DateTime GetDateTimeAt(int index)
        {
            // Unix timestamp is seconds past epoch
            long unixTimeStamp = GetDateAt(index);
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
        }
        public short GetYearAt(int index) => YearMatrix[index];

        public Texture2D GetImageAt(int index) => ImageMatrix[index];
        public VRCUrl GetUrlAt(int index) => LinkMatrix[index];

        // Complicated
        public VRCUrl[][] ParseUrlsAt(int index)
        {
            string raw = SeasonMatrix[index];
            if (string.IsNullOrWhiteSpace(raw)) return null;

            string[] seasons = raw.Split('\n');
            VRCUrl[][] data = new VRCUrl[seasons.Length][];

            int start, end, len;
            for (int i = 0, j; i < seasons.Length; i++)
            {
                string[] seasonRange = seasons[i].Split(' ');
                if (!int.TryParse(seasonRange[0], out start) || !int.TryParse(seasonRange[1], out end))
                {
                    LogError("An error ocurred trying to parse seasonal data at " + index + ":" + i);
                    break;
                }

                len = end - start;
                data[i] = new VRCUrl[len];
                for (j = 0; j < len; j++)
                {
                    // Debug.Log("Pushing url: " + j);
                    data[i][j] = LinkMatrix[j + start];
                }
            }

            return data;
        }

        public VRCUrl GetChapterAt(int index, int season, int chapter)
        {
            string raw = SeasonMatrix[index];
            string[] seasons = raw.Split('\n');
            if (season >= seasons.Length) return null;
            string[] seasonRange = seasons[season].Split(' ');
            string indexStart = seasonRange[0];
            string indexEnd = seasonRange[1];
            if (int.TryParse(indexStart, out int start) && int.TryParse(indexEnd, out int end))
            {
                start = start + chapter;
                return start < end ? LinkMatrix[start] : null;
            }
            else return null;
        }
        #endregion
    }
}